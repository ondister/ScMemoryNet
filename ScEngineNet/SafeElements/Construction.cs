using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScEngineNet.SafeElements
{
   public  class Construction
    {
        private List<ScElement> elements;

        public List<ScElement> Elements
        {
            get { return elements; }
        }

        internal Construction()
        {
            this.elements = new List<ScElement>();

        }

    }
}
