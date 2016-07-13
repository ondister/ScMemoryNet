using System;

using ScEngineNet.SafeElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
namespace Demo
{


    /// <summary>
    /// Демо приложение для проверки работоспособности. Можно использовать как быстрый старт
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            const string configFile = @"d:\OSTIS\sc-machine-iot\bin\sc-memory.ini";
            const string repoPath = @"d:\OSTIS\sc-machine-iot\bin\repo";
            const string extensionPath = @"d:\OSTIS\sc-machine-iot\bin\extensions";
            const string netExtensionPath = @"d:\OSTIS\sc-machine-iot\bin\netextensions";

            ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);


            var node = ScMemory.BaseContext.CreateNode(ElementType.Node_a);
            node.MainIdentifiers[NLanguages.Lang_ru] = "идентификатор";
            node.MainIdentifiers[NLanguages.Lang_en] = "idtffff";
             ScNode main_idtf = ScMemory.BaseContext.FindNode("nrel_main_idtf");
           Console.WriteLine(node.GetElementByNrelClass(main_idtf, NLanguages.Lang_ru,ElementType.Link_a).ScAddress);
          
            
            
            
            Console.ReadKey();
            Console.WriteLine(ScMemory.ShutDown(true));
        }
    }
}
