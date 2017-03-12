using System.Collections;
using System.Collections.Generic;
using ScEngineNet.Native;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Итератор для поиска конструкций по шаблону.
    ///     Создается в методе CreateIterator класса <see cref="ScMemoryContext" />
    /// </summary>
    public class ScIterator : IEnumerable<ScConstruction>
    {
        private readonly ScIterator3Type iterator3Type;
        private readonly ScIterator5Type iterator5Type;
        private readonly ScIteratorParam p1;
        private readonly ScIteratorParam p2;
        private readonly ScIteratorParam p3;
        private readonly ScIteratorParam p4;
        private readonly ScIteratorParam p5;
        private readonly ScMemoryContext scContext;

        private ScIterator(ScMemoryContext scContext)
        {
            this.scContext = scContext;
        }

        /// <summary>
        ///     Возвращает перечислитель, выполняющий перебор элементов в коллекции.
        /// </summary>
        /// <returns>
        ///     Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1" />, который может использоваться для перебора
        ///     элементов коллекции.
        /// </returns>
        public IEnumerator<ScConstruction> GetEnumerator()
        {
            if (iterator3Type != ScIterator3Type.sc_iterator3_unknown)
            {
                return new ScEnumerator(scContext, iterator3Type, p1, p2, p3);
            }
            return new ScEnumerator(scContext, iterator5Type, p1, p2, p3, p4, p5);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        private IEnumerator GetEnumerator1()
        {
            return GetEnumerator();
        }

        #region Constructors

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScTypes t2)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_f_a_a;
            iterator5Type = ScIterator5Type.sc_iterator5_unknown;
            p1 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p2 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
        }

        internal ScIterator(ScMemoryContext scContext, ScTypes t1, ScTypes t2, ScElement e1)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_a_a_f;
            iterator5Type = ScIterator5Type.sc_iterator5_unknown;

            p1 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p2 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p3 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScElement e2)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_f_a_f;
            iterator5Type = ScIterator5Type.sc_iterator5_unknown;

            p1 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p2 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam {IsType = false, Address = e2.ScAddress.WScAddress};
        }

        internal ScIterator(ScMemoryContext scContext, ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScTypes t4)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            iterator5Type = ScIterator5Type.sc_iterator5_a_a_f_a_a;

            p1 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p2 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p3 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p4 = new ScIteratorParam {IsType = true, Type = t3.ElementType};
            p5 = new ScIteratorParam {IsType = true, Type = t4.ElementType};
        }

        internal ScIterator(ScMemoryContext scContext, ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScElement e2)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            iterator5Type = ScIterator5Type.sc_iterator5_a_a_f_a_f;

            p1 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p2 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p3 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p4 = new ScIteratorParam {IsType = true, Type = t3.ElementType};
            p5 = new ScIteratorParam {IsType = false, Address = e2.ScAddress.WScAddress};
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScTypes t4)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            iterator5Type = ScIterator5Type.sc_iterator5_f_a_a_a_a;

            p1 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p2 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p4 = new ScIteratorParam {IsType = true, Type = t3.ElementType};
            p5 = new ScIteratorParam {IsType = true, Type = t4.ElementType};
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScElement e2)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            iterator5Type = ScIterator5Type.sc_iterator5_f_a_a_a_f;

            p1 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p2 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p4 = new ScIteratorParam {IsType = true, Type = t3.ElementType};
            p5 = new ScIteratorParam {IsType = false, Address = e2.ScAddress.WScAddress};
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScTypes t3)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            iterator5Type = ScIterator5Type.sc_iterator5_f_a_f_a_a;

            p1 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p2 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam {IsType = false, Address = e2.ScAddress.WScAddress};
            p4 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p5 = new ScIteratorParam {IsType = true, Type = t3.ElementType};
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScElement e3)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            iterator5Type = ScIterator5Type.sc_iterator5_f_a_f_a_f;

            p1 = new ScIteratorParam {IsType = false, Address = e1.ScAddress.WScAddress};
            p2 = new ScIteratorParam {IsType = true, Type = t1.ElementType};
            p3 = new ScIteratorParam {IsType = false, Address = e2.ScAddress.WScAddress};
            p4 = new ScIteratorParam {IsType = true, Type = t2.ElementType};
            p5 = new ScIteratorParam {IsType = false, Address = e3.ScAddress.WScAddress};
        }

        #endregion
    }
}