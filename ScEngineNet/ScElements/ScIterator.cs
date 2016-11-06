using ScEngineNet.Native;
using System.Collections;
using System.Collections.Generic;

namespace ScEngineNet.ScElements
{
    /// <summary>
    /// Итератор для поиска конструкций по шаблону.
    /// Создается в методе CreateIterator класса <see cref="ScMemoryContext" />
    /// </summary>
    public class ScIterator : IEnumerable<ScConstruction>
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
        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScTypes t2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_a;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;
            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p3 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };

        }

        internal ScIterator(ScMemoryContext scContext, ScTypes t1, ScTypes t2, ScElement e1)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_a_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            p1 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p2 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };
            p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };


        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };


        }

        internal ScIterator(ScMemoryContext scContext, ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScTypes t4)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_a;

            p1 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p2 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };
            p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t3.ElementType };
            p5 = new ScIteratorParam() { IsType = true, Type = t4.ElementType };


        }

        internal ScIterator(ScMemoryContext scContext, ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_f;

            p1 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p2 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };
            p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t3.ElementType };
            p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };


        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScTypes t4)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_a;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p3 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };
            p4 = new ScIteratorParam() { IsType = true, Type = t3.ElementType };
            p5 = new ScIteratorParam() { IsType = true, Type = t4.ElementType };


        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_f;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p3 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };
            p4 = new ScIteratorParam() { IsType = true, Type = t3.ElementType };
            p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };


        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScTypes t3)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_a;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1.ElementType };
            p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t2.ElementType };
            p5 = new ScIteratorParam() { IsType = true, Type = t3.ElementType };


        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScElement e3)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_f;

            p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            p2 = new ScIteratorParam() { IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            p4 = new ScIteratorParam() { IsType = true, Type = t2.ElementType};
            p5 = new ScIteratorParam() { IsType = false, Address = e3.ScAddress.WScAddress };


        }

        #endregion

        /// <summary>
        /// Возвращает перечислитель, выполняющий перебор элементов в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1" />, который может использоваться для перебора элементов коллекции.
        /// </returns>
        public IEnumerator<ScConstruction> GetEnumerator()
        {

            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
                return new ScEnumerator(this.scContext, this.iterator3type, this.p1, this.p2, this.p3);
            }
            else
            {
                return new ScEnumerator(this.scContext, this.iterator5type, this.p1, this.p2, this.p3, this.p4, this.p5);
            }
        }


        private IEnumerator GetEnumerator1()
        {
            return (IEnumerator)this.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator1();
        }
    }
}
