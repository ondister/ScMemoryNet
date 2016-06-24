using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ScEngineNet;
using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;

namespace ExNetPullEnty
{
    public class PullentiEngine
    {
        private ScMemoryContext context;
        ScEvent initNodeEvent;
        public PullentiEngine(ScMemoryContext context)
        {
            this.context = context;
            var initNode = context.CreateNode(ElementType.ClassNode_a, "pullenti_init_node");
            initNodeEvent = context.CreateEvent(initNode, ScEventType.SC_EVENT_ADD_OUTPUT_ARC);
            initNodeEvent.ElementEvent += initNodeEvent_ElementEvent;


            //
            Stopwatch st = new Stopwatch();
            st.Start();
            for (int i = 0; i < 10000; i++)
            {
                context.CreateNode(ElementType.ClassNode_a, "idf_" + i.ToString());
            }

            st.Stop();

            Console.WriteLine(st.ElapsedMilliseconds);



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
