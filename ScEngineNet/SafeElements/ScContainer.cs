using System;
using System.Collections;
using System.Collections.Generic;

using ScEngineNet.NativeElements;

namespace ScEngineNet.SafeElements
{
    public sealed class ScContainer : IEnumerable<Construction>, IEnumerator<Construction>, IDisposable
    {
        private readonly IntPtr scContext;
        private readonly ScIterator3Type iterator3type;
        private ScIterator5Type iterator5type;
        private Construction construction;
        IntPtr iterator = IntPtr.Zero;

        #region Конструкторы

        private ScContainer(IntPtr scContext)
        {
            this.scContext = scContext;
            this.construction = new Construction();
        }

        internal ScContainer(IntPtr scContext,ScElement e1, ElementType t1, ElementType t2)
            : this( scContext)
        {
            this.construction = new Construction();
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_a;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;
            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = true, Type = t2 };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScContainer(IntPtr scContext, ElementType t1, ElementType t2, ScElement e1)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_a_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScContainer(IntPtr scContext, ScElement e1, ElementType t1, ScElement e2)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScContainer(IntPtr scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ElementType t4)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_a;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = true, Type = t4 };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p3, p4);
        }

        internal ScContainer(IntPtr scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_f;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p3, p4);
        }

        internal ScContainer(IntPtr scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ElementType t4)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_a;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = true, Type = t4 };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p3, p4);
        }

        internal ScContainer(IntPtr scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ScElement e2)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_f;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p3, p4);
        }

        internal ScContainer(IntPtr scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ElementType t3)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_a;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = true, Type = t3 };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p3, p4);
        }

        internal ScContainer(IntPtr scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ScElement e3)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_f;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = false, Address = e3.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p3, p4);
        }

        #endregion

        #region перечислители

        IEnumerator<Construction> IEnumerable<Construction>.GetEnumerator()
        {
            return this;
        }

        Construction IEnumerator<Construction>.Current
        {
            get
            {
                return this.construction;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        object IEnumerator.Current
        {
            get
            {
                return this.construction;
            }
        }

        public bool MoveNext()
        {
            this.Reset();
            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
                if (NativeMethods.sc_iterator3_next(iterator) == false)
                {
                    this.Reset();
                    return false;
                }
                for (uint element = 0; element < 3; element++)
                {
                    this.construction.Elements.Add(new ScElement(new ScAddress(NativeMethods.sc_iterator3_value(this.iterator, element)), this.scContext));
                }
            }
            else
            {
                if (NativeMethods.sc_iterator3_next(iterator) == false)
                {
                    this.Reset();
                    return false;
                }
                for (uint element = 0; element < 5; element++)
                {
                    this.construction.Elements.Add(new ScElement(new ScAddress(NativeMethods.sc_iterator5_value(this.iterator, element)),  this.scContext));
                }
            }

            return true;
        }

        public void Reset()
        {
            construction = new Construction();
        }

        #endregion

        #region IDisposable

        private bool disposed = false;

        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    construction = null;
                }
                //unmanaged
                this.Delete();
                this.iterator = IntPtr.Zero;
            }
        }

        private bool Delete()
        {
            bool isDeleted = false;
            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
                NativeMethods.sc_iterator3_free(this.iterator);
            }
            else
            {
                NativeMethods.sc_iterator5_free(this.iterator);
            }
            return isDeleted;
        }

        ~ScContainer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
