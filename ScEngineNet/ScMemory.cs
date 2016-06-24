using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ScEngineNet;
using ScEngineNet.ExtensionsNet;
using ScEngineNet.SafeElements;

namespace ScMemoryNet
{
    public sealed class ScMemory
    {
        private static ScMemoryContext baseContext;

        public static ScMemoryContext BaseContext
        {
            get { return baseContext; }
        }
        private static List<IScExtensionNet> listExtensionsNet;

        public static void Initialize(bool clearBeforeInit,string configFile, string repoPath, string extensionsPath, string netExtensionsPath)
        {
            if (!File.Exists(configFile)) { throw new Exception("Отсутствует указанный конфигурационный файл"); }
            if (!Directory.Exists(repoPath)) { throw new Exception("Отсутствует указанная директория репозитория"); }
            if (!Directory.Exists(extensionsPath)) { throw new Exception("Отсутствует указанная директория расширений"); }
            if (!Directory.Exists(netExtensionsPath)) { throw new Exception("Отсутствует указанная директория расширений .net"); }
          
            ScMemoryParams parameters = new ScMemoryParams(clearBeforeInit, configFile, repoPath, extensionsPath, netExtensionsPath);
            if (ScMemoryContext.IsMemoryInitialized() == false)
            {
                IntPtr wcontext = NativeMethods.sc_memory_initialize(parameters.scParams);
                baseContext = new ScMemoryContext(wcontext);

                ScMemory.LoadExtensionsNets(netExtensionsPath);

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
                        IScExtensionNet exNet = (IScExtensionNet)assembly.CreateInstance(t.FullName);
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



        public static bool ShutDown(bool SaveMemoryState)
        {
            bool IsShutDown = false;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                if (ScMemory.UnLoadExtensionsNet())
                {

                    IsShutDown = NativeMethods.sc_memory_shutdown(SaveMemoryState);
                    //if (IsShutDown == true)
                    //{
                    //    ScMemory.baseContext = null;
                    //};
                }
            }

            return IsShutDown;
        }



    }
}
