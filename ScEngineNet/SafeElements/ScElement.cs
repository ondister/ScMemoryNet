using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Базовый класс для всех sc-элементов памяти: sc-узла, sc-ссылки и sc-дуги. Напрямую создать его нельзя
    /// </summary>
    public class ScElement : IEquatable<ScElement>
    {
        internal ScMemoryContext scContext;
        private ScAddress scAddress;

        /// <summary>
        /// Вовращает тип элемента
        /// </summary>
        /// <value>
        /// Тип элемента
        /// </value>
        public ElementType ElementType
        {
            get
            {
                return ScMemorySafeMethods.GetElementType(this.scContext, this);
            }
        }
        /// <summary>
        /// Returns true if ScElement is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                return this.isValid() ;
            }
        }

        /// <summary>
        /// Возвращает адрес элемента
        /// </summary>
        /// <value>
        /// Sc-адрес
        /// </value>
        public ScAddress ScAddress
        {
            get { return scAddress; }
        }

        /// <summary>
        /// Удаляет элемент из памяти
        /// </summary>
        /// <returns>Возвращает ScResult.SC_RESULT_OK, если элемент удален</returns>
        public ScResult DeleteFromMemory()
        {
            ScResult isDeleted = ScResult.SC_RESULT_ERROR;
            isDeleted = ScMemorySafeMethods.DeleteElement(this.scContext, this);
            this.scAddress = ScAddress.Invalid;
            return isDeleted;
        }

        internal ScElement(ScAddress scAddress, ScMemoryContext scContext)
        {
            this.scContext = scContext;
            this.scAddress = scAddress;
        }

        /// <summary>
        /// Изменяет подтип элемента. 
        /// </summary>
        /// <param name="subType">Подтип элемента</param>
        /// <returns>Возвращает ScResult.SC_RESULT_OK, если подтип изменен</returns>
        public ScResult ChangeSubType(ElementType subType)
        {
            return NativeMethods.sc_memory_change_element_subtype(this.scContext.PtrScMemoryContext, this.ScAddress.WScAddress, subType);
        }

        /// <summary>
        /// Добавляет входящую дугу
        /// </summary>
        /// <param name="beginElement">Начальный элемент</param>
        /// <param name="arcType">Тип дуги</param>
        /// <returns></returns>
        public ScArc AddInputArc(ScElement beginElement, ElementType arcType)
        {
            return ScMemorySafeMethods.CreateArc(this.scContext, arcType, beginElement, this);
        }

        /// <summary>
        /// Добавляет исходящую дугу
        /// </summary>
        /// <param name="endElement">Конечный элемент</param>
        /// <param name="arcType">Тип дуги</param>
        /// <returns></returns>
        public ScArc AddOutputArc(ScElement endElement, ElementType arcType)
        {
            return ScMemorySafeMethods.CreateArc(this.scContext, arcType, this, endElement);
        }

        /// <summary>
        /// Создает событие для элемента
        /// </summary>
        /// <param name="element">Sc-элемент</param>
        /// <param name="eventType">Тип события</param>
        /// <returns>Возвращает событие <see cref="ScEvent"/> </returns>
        public ScEvent CreateEvent(ScEventType eventType)
        {
            var scEvent = new ScEvent(this.ScAddress, eventType);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {

                scEvent.Subscribe(this.scContext);
            }
            return scEvent;
        }





        /// <summary>
        /// Находит элемент по узлу неролевого отношения и узлу класса.
        /// </summary>
        /// <param name="nrelNode">Узел неролевого отношения</param>
        /// <param name="classNode">Узел класса</param>
        /// <param name="findElementType">Тип искомого элемента</param>
        /// <returns></returns>
        public ScElement GetElementByNrelClass(ScNode nrelNode, ScNode classNode, ElementType findElementType)
        {
            ScElement element = null;
           

            var container5 = this.scContext.CreateIterator(this, ElementType.ConstantCommonArc_c, findElementType, ElementType.PositiveConstantPermanentAccessArc_c, nrelNode);
            foreach (var construction in container5)
            {
                var container3 = this.scContext.CreateIterator(classNode, ElementType.PositiveConstantPermanentAccessArc_c, construction.Elements[2]);
                if (container3.GetAllConstructions().Count != 0)
                {
                    element = construction.Elements[2];
                    break;
                }
            }

            return element;
        }

        private  bool isValid()
        {
            bool isValid = false;
            if (this!=null && this.scAddress.IsValid )
            { isValid = true; }
            return isValid;
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
        /// <param name="scElement1">Первый элемент</param>
        /// <param name="scElement2">Второй элемент</param>
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
