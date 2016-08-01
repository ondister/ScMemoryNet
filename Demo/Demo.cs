using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScEngineNet.SafeElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
using System.Diagnostics;
using System.Xml;
using ScEngineNet;

namespace Demo
{
   public  class Demo
    {

       public void Start()
       {
           const string configFile = @"d:\OSTIS\sc-machine-iot\bin\sc-memory.ini";
           const string repoPath = @"d:\OSTIS\sc-machine-iot\bin\repo";
           const string extensionPath = @"d:\OSTIS\sc-machine-iot\bin\extensions";
           const string netExtensionPath = @"d:\OSTIS\sc-machine-iot\bin\netextensions";
           ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);
           using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
           { 
               //создаем узел с идентификаторами
             
                   var node = context.CreateNode(ElementType.ConstantNode_c, "sys_identificator");
                   node.MainIdentifiers[NLanguages.Lang_en] = "engl_idtf";
                   node.MainIdentifiers[NLanguages.Lang_ru] = "ru_idtf";

               //итерируем их
                   ScNode main_idtf = context.FindNode("nrel_main_idtf");
                   if (main_idtf.IsValid)
                   {
                       var iterator = context.CreateIterator(node, ElementType.ConstantCommonArc_c, ElementType.Link_a, ElementType.PositiveConstantPermanentAccessArc_c, main_idtf);
                       foreach (var construction in iterator)
                       {
                           var linkContent = ((ScLink)construction.Elements[2]).LinkContent;
                           Console.WriteLine(((ScString)linkContent).Value);
                       }
                   }

               //подпитываем элемент на событие добавления исходящей дуги
                 var nodeEvent=  node.CreateEvent(ScEventType.SC_EVENT_ADD_OUTPUT_ARC);
                 nodeEvent.ElementEvent += nodeEvent_ElementEvent;
               //добаляем исходящую дугу
                 var link = context.CreateLink("linka");
                 node.AddOutputArc(link, ElementType.PositiveConstantPermanentAccessArc_c);
             



           }
           
           Console.ReadKey();
           Console.WriteLine(ScMemory.ShutDown(false));
           Console.ReadKey();
       }

       void nodeEvent_ElementEvent(object sender, ScEventArgs e)
       {
           Console.WriteLine("arc added");
       }


    }
}
