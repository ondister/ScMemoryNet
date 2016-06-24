using System;

namespace ScEngineNet.SafeElements
{
    public class ScArc : ScElement
    {
        ScElement beginElement;

        public ScElement BeginElement
        {
            get
            {
                this.beginElement = ScMemorySafeMethods.GetArcBeginElement(base.scContext, this);
                return beginElement;
            }
        }

        private ScElement endElement;

        public ScElement EndElement
        {
            get
            {
                this.endElement = ScMemorySafeMethods.GetArcEndElement(base.scContext, this);
                return this.endElement;
            }
        }

        internal ScArc(ScAddress arcAddress, IntPtr scExtContent)
            : base(arcAddress, scExtContent)
        { }
    }
}
