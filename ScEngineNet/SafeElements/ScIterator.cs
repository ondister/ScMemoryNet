using System;
using System.Collections;
using System.Collections.Generic;

using ScEngineNet.NativeElements;
using System.Runtime.InteropServices;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Итератор для поиска конструкций по шаблону.
    /// Создается в методе CreateIterator класса <see cref="ScMemoryContext" />
    /// </summary>
    public sealed class ScIterator :SafeHandle, IEnumerable<Construction>, IEnumerator<Construction>
    {
        private readonly ScMemoryContext scContext;
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

        private ScIterator(ScMemoryContext scContext)
            :base(IntPtr.Zero,true)
        {
            this.scContext = scContext;
            this.construction = new Construction();
            this.constructions = new List<Construction>();
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ElementType t2)
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

        internal ScIterator(ScMemoryContext scContext, ElementType t1, ElementType t2, ScElement e1)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_a_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ScElement e2)
            : this( scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_f_a_f;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            ScIteratorParam p1 = new ScIteratorParam() { IsType = false, Address = e1.ScAddress.WScAddress };
            ScIteratorParam p2 = new ScIteratorParam() { IsType = true, Type = t1 };
            ScIteratorParam p3 = new ScIteratorParam() { IsType = false, Address = e2.ScAddress.WScAddress };

            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScIterator(ScMemoryContext scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ElementType t4)
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

        internal ScIterator(ScMemoryContext scContext, ElementType t1, ElementType t2, ScElement e1, ElementType t3, ScElement e2)
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

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ElementType t4)
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

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ElementType t2, ElementType t3, ScElement e2)
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

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ElementType t3)
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

        internal ScIterator(ScMemoryContext scContext, ScElement e1, ElementType t1, ScElement e2, ElementType t2, ScElement e3)
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


        #region SafeHandle        
        /// <summary>
        /// При переопределении в производном классе получает значение, показывающее, допустимо ли значение дескриптора.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override bool IsInvalid
        {
            get { return this.iterator==IntPtr.Zero; }
        }
        /// <summary>
        /// При переопределении в производном классе выполняет код, необходимый для освобождения дескриптора.
        /// </summary>
        /// <returns>
        /// Значение true, если дескриптор освобождается успешно, в противном случае, в случае катастрофической ошибки — значение  false.В таком случае создается управляющий помощник по отладке releaseHandleFailed MDA.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            this.Delete();
            return !IsInvalid;
        }
        #endregion
    }
}
