using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using ScEngineNet;
using ScEngineNet.ExtensionsNet;
using ScEngineNet.NetHelpers;

namespace ScMemoryNet
{
    /// <summary>
    /// Класс, имплементирующий Sc-хранилище
    /// </summary>
    public sealed class ScMemory
    {
        private static ScMemoryContext baseContext;
        /// <summary>
        /// Определяет, инициализирована ли память
        /// </summary>
        /// <value>
        /// <c>true</c> если память инициализирована, в противном случае <c>false</c>.
        /// </value>
        public static bool IsInitialized
        {
            get { return ScMemoryContext.IsMemoryInitialized(); }
        }

        /// <summary>
        /// Возвращает базовый контекст памяти.
        /// Используйте его только для чтения элементов, так как он инициализируется с максимальными правами доступа, и элементы, созданные в нем могут быть невидимы для других контекстов.
        /// </summary>
        /// <value>
        /// Базовый контекст.
        /// </value>
        public static ScMemoryContext BaseContext
        {
            get { return baseContext; }
        }

        private static List<IScExtensionNet> listExtensionsNet;

        /// <summary>
        /// Инициализирует память
        /// </summary>
        /// <param name="clearBeforeInit"> если  <c>true</c> то память очищается при инициализации.</param>
        /// <param name="configFile">Путь к конфигурационному файлу</param>
        /// <param name="repoPath">Путь к репозиторию</param>
        /// <param name="extensionsPath">Путь к расширениям</param>
        /// <param name="netExtensionsPath">Путь к расширениям .net</param>
        /// <exception cref="System.Exception">
        /// Отсутствует указанный конфигурационный файл
        /// or
        /// Отсутствует указанная директория репозитория
        /// or
        /// Отсутствует указанная директория расширений
        /// or
        /// Отсутствует указанная директория расширений .net
        /// or
        /// Память уже инициализирована. Нельзя использовать одновременно несколько экземпляров памяти. Создайте новый ScMemoryContext
        /// </exception>
        public static void Initialize(bool clearBeforeInit,string configFile, string repoPath, string extensionsPath, string netExtensionsPath)
        {
          
            var parameters = new ScMemoryParams(clearBeforeInit, configFile, repoPath, extensionsPath, netExtensionsPath);
            ScMemory.Initialize(parameters);
        }

        /// <summary>
        /// Инициализирует память
        /// </summary>
        /// <param name="parameters">Параметры памяти</param>
        /// <exception cref="System.Exception">
        /// Отсутствует указанный конфигурационный файл
        /// or
        /// Отсутствует указанная директория репозитория
        /// or
        /// Отсутствует указанная директория расширений
        /// or
        /// Отсутствует указанная директория расширений .net
        /// or
        /// Память уже инициализирована. Нельзя использовать одновременно несколько экземпляров памяти. Создайте новый ScMemoryContext
        /// </exception>
        public static void Initialize(ScMemoryParams parameters)
        {
            if (!File.Exists(parameters.ConfigFile)) { throw new Exception("Отсутствует указанный конфигурационный файл"); }
            if (!Directory.Exists(parameters.RepoPath)) { throw new Exception("Отсутствует указанная директория репозитория"); }
            if (!Directory.Exists(parameters.ExtensionsPath)) { throw new Exception("Отсутствует указанная директория расширений"); }
            if (!Directory.Exists(parameters.NetExtensionsPath)) { throw new Exception("Отсутствует указанная директория расширений .net"); }

            if (ScMemoryContext.IsMemoryInitialized() == false)
            {
                IntPtr wcontext = NativeMethods.sc_memory_initialize(parameters.scParams);
                baseContext = new ScMemoryContext(wcontext);

                ScMemory.LoadExtensionsNets(parameters.NetExtensionsPath);

                NLanguages.CreateKeyNodes();
                DataTypes.CreateKeyNodes();
            }
            else
            {
                throw new Exception("Память уже инициализирована. Нельзя использовать одновременно несколько экземпляров памяти. Создайте новый ScMemoryContext");
            }
        }




        private static void LoadExtensionsNets(string netExtensionsPath)
        {
            string[] files = Directory.GetFiles(netExtensionsPath, "*.dll");
            Console.WriteLine("** Message: Initialize .net extensions from " + netExtensionsPath);
            listExtensionsNet = new List<IScExtensionNet>();
            foreach (string fName in files)
            {
                Assembly assembly = Assembly.LoadFrom(fName);
                foreach (Type t in assembly.GetExportedTypes())
                {
                    if (typeof(IScExtensionNet).IsAssignableFrom(t))
                    {
                        var exNet = (IScExtensionNet) assembly.CreateInstance(t.FullName);
                        Console.WriteLine("** Message: Initialize .net module: " + fName);
                        exNet.Initialize();
                        listExtensionsNet.Add(exNet);
                    }
                }
            }
        }

        private static bool UnLoadExtensionsNet()
        {
            foreach (var exNet in listExtensionsNet)
            {
                Console.WriteLine("** Message: Shutdown .net module: " + exNet.NetExtensionName);
                exNet.ShutDown();
            }
            return true;
        }

        /// <summary>
        /// Останавливает движок sc-хранилища.
        /// </summary>
        /// <param name="SaveMemoryState">если установлено <c>true</c> то сохраняет состояние памяти.</param>
        /// <returns><c>true</c>, если память остановилась </returns>
        public static bool ShutDown(bool SaveMemoryState)
        {
            bool IsShutDown = false;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                if (ScMemory.UnLoadExtensionsNet())
                {
                    IsShutDown = NativeMethods.sc_memory_shutdown(SaveMemoryState);
                }
            }

            return IsShutDown;
        }
    }
}
