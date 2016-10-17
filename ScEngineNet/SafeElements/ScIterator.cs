﻿using ScEngineNet.NativeElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScEngineNet.SafeElements
{
   public class ScIterator:IEnumerable<ScConstruction>
    {
     
       private readonly ScMemoryContext scContext;
        private readonly ScIterator3Type iterator3type;
        private readonly ScIterator5Type iterator5type;

        private ScIteratorParam p1;
        private ScIteratorParam p2;
        private ScIteratorParam p3;
        private ScIteratorParam p4;
        private ScIteratorParam p5;

        private ScIterator(ScMemoryContext scContext)
        {
            this.scContext = scContext;
        }

        #region Constructors
        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ElementType t2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_a;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;
            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = true, Type = t2 };

        }

        internal ScIterator(ScMemoryContext scContext, ElementType t1, ElementType t2, ScElement e1)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_a_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };

         
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

          
        }

        internal ScIterator(ScMemoryContext scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ElementType t4)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_a;

            p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            p2 = new ScIteratorParam() { IsType = true, Type = t2 };
            p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            p5 = new ScIteratorParam() { IsType = true, Type = t4 };

           
        }

        internal ScIterator(ScMemoryContext scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_f;

            p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            p2 = new ScIteratorParam() { IsType = true, Type = t2 };
            p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

           
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ElementType t4)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_a;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = true, Type = t2 };
            p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            p5 = new ScIteratorParam() { IsType = true, Type = t4 };

           
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_f;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = true, Type = t2 };
            p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

           
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ElementType t3)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_a;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t2 };
            p5 = new ScIteratorParam() { IsType = true, Type = t3 };

            
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ScElement e3)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_f;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t2 };
            p5 = new ScIteratorParam() { IsType = false, Address = e3.ScAddress.WScAddress };

           
        }

        #endregion


        public IEnumerator<ScConstruction> GetEnumerator()
        {

            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
                return new ScEnumerator(this.scContext, this.iterator3type, this.p1, this.p2, this.p3);
            }
            else 
            {
                return new ScEnumerator(this.scContext, this.iterator5type, this.p1, this.p2, this.p3,this.p4,this.p5);
            }
        }


        private IEnumerator GetEnumerator1()
        {
            return (IEnumerator)this.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
          return  this.GetEnumerator1();
        }
    }
}
