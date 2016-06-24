using System;
using System.Diagnostics;
using ScEngineNet;
using ScEngineNet.SafeElements;
using ScEngineNet.NativeElements;
using System.Collections.Generic;
using ScMemoryNet;
using System.Runtime.InteropServices;
namespace Demo
{
    class Program
    {

        static void Main(string[] args)
        {


            string configFile = @"d:\OSTIS\sc-machine-iot\bin\sc-memory.ini";
            string repoPath = @"d:\OSTIS\sc-machine-iot\bin\repo";
            string extensionPath = @"d:\OSTIS\sc-machine-iot\bin\extensions";
            string netExtensionPath = @"d:\OSTIS\sc-machine-iot\bin\netextensions";



            ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);

            var pNode = ScMemory.BaseContext.FindNode("pullenti_init_node");

            if (pNode.ScAddress != ScAddress.Invalid)
            {
                var link = ScMemory.BaseContext.CreateLink(new ScLinkContent("Этот текст будет обработан расширением"));
                var link2 = ScMemory.BaseContext.CreateLink(new ScLinkContent(" И этот текст будет обработан расширением"));
              

                ScMemory.BaseContext.CreateArc(pNode, link, ElementType.AccessArc_a);
                ScMemory.BaseContext.CreateArc(pNode, link2, ElementType.AccessArc_a);
              
            }

            Console.ReadKey();


            Console.WriteLine(ScMemory.ShutDown(true));
           
        }

      



    }
}
