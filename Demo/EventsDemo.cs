using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScEngineNet.SafeElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
using System.Diagnostics;
using System.Xml;
using ScEngineNet;
using System;
using System.Threading;
namespace Demo
{
    public class EventsDemo
    {
        public void Start()
        {
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                //создаем узел 
                var node = context.CreateNode(ElementType.ConstantNode_c, "sys_identificator1");
                //создаем линк
                var link = context.CreateLink("linka");
                //подписываем на событие изменения контента ссылку
             //   link.ContentChanged += link_ContentChanged;
              //  link.LinkContent = "new content";

                ////подписываем элемент на событие добавления исходящей дуги
                //node.OutputArcAdded += node_OutputArcAdded;
                ////подписываем элемент на событие удаления исходящей дуги
                //node.OutputArcRemoved += node_OutputArcRemoved;

                ////подписываем элемент на событие добавления входящей дуги
                //node.InputArcAdded += node_InputArcAdded;
                ////подписываем элемент на событие удаления входящей дуги
                //node.InputArcRemoved += node_InputArcRemoved;

                //добаляем исходящую дугу
                var arc = node.AddOutputArc(link, ElementType.PositiveConstantPermanentAccessArc_c);
               arc.ElementRemoved += arc_ElementRemoved;
                Console.WriteLine("arc" + arc.ScAddress);
                //удаляем исходящую дугу
                 arc.DeleteFromMemory();
                 Thread.Sleep(3000);
               //  arc.ElementRemoved -= arc_ElementRemoved;
                

                 var nnn = context.CreateNode(ElementType.ClassNode_a);
                 Console.WriteLine("node" + nnn.ScAddress);
                 nnn.DeleteFromMemory();

                //добавляем входящую дугу
                var inputArc = node.AddInputArc( ElementType.PositiveConstantPermanentAccessArc_c,link);
                inputArc.ElementRemoved += inputArc_ElementRemoved;
                Console.WriteLine("inputArc" + inputArc.ScAddress);
                //удаляем входящую дугу
                inputArc.DeleteFromMemory();
                Console.ReadKey();
            }

        }

        void inputArc_ElementRemoved(object sender, ScEventArgs e)
        {
            Console.WriteLine("input arc as element removed");
        }

        void node1_ElementRemoved(object sender, ScEventArgs e)
        {
            Console.WriteLine("node1 removed");
        }

        void node_ElementRemoved(object sender, ScEventArgs e)
        {
            Console.WriteLine("node removed");
        }

        void node_InputArcRemoved(object sender, ScEventArgs e)
        {
            Console.WriteLine("input arc removed");
        }

        void node_InputArcAdded(object sender, ScEventArgs e)
        {
            Console.WriteLine("input arc added");
        }

        void arc_ElementRemoved(object sender, ScEventArgs e)
        {
            Console.WriteLine("output arc as element removed");
        }

        void link_ContentChanged(object sender, ScEventArgs e)
        {
            Console.WriteLine("ContentChanged");
        }

        void node_OutputArcRemoved(object sender, ScEventArgs e)
        {
            Console.WriteLine("output arc removed");
        }

        void node_OutputArcAdded(object sender, ScEventArgs e)
        {
            Console.WriteLine("output arc added");
        }

    }
}
