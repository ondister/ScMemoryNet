using System;

namespace ScEngineNet.SafeElements
{
    public class ScLink : ScElement
    {
        public ScLinkContent LinkContent
        {
            get
            {
                return ScMemorySafeMethods.GetLinkContent(base.scContext, this);
            }
            set
            {
                ScMemorySafeMethods.SetLinkContent(base.scContext, value, this);
            }
        }

        internal ScLink(ScAddress linkAddress, IntPtr scContext)
            : base(linkAddress,scContext)
        { }
    }
}
