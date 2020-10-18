using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ScEngineNet.Native;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;


namespace ScEngineNet
{
    /// <summary>
    /// Класс, имплементирующий Sc-хранилище
    /// </summary>
    public sealed class ScMemory
    {
        //  private static ScMemoryContext baseContext;
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
        public static void Initialize(bool clearBeforeInit, string configFile, string repoPath, string extensionsPath, string netExtensionsPath)
        {

            var parameters = new ScMemoryParams(clearBeforeInit, configFile, repoPath, extensionsPath, netExtensionsPath);
           Initialize(parameters);
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
        
          
            if (ScMemoryContext.IsMemoryInitialized() == false)
            {

                NativeMethods.sc_memory_initialize(parameters.ScParams);
                //только если указанная директория существует
                if (Directory.Exists(parameters.NetExtensionsPath) )
                {
                   
                }

                ScDataTypes.Instance.CreateKeyNodes();
                ScKeyNodes.Instance.CreateKeyNodes();
            }
            else
            {
                throw new Exception("Память уже инициализирована. Нельзя использовать одновременно несколько экземпляров памяти. Лучше создайте новый ScMemoryContext");
            }

        }





       

        /// <summary>
        /// Останавливает движок sc-хранилища.
        /// </summary>
        /// <param name="saveMemoryState">если установлено <c>true</c> то сохраняет состояние памяти.</param>
        /// <returns><c>true</c>, если память остановилась </returns>
        public static bool ShutDown(bool saveMemoryState)
        {
            var isShutDown = false;

            
            //уничтожение объектов с указателями в памяти перед закрытием памяти


            //закрытие расширений и закрытие библиотеки
            if (ScMemoryContext.IsMemoryInitialized())
            {
               
                    isShutDown = NativeMethods.sc_memory_shutdown(saveMemoryState);
                
            }


            return isShutDown;
        }







    }
}
