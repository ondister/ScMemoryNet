using System;

namespace ScEngineNet.SafeElements
{
    public class ScNode:ScElement
    {
        public Identifier Identifier
        {
            get { return ScMemorySafeMethods.GetSystemIdentifier(base.scContext, this); }
            set { ScMemorySafeMethods.SetSystemIdentifier(base.scContext, this, value); }
        }

        internal ScNode(ScAddress nodeAddress, IntPtr scContext)
            : base(nodeAddress, scContext)
        { }
    }
}
