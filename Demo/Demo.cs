using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ScEngineNet.ScElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
using System.Diagnostics;
using System.Xml;
using ScEngineNet;
using ScEngineNet.Native;

namespace Demo
{
    public class Demo
    {
        
        public void Start()
        {

           


        //    const string configFile = @"d:\OSTIS\sc-machine-master\bin\sc-memory.ini";
        //    const string repoPath = @"d:\OSTIS\sc-machine-master\bin\repo";
        //    const string extensionPath = @"d:\OSTIS\sc-machine-master\bin\extensions";
        //    const string netExtensionPath = @"d:\OSTIS\sc-machine-master\bin\netextensions";
        //    ScMemory.Initialize(false, configFile, repoPath, extensionPath, netExtensionPath);
        //    ScMemoryContext context = new ScMemoryContext(ScAccessLevels.MinLevel);
        //    var classLinkForTokenize = context.FindNode("class_link_for_tokenize");

        //    var link = context.CreateLink("«У секретаря закончился картридж, заменишь?» — «Ок». «По дороге посмотри там, бухгалтера 1С не пускает» — «Ок». «Алло, и ещё, пока не забыл — у верстальщика хард скрипит, видимо, помирает». Примерно так координируется работа ИТ-отдела в небольших компаниях, нередко то же самое происходит и в средних. Задачи оказываются забытыми, сотрудники простаивают в ожидании, на момент инвентаризации непременно теряются какие-то комплектующие или бумаги на них, экономисты урезают бюджет, потому что обосновать будущие траты почти нереально. С лицензиями ПО — вообще беда. Ну и ладно, давайте всем новый MS Office купим. Что нам, ITIL с ITSM внедрять, что ли? Да, внедрять. Да, ITIL. Ну точнее, не совсем.");
        //    classLinkForTokenize.AddOutputArc(link, ScTypes.ArcAccessConstantPositivePermanent);


        //    var link1 = context.CreateLink("доктор опять в шкафу");
        //    classLinkForTokenize.AddOutputArc(link1, ScTypes.ArcAccessConstantPositivePermanent);
        //    Console.ReadKey();

        //   //запрос
        //    var classQuerryString = context.FindNode("class_querry_string");
        //    var querryInstNode = context.CreateNode(ScTypes.NodeConstant);
        //    querryInstNode.SystemIdentifier=context.CreateUniqueIdentifier(ScNode.InstancePreffix, querryInstNode);
        //    querryInstNode.MainIdentifiers[ScDataTypes.Instance.LanguageRu] = "бухгалтер залез в шкаф";
        //    // подписываемся на ответ
        //    querryInstNode.OutputArcAdded += classQuerryString_OutputArcAdded;

        //    //создаем запрос
        //    classQuerryString.AddOutputArc(querryInstNode, ScTypes.ArcAccessConstantPositivePermanent);
            
        //    Console.ReadKey();

           

            


        //    classQuerryString.OutputArcAdded -= classQuerryString_OutputArcAdded;

        //    classLinkForTokenize.Dispose();
        //    classQuerryString.Dispose();
        //    context.Dispose();

        //    Console.WriteLine("Memory shutDown whith {0}", ScMemory.ShutDown(true));

            
          
        //    Console.ReadKey();
        //}

        //void classQuerryString_OutputArcAdded(object sender, ScEventArgs e)
        //{

        //    using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
        //    { 
        //        var nrelQuerryResponse = context.FindNode("class_querry_response");

        //        var constrIterator = context.CreateIterator(nrelQuerryResponse, ScTypes.ArcAccessConstantPositivePermanent, e.Arc);
        //        Console.WriteLine(constrIterator.Count());
        //        if (constrIterator.Count() != 0)
        //        {
        //            Console.WriteLine("Внимание!!! Ответ готов!!!");
        //        }
           
        //    }
        }





    }
}
