using System.Collections.Generic;

namespace ScEngineNet.SafeElements
{
   public  class Construction
    {
        private readonly List<ScElement> elements;

        public List<ScElement> Elements
        {
            get { return elements; }
        }

        internal Construction()
        {
            elements = new List<ScElement>();
        }
    }
}
