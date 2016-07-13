using System;
using System.Collections;
using System.Collections.Generic;

using ScEngineNet.NativeElements;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Итератор для поиска конструкций по шаблону.
    /// Создается в методе CreateIterator класса <see cref="ScMemoryContext" />
    /// </summary>
    public sealed class ScIterator : IEnumerable<Construction>, IEnumerator<Construction>, IDisposable
    {
        private readonly IntPtr scContext;
        private readonly ScIterator3Type iterator3type;
        private ScIterator5Type iterator5type;
        private Construction construction;
        IntPtr iterator = IntPtr.Zero;
        private List<Construction> constructions;

        /// <summary>
        /// Взвращает найденные конструкции. Конструкции будут доступны после выполнения метода GetAllConctruction или после итерации всех конструкций.
        /// </summary>
        /// <value>
        /// Коллекция конструкций
        /// </value>
        public List<Construction> Constructions
        {
            get { return constructions; }
        }


        #region Конструкторы

        private ScIterator(IntPtr scContext)
        {
            this.scContext = scContext;
            this.construction = new Construction();
            this.constructions = new List<Construction>();
        }

        internal ScIterator(IntPtr scContext,ScElement e1, ElementType t1, ElementType t2)
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

        internal ScIterator(IntPtr scContext, ElementType t1, ElementType t2, ScElement e1)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_a_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScIterator(IntPtr scContext, ScElement e1, ElementType t1, ScElement e2)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScIterator(IntPtr scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ElementType t4)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_a;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = true, Type = t4 };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        internal ScIterator(IntPtr scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ScElement e2)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_a_a_f_a_f;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        internal ScIterator(IntPtr scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ElementType t4)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_a;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = true, Type = t4 };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        internal ScIterator(IntPtr scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ScElement e2)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_a_a_f;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t3 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        internal ScIterator(IntPtr scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ElementType t3)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_a;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = true, Type = t3 };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        internal ScIterator(IntPtr scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ScElement e3)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = ScIterator5Type.sc_iterator5_f_a_f_a_f;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };
            ScIteratorParam p4 = new ScIteratorParam() { IsType = true, Type = t2 };
            ScIteratorParam p5 = new ScIteratorParam() { IsType = false, Address = e3.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        #endregion


        /// <summary>
        /// Получает сразу все конструкции, удовлетворяющие шаблону
        /// Необходимо пользоваться с осторожностью, так как вывод сразу всех конструкций может занять длительное время.
        /// </summary>
        /// <returns>Список конструкций</returns>
        public List<Construction> GetAllConstructions()
        {
            this.constructions = new List<Construction>();
            foreach (var constr in this)
            {
                this.constructions.Add(constr);
            }
            return this.constructions;
        }



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

        /// <summary>
        /// Перемещает перечислитель к следующему элементу коллекции.
        /// </summary>
        /// <returns>
        /// Значение true, если перечислитель был успешно перемещен к следующему элементу; значение false, если перечислитель достиг конца коллекции.
        /// </returns>
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
                    this.construction.Elements.Add(ScMemorySafeMethods.GetElement(new ScAddress(NativeMethods.sc_iterator3_value(this.iterator, element)).WScAddress, this.scContext));
                }
               
            }
            else
            {
                if (NativeMethods.sc_iterator5_next(iterator) == false)
                {
                    this.Reset();
                    return false;
                }
                for (uint element = 0; element < 5; element++)
                {
                    this.construction.Elements.Add(ScMemorySafeMethods.GetElement(new ScAddress(NativeMethods.sc_iterator5_value(this.iterator, element)).WScAddress, this.scContext));
                }
            }
            this.constructions.Add(this.construction);
            return true;
        }

        /// <summary>
        /// Устанавливает перечислитель в его начальное положение, перед первым элементом коллекции.
        /// </summary>
        public void Reset()
        {
            construction = new Construction();
        }

        #endregion

        #region IDisposable

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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

        /// <summary>
        /// Finalizes an instance of the <see cref="ScIterator"/> class.
        /// </summary>
        ~ScIterator()
        {
            Dispose(false);
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
