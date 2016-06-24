using ScEngineNet.NativeElements;
using System;

namespace ScEngineNet.SafeElements
{
    public class ScEventArgs : EventArgs
    {


        private ScEventType eventType;
        private ScElement element;
        private ScArc arc;
       
        public ScEventType EventType
        {
            get { return eventType; }
        }

             public ScElement Element
        {
            get { return this.element; }
        }
     

        public ScArc Arc
        {
            get { return this.arc; }
        }

        public ScEventArgs(ScEventType eventType, ScElement element, ScArc arc)
        {
            this.eventType = eventType;
            this.element = element;
            this.arc = arc;
        }

    }
}
