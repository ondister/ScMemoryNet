using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ScEngineNet.SafeElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
using System.Diagnostics;
using System.Xml;
using ScEngineNet;
using ScEngineNet.NativeElements;

namespace Demo
{
    public class Demo
    {
        ScMemoryContext context;
        private Stopwatch clock;
        public void Start()
        {

            //var scParams = new WScMemoryParams
            //{
            //    Clear = true,
            //    ConfigFile = @"sc-memory.ini",
            //    RepoPath = @"repo",
            //    ExtensionsPath = @"extensions"
            //};

            ////sc_memory_initialize 
            //var scMemoryContext = NativeMethods.sc_memory_initialize(scParams);

            ////создаем конструкцию
            //WScAddress addrNode = NativeMethods.sc_memory_node_new(scMemoryContext, ElementType.ConstantNode_c);
            //WScAddress addrLink = NativeMethods.sc_memory_link_new(scMemoryContext);
            //WScAddress addrCommArc = NativeMethods.sc_memory_arc_new(scMemoryContext, ElementType.PositiveConstantPermanentAccessArc_c, addrNode, addrLink);

            //IntPtr iter = NativeMethods.sc_iterator3_f_a_f_new(scMemoryContext, addrNode, ElementType.PositiveConstantPermanentAccessArc_c, addrLink);
            //Console.WriteLine("iter 1 {0}", iter);
            //NativeMethods.sc_iterator3_next(iter);
            //NativeMethods.sc_iterator3_value(iter, 0);
            //NativeMethods.sc_iterator3_free(iter);
            //Console.WriteLine("iter 1 after delete {0}", iter);



            //iter = NativeMethods.sc_iterator3_f_a_f_new(scMemoryContext, addrNode, ElementType.PositiveConstantPermanentAccessArc_c, addrLink);
            //Console.WriteLine("iter 2 {0}", iter);
            //NativeMethods.sc_iterator3_next(iter);
            //NativeMethods.sc_iterator3_value(iter, 0);
            //NativeMethods.sc_iterator3_free(iter);
            //Console.WriteLine("iter 2 after delete {0}", iter);

            //iter = NativeMethods.sc_iterator3_f_a_f_new(scMemoryContext, addrNode, ElementType.PositiveConstantPermanentAccessArc_c, addrLink);
            //Console.WriteLine("iter 3 {0}", iter);
            //NativeMethods.sc_iterator3_next(iter);
            //NativeMethods.sc_iterator3_value(iter, 0);
            //NativeMethods.sc_iterator3_free(iter);
            //Console.WriteLine("iter 3 after delete {0}", iter);


            //NativeMethods.sc_memory_context_free(scMemoryContext);



            const string configFile = @"d:\OSTIS\sc-machine-master\bin\sc-memory.ini";
            const string repoPath = @"d:\OSTIS\sc-machine-master\bin\repo";
            const string extensionPath = @"d:\OSTIS\sc-machine-master\bin\extensions";
            const string netExtensionPath = @"d:\OSTIS\sc-machine-master\bin\netextensions";
            ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);


            //ScNode nd = context.CreateNode(ElementType.ConstantNode_c);
            //ScNode nd1 = context.CreateNode(ElementType.NonRoleNode_a);
            //nd.AddOutputArc(nd1, ElementType.PositiveConstantPermanentAccessArc_c);
            //ScIterator iter = context.CreateIterator(nd, ElementType.PositiveConstantPermanentAccessArc_c, nd1);



            //foreach (var c in iter)
            //{
            //    Console.WriteLine(c.Elements.Count);
            //    Console.WriteLine(c.Elements[0].ScAddress);
            //}

            //Console.WriteLine(iter.Count());
            //Console.WriteLine(iter.ElementAt(0).Elements[1]);
            //Console.WriteLine(iter.ElementAt(0).Elements[2]);




            //ScLink link = context.CreateLink("Весело, не правда ли? Что бы достать объект из репозитория, нужно сначала вытащить объект из репозитория, для чего нужно вытащить объект из репозитория, для чего… Правда в том, что ложки нет никто не может гарантировать существование объекта в репозитории. Единственный способ гарантировать существование любого объекта это создать его и зажать его ссылку. Все остальное – плохой дизайн. Data access layer должен достать объект из репозитория и передать клиенту. Он не знает как обработать отсутствие объекта и вообще не в курсе плохо ли это. За это отвечает уровень логики. Невозможность гарантировать существования объекта вне поля видимости, приводит ко второй идее приведенной в статье по ссылке вверху. Использование nullable контейнера для ссылочных типов. Я перефразирую: nullable reference. Масло масляное. Было странно узнать про популярность этого паттерна. Ссылочный тип может ссылаться на null. Зачем засовывать один ссылочный тип в другой и добавлять в контейнер свойство HasValue, для меня решительно не понятно. Для проверки на HasValue? Что мешает обратится к содержимому объекту без этой проверки? Можно точно так-же безалаберно не проверить на null через неравенство. Более того, этот паттерн не только бесполезен, но и вреден. Взгляните на следующий код:");
            //ScNode tokenizedNode = context.FindNode("class_link_for_tokenize");
            //if ((object)tokenizedNode != null)
            //{
            //    clock = new Stopwatch();
            //    tokenizedNode.AddOutputArc(link, ElementType.PositiveConstantPermanentAccessArc_c);
            //    clock.Start();
            //    Console.WriteLine("Ссылка добавлена в очередь.");
            //    link.InputArcAdded += link_InputArcAdded;
            //}




            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            ScLink link = context.CreateLink("linkkkk");

            Console.WriteLine(((ScString)link.LinkContent).Value);

            link.Dispose();
            Console.WriteLine(((ScString)link.LinkContent).Value);

            context.Dispose();

            Console.WriteLine("Memory shutDown whith {0}", ScMemory.ShutDown(false));

            
          
            Console.ReadKey();
        }





    }
}
