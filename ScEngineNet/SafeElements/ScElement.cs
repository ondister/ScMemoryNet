using ScEngineNet.NativeElements;
using System;

namespace ScEngineNet.SafeElements
{
    public class ScElement : IEquatable<ScElement>
    {
        internal IntPtr scContext;
        private ScAddress scAddress;
      

        public ElementType ElementType
        {
            get
            {
                return ScMemorySafeMethods.GetElementType(this.scContext, this);
            }
        }
        public ScAddress ScAddress
        {
            get { return scAddress; }
        }

        public bool DeleteFromMemory()
        {
            bool isDeleted = false;
            isDeleted = ScMemorySafeMethods.DeleteElement(this.scContext, this);
             this.scAddress = ScAddress.Invalid;
            return isDeleted;
        }
        internal ScElement(ScAddress scAddress,IntPtr scContext)
        {
            this.scContext = scContext;
            this.scAddress = scAddress;
        }

        public bool ChangeSubType(ElementType subType)
        {
            return NativeMethods.sc_memory_change_element_subtype(this.scContext, this.ScAddress.WScAddress, subType).ToBool();
        }


       

        #region Реализация сравнения
        /// <summary>
        /// Определяет равен ли заданный объект <see cref="ScElement"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="ScElement"/></param>
        public bool Equals(ScElement obj)
        {
            if (obj == null)
                return false;

            return obj.ScAddress == this.scAddress;
        }

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="T:System.Object"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object"/></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ScElement scElement = obj as ScElement;
            if (scElement as ScElement == null)
                return false;
            return scElement.ScAddress == this.scAddress;
        }

        /// <summary>
        /// Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.scAddress.Offset.ToString() + this.scAddress.Segment.ToString());
        }

        /// <summary>
        /// Оператор сравнения элементов
        /// </summary>
        /// <param name="scElement">Первый элемент</param>
        /// <param name="scElement">Второй элемент</param>
        /// <returns>Возвращает True, если элементы равны</returns>
        public static bool operator ==(ScElement scElement1, ScElement scElement2)
        {
            bool isEqual = false;
            if (((object)scElement1 != null) && ((object)scElement2 != null))
            {
                isEqual = scElement1.Equals(scElement2);
            }

            return isEqual;
        }

        /// <summary>
        /// Оператор сравнения элементов
        /// </summary>
        /// <param name="scElement1">Первый элемент</param>
        /// <param name="scElement2">Второй элемент</param>
        /// <returns>Возвращает True, если  элементы равны</returns>
        public static bool operator !=(ScElement scElement1, ScElement scElement2)
        {
            return !(scElement1 == scElement2);
        }

        #endregion


    }
}
