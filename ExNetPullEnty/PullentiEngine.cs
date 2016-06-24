using System;
using System.Diagnostics;

using ScEngineNet;
using ScEngineNet.SafeElements;

namespace ExNetPullEnty
{
    public class PullentiEngine
    {
        private ScMemoryContext context;
        private readonly ScEvent initNodeEvent;

        public PullentiEngine(ScMemoryContext context)
        {
            this.context = context;
            var initNode = context.CreateNode(ElementType.ClassNode_a, "pullenti_init_node");
            initNodeEvent = context.CreateEvent(initNode, ScEventType.SC_EVENT_ADD_OUTPUT_ARC);
            initNodeEvent.ElementEvent += initNodeEvent_ElementEvent;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 10000; i++)
            {
                context.CreateNode(ElementType.ClassNode_a, "idf_" + i);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        void initNodeEvent_ElementEvent(object sender, ScEventArgs e)
        {
            ProcessLink((e.Arc.EndElement as ScLink));
        }
        
        private void ProcessLink(ScLink link)
        {
            Console.WriteLine("linkForProcess " + ScLinkContent.ToString(link.LinkContent.Content));
        }

        public void DeleteEvents()
        {
            initNodeEvent.Delete();
        }
    }
}
