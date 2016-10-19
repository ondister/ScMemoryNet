using System;
using System.Linq;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Базовый класс для всех sc-элементов памяти: sc-узла, sc-ссылки и sc-дуги. Напрямую создать его нельзя
    /// </summary>
    public class ScElement : IEquatable<ScElement>, IDisposable
    {
        private ScMemoryContext scContext;

        internal ScMemoryContext ScContext
        {
            get { return scContext; }
        }
        private ScAddress scAddress;

        /// <summary>
        /// The disposal exception_msg
        /// </summary>
        protected const string disposalException_msg = "Был вызван метод Dispose и cсылка на объект в памяти уже удалена";
        /// <summary>
        /// The memory not initialized exception_msg
        /// </summary>
        protected const string memoryNotInitializedException_msg = "Библиотека ScMemory.Net не инициализирована";
        /// <summary>
        /// The context invalid exception_msg
        /// </summary>
        protected const string contextInvalidException_msg = "Указанная ссылка на ScContext не действительна";

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
                return this.isValid();
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


        internal ScElement(ScAddress scAddress, ScMemoryContext scContext)
        {
            this.scContext = scContext;
            this.scAddress = scAddress;

            this.scOutputArcAddedEvent = new ScEvent(this.scAddress, ScEventType.SC_EVENT_ADD_OUTPUT_ARC);
            this.scOutputArcRemovedEvent = new ScEvent(this.scAddress, ScEventType.SC_EVENT_REMOVE_OUTPUT_ARC);

            this.scInputArcAddedEvent = new ScEvent(this.scAddress, ScEventType.SC_EVENT_ADD_INPUT_ARC);
            this.scInputArcRemovedEvent = new ScEvent(this.scAddress, ScEventType.SC_EVENT_REMOVE_INPUT_ARC);

            this.scElementRemovedEvent = new ScEvent(this.scAddress, ScEventType.SC_EVENT_REMOVE_ELEMENT);
        }

        /// <summary>
        /// Удаляет элемент из памяти
        /// </summary>
        /// <returns>Возвращает ScResult.SC_RESULT_OK, если элемент удален</returns>
        public ScResult DeleteFromMemory()
        {
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            ScResult isDeleted = ScResult.SC_RESULT_ERROR;
            isDeleted = ScMemorySafeMethods.DeleteElement(this.scContext, this);
            this.scAddress = ScAddress.Invalid;

            return isDeleted;
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
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

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
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            return ScMemorySafeMethods.CreateArc(this.scContext, arcType, this, endElement);
        }

        /// <summary>
        /// Creates the event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns></returns>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="ScMemoryNotInitializeException"></exception>
        /// <exception cref="ScContextInvalidException"></exception>
        internal ScEvent CreateEvent(ScEventType eventType)
        {
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            var scEvent = new ScEvent(this.ScAddress, eventType);
            scEvent.Subscribe(this.scContext);
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

            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            ScElement element = null;

            var container5 = this.scContext.CreateIterator(this, ElementType.ConstantCommonArc_c, findElementType, ElementType.PositiveConstantPermanentAccessArc_c, nrelNode);
            foreach (var construction in container5)
            {
                var container3 = this.scContext.CreateIterator(classNode, ElementType.PositiveConstantPermanentAccessArc_c, construction[2]);
                if (container3.Count() != 0)
                {
                    element = construction[2];
                    break;
                }
            }

            return element;
        }

        private bool isValid()
        {
            bool isValid = false;
            if (this != null && this.scAddress.IsValid)
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

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            if (this.ScAddress.Offset == obj.ScAddress.Offset && this.ScAddress.Segment == obj.ScAddress.Segment)
            { return true; }
            else
            { return false; }

        }

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="T:System.Object"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object"/></param>
        public override bool Equals(object obj)
        {

            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return this.Equals(obj as ScElement);
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


        #region Events
        private readonly EventSet elementEventSet = new EventSet();
        internal EventSet EventSet { get { return elementEventSet; } }


        #region OutputArcAdded
        private ScEvent scOutputArcAddedEvent;//не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey outputArcAddedEventKey = new EventKey();
        /// <summary>
        /// Occurs when [output arc added].
        /// </summary>
        public event ElementEventHandler OutputArcAdded
        {
            add
            {
                //subscribe           
                this.scOutputArcAddedEvent = this.CreateEvent(ScEventType.SC_EVENT_ADD_OUTPUT_ARC);
                scOutputArcAddedEvent.ElementEvent += scOutputArcAddedEvent_ElementEvent;
                elementEventSet.Add(outputArcAddedEventKey, value);
            }
            remove
            {
                //delete
                elementEventSet.Remove(outputArcAddedEventKey, value);
                this.scOutputArcAddedEvent.Dispose();
            }
        }

        void scOutputArcAddedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnOutputArcAdded(e);
        }

        /// <summary>
        /// Raises the <see cref="E:OutputArcAdded" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs"/> instance containing the event data.</param>
        protected virtual void OnOutputArcAdded(ScEventArgs args)
        {
            elementEventSet.Raise(outputArcAddedEventKey, this, args);
        }

        #endregion

        #region OutputArcRemoved
        private ScEvent scOutputArcRemovedEvent;//не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey outputArcRemovedEventKey = new EventKey();
        /// <summary>
        /// Occurs when [output arc removed].
        /// </summary>
        public event ElementEventHandler OutputArcRemoved
        {
            add
            {
                //subscribe           
                this.scOutputArcRemovedEvent = this.CreateEvent(ScEventType.SC_EVENT_REMOVE_OUTPUT_ARC);
                scOutputArcRemovedEvent.ElementEvent += scOutputArcRemovedEvent_ElementEvent;
                elementEventSet.Add(outputArcRemovedEventKey, value);
            }
            remove
            {
                //delete
                elementEventSet.Remove(outputArcRemovedEventKey, value);
                this.scOutputArcRemovedEvent.Dispose();
            }
        }

        void scOutputArcRemovedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnOutputArcRemoved(e);
        }


        /// <summary>
        /// Raises the <see cref="E:OutputArcRemoved" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs"/> instance containing the event data.</param>
        protected virtual void OnOutputArcRemoved(ScEventArgs args)
        {
            elementEventSet.Raise(outputArcRemovedEventKey, this, args);
        }

        #endregion

        #region InputArcRemoved
        private ScEvent scInputArcRemovedEvent;//не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey inputArcRemovedEventKey = new EventKey();
        /// <summary>
        /// Occurs when [input arc removed].
        /// </summary>
        public event ElementEventHandler InputArcRemoved
        {
            add
            {
                //subscribe           
                this.scInputArcRemovedEvent = this.CreateEvent(ScEventType.SC_EVENT_REMOVE_INPUT_ARC);
                scInputArcRemovedEvent.ElementEvent += scInputArcRemovedEvent_ElementEvent;
                elementEventSet.Add(inputArcRemovedEventKey, value);
            }
            remove
            {
                //delete
                elementEventSet.Remove(inputArcRemovedEventKey, value);
                this.scInputArcRemovedEvent.Dispose();
            }
        }

        void scInputArcRemovedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnInputArcRemoved(e);
        }

        /// <summary>
        /// Raises the <see cref="E:InputArcRemoved" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs"/> instance containing the event data.</param>
        protected virtual void OnInputArcRemoved(ScEventArgs args)
        {
            elementEventSet.Raise(inputArcRemovedEventKey, this, args);
        }

        #endregion

        #region InputArcAdded
        private ScEvent scInputArcAddedEvent;//не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey inputArcAddedEventKey = new EventKey();
        /// <summary>
        /// Occurs when [input arc added].
        /// </summary>
        public event ElementEventHandler InputArcAdded
        {
            add
            {
                //subscribe           
                this.scInputArcAddedEvent = this.CreateEvent(ScEventType.SC_EVENT_ADD_INPUT_ARC);
                scInputArcAddedEvent.ElementEvent += scInputArcAddedEvent_ElementEvent;
                elementEventSet.Add(inputArcAddedEventKey, value);
            }
            remove
            {
                //delete
                elementEventSet.Remove(inputArcAddedEventKey, value);
                this.scInputArcAddedEvent.Dispose();
            }
        }

        void scInputArcAddedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnInputArcAdded(e);
        }

        /// <summary>
        /// Raises the <see cref="E:InputArcAdded" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs"/> instance containing the event data.</param>
        protected virtual void OnInputArcAdded(ScEventArgs args)
        {
            elementEventSet.Raise(inputArcAddedEventKey, this, args);
        }

        #endregion

        #region ElementRemoved
        private ScEvent scElementRemovedEvent;//не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey elementRemovedEventKey = new EventKey();
        /// <summary>
        /// Occurs when [element removed].
        /// </summary>
        public event ElementEventHandler ElementRemoved
        {
            add
            {
                //subscribe           
                this.scElementRemovedEvent = this.CreateEvent(ScEventType.SC_EVENT_REMOVE_ELEMENT);
                scElementRemovedEvent.ElementEvent += scElementRemovedEvent_ElementEvent;
                elementEventSet.Add(elementRemovedEventKey, value);
            }
            remove
            {
                //delete
                elementEventSet.Remove(elementRemovedEventKey, value);
                this.scElementRemovedEvent.Dispose();
            }
        }



        void scElementRemovedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnElementRemoved(e);

            this.scElementRemovedEvent.Dispose();
        }


        /// <summary>
        /// Raises the <see cref="E:ElementRemoved" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs"/> instance containing the event data.</param>
        protected virtual void OnElementRemoved(ScEventArgs args)
        {
            elementEventSet.Raise(elementRemovedEventKey, this, args);
        }

        #endregion



        #endregion

        #region IDisposal
        private bool disposed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ScElement"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("call Dispose({0}) ScElement with {1}", disposing, this.ScAddress);


            if (!disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.



                // Suppress finalization of this disposed instance.
                if (disposing)
                {
                    this.scOutputArcAddedEvent.Dispose();
                    this.scOutputArcRemovedEvent.Dispose();

                    this.scInputArcAddedEvent.Dispose();
                    this.scInputArcRemovedEvent.Dispose();

                    this.scElementRemovedEvent.Dispose();
                    GC.SuppressFinalize(this);
                }
                disposed = true;
            }


        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScElement"/> class.
        /// </summary>
        ~ScElement()
        {
            Dispose(false);
        }
        #endregion

    }
}
