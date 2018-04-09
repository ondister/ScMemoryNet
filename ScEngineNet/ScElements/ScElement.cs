using System;
using System.Linq;
using ScEngineNet.Events;
using ScEngineNet.Native;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Базовый класс для всех sc-элементов памяти: sc-узла, sc-ссылки и sc-дуги. Напрямую создать его нельзя
    /// </summary>
    public class ScElement : IEquatable<ScElement>, IDisposable
    {
        /// <summary>
        ///     The disposal exception_msg
        /// </summary>
        protected const string DisposalExceptionMsg =
            "Был вызван метод Dispose и cсылка на объект в памяти уже удалена";

        /// <summary>
        ///     The memory not initialized exception_msg
        /// </summary>
        protected const string MemoryNotInitializedExceptionMsg = "Библиотека ScMemory.Net не инициализирована";

        /// <summary>
        ///     The context invalid exception_msg
        /// </summary>
        protected const string ContextInvalidExceptionMsg = "Указанная ссылка на ScContext не действительна";

        internal ScElement(ScAddress scAddress, ScMemoryContext scContext)
        {
            ScContext = scContext;
            ScAddress = scAddress;
            EventSet = new EventSet();

            scOutputArcAddedEvent = new ScEvent(ScAddress, ScEventType.ScEventAddOutputArc);
            scOutputArcRemovedEvent = new ScEvent(ScAddress, ScEventType.ScEventRemoveOutputArc);

            scInputArcAddedEvent = new ScEvent(ScAddress, ScEventType.ScEventAddInputArc);
            scInputArcRemovedEvent = new ScEvent(ScAddress, ScEventType.ScEventRemoveInputArc);

            scElementRemovedEvent = new ScEvent(ScAddress, ScEventType.ScEventRemoveElement);
        }

        internal ScMemoryContext ScContext { get; private set; }

        /// <summary>
        ///     Вовращает тип элемента
        /// </summary>
        /// <value>
        ///     Тип элемента
        /// </value>
        public ScTypes ElementType
        {
            get { return GetElementType(); }
        }

        /// <summary>
        ///     Returns true if ScElement is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get { return ScAddress.IsValid; }
        }

        /// <summary>
        ///     Возвращает адрес элемента
        /// </summary>
        /// <value>
        ///     Sc-адрес
        /// </value>
        public ScAddress ScAddress { get; set; }

        private ScTypes GetElementType()
        {
            ElementTypes elementType;
            NativeMethods.sc_memory_get_element_type(ScContext.PtrScMemoryContext, ScAddress.WScAddress,
                out elementType);
            return new ScTypes(elementType);
        }

        /// <summary>
        ///     Удаляет элемент из памяти
        /// </summary>
        /// <returns>Возвращает ScResult.SC_RESULT_OK, если элемент удален</returns>
        public ScResult DeleteFromMemory()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), DisposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
            }
            if (ScContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(ContextInvalidExceptionMsg);
            }

            var isDeleted = DeleteElement();
            ScAddress = ScAddress.Invalid;

            return isDeleted;
        }

        private ScResult DeleteElement()
        {
            var result = NativeMethods.sc_memory_element_free(ScContext.PtrScMemoryContext, ScAddress.WScAddress);
            return result;
        }

        /// <summary>
        ///     Добавляет входящую дугу
        /// </summary>
        /// <param name="beginElement">Начальный элемент</param>
        /// <param name="arcType">Тип дуги</param>
        /// <returns></returns>
        public ScArc AddInputArc(ScTypes arcType, ScElement beginElement)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), DisposalExceptionMsg);
            }
            if (ScContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(ContextInvalidExceptionMsg);
            }

            return ScContext.CreateArc(beginElement, this, arcType);
        }

        /// <summary>
        ///     Добавляет исходящую дугу
        /// </summary>
        /// <param name="endElement">Конечный элемент</param>
        /// <param name="arcType">Тип дуги</param>
        /// <returns></returns>
        public ScArc AddOutputArc(ScElement endElement, ScTypes arcType)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), DisposalExceptionMsg);
            }
            if (ScContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(ContextInvalidExceptionMsg);
            }

            return ScContext.CreateArc(this, endElement, arcType);
        }

        /// <summary>
        ///     Creates the event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns></returns>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="ScMemoryNotInitializeException"></exception>
        /// <exception cref="ScContextInvalidException"></exception>
        internal ScEvent CreateEvent(ScEventType eventType)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), DisposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
            }
            if (ScContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(ContextInvalidExceptionMsg);
            }

            var scEvent = new ScEvent(ScAddress, eventType);
            scEvent.Subscribe(ScContext);
            return scEvent;
        }

        /// <summary>
        ///     Находит элемент по узлу неролевого отношения и узлу класса.
        /// </summary>
        /// <param name="nrelNode">Узел неролевого отношения</param>
        /// <param name="classNode">Узел класса</param>
        /// <param name="findElementType">Тип искомого элемента</param>
        /// <returns></returns>
        public ScElement GetElementByNrelClass(ScNode nrelNode, ScNode classNode, ScTypes findElementType)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), DisposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
            }
            if (ScContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(ContextInvalidExceptionMsg);
            }

            ScElement element = null;

            var container5 = ScContext.CreateIterator(this, ScTypes.ArcCommonConstant, findElementType,
                ScTypes.ArcAccessConstantPositivePermanent, nrelNode);
            foreach (var construction in container5)
            {
                var container3 = ScContext.CreateIterator(classNode, ScTypes.ArcAccessConstantPositivePermanent,
                    construction[2]);
                if (container3.Count() != 0)
                {
                    element = construction[2];
                    break;
                }
            }

            return element;
        }

        #region Реализация сравнения

        /// <summary>
        ///     Определяет равен ли заданный объект <see cref="ScElement" /> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="ScElement" /></param>
        public bool Equals(ScElement obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            if (ScAddress.Offset == obj.ScAddress.Offset && ScAddress.Segment == obj.ScAddress.Segment)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Определяет равен ли заданный объект <see cref="T:System.Object" /> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object" /></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Equals(obj as ScElement);
        }

        /// <summary>
        ///     Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return Convert.ToInt32(ScAddress.Offset + ScAddress.Segment.ToString());
        }

        /// <summary>
        ///     Оператор сравнения элементов
        /// </summary>
        /// <param name="scElement1">Первый элемент</param>
        /// <param name="scElement2">Второй элемент</param>
        /// <returns>Возвращает True, если элементы равны</returns>
        public static bool operator ==(ScElement scElement1, ScElement scElement2)
        {
            if (ReferenceEquals(scElement1, null) || ReferenceEquals(scElement2, null))
            {
                return ReferenceEquals(scElement1, scElement2);
            }
            return scElement1.Equals(scElement2);
        }

        /// <summary>
        ///     Оператор сравнения элементов
        /// </summary>
        /// <param name="scElement1">Первый элемент</param>
        /// <param name="scElement2">Второй элемент</param>
        /// <returns>Возвращает True, если  элементы равны</returns>
        public static bool operator !=(ScElement scElement1, ScElement scElement2)
        {
            //return !(scElement1 == scElement2);
            if (ReferenceEquals(scElement1, null) || ReferenceEquals(scElement2, null))
            {
                return !ReferenceEquals(scElement1, scElement2);
            }
            return !scElement1.Equals(scElement2);
        }

        #endregion

        #region Events
       
        internal EventSet EventSet { get; private set; }

        #region OutputArcAdded

        private ScEvent scOutputArcAddedEvent; //не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey OutputArcAddedEventKey = new EventKey();

        /// <summary>
        ///     Occurs when [output arc added].
        /// </summary>
        public event ElementEventHandler OutputArcAdded
        {
            add
            {
                //subscribe           
                scOutputArcAddedEvent = CreateEvent(ScEventType.ScEventAddOutputArc);
                scOutputArcAddedEvent.ElementEvent += scOutputArcAddedEvent_ElementEvent;
                EventSet.Add(OutputArcAddedEventKey, value);
            }
            remove
            {
                //delete
                EventSet.Remove(OutputArcAddedEventKey, value);
                if (scOutputArcAddedEvent.UnSubscribe())
                {
                   // scOutputArcAddedEvent.Dispose();
                }
            }
        }

        private void scOutputArcAddedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnOutputArcAdded(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OutputArcAdded" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs" /> instance containing the event data.</param>
        protected virtual void OnOutputArcAdded(ScEventArgs args)
        {
            EventSet.Raise(OutputArcAddedEventKey, this, args);
        }

        #endregion

        #region OutputArcRemoved

        private ScEvent scOutputArcRemovedEvent; //не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey OutputArcRemovedEventKey = new EventKey();

        /// <summary>
        ///     Occurs when [output arc removed].
        /// </summary>
        public event ElementEventHandler OutputArcRemoved
        {
            add
            {
                //subscribe           
                scOutputArcRemovedEvent = CreateEvent(ScEventType.ScEventRemoveOutputArc);
                scOutputArcRemovedEvent.ElementEvent += scOutputArcRemovedEvent_ElementEvent;
                EventSet.Add(OutputArcRemovedEventKey, value);
            }
            remove
            {
                //delete
                EventSet.Remove(OutputArcRemovedEventKey, value);
                if (scOutputArcRemovedEvent.UnSubscribe())
                {
                  //  scOutputArcRemovedEvent.Dispose();
                }
            }
        }

        private void scOutputArcRemovedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnOutputArcRemoved(e);
        }


        /// <summary>
        ///     Raises the <see cref="E:OutputArcRemoved" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs" /> instance containing the event data.</param>
        protected virtual void OnOutputArcRemoved(ScEventArgs args)
        {
            EventSet.Raise(OutputArcRemovedEventKey, this, args);
        }

        #endregion

        #region InputArcRemoved

        private ScEvent scInputArcRemovedEvent; //не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey InputArcRemovedEventKey = new EventKey();

        /// <summary>
        ///     Occurs when [input arc removed].
        /// </summary>
        public event ElementEventHandler InputArcRemoved
        {
            add
            {
                //subscribe           
                scInputArcRemovedEvent = CreateEvent(ScEventType.ScEventRemoveInputArc);
                scInputArcRemovedEvent.ElementEvent += scInputArcRemovedEvent_ElementEvent;
                EventSet.Add(InputArcRemovedEventKey, value);
            }
            remove
            {
                //delete
                EventSet.Remove(InputArcRemovedEventKey, value);
                if (scInputArcRemovedEvent.UnSubscribe())
                {
                   // scInputArcRemovedEvent.Dispose();
                }
            }
        }

        private void scInputArcRemovedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnInputArcRemoved(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:InputArcRemoved" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs" /> instance containing the event data.</param>
        protected virtual void OnInputArcRemoved(ScEventArgs args)
        {
            EventSet.Raise(InputArcRemovedEventKey, this, args);
        }

        #endregion

        #region InputArcAdded

        private ScEvent scInputArcAddedEvent; //не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey InputArcAddedEventKey = new EventKey();

        /// <summary>
        ///     Occurs when [input arc added].
        /// </summary>
        public event ElementEventHandler InputArcAdded
        {
            add
            {
                //subscribe           
                scInputArcAddedEvent = CreateEvent(ScEventType.ScEventAddInputArc);
                scInputArcAddedEvent.ElementEvent += scInputArcAddedEvent_ElementEvent;
                EventSet.Add(InputArcAddedEventKey, value);
            }
            remove
            {
                //delete
                EventSet.Remove(InputArcAddedEventKey, value);
                if (scInputArcAddedEvent.UnSubscribe())
                {
                  //  scInputArcAddedEvent.Dispose();
                }
            }
        }

        private void scInputArcAddedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnInputArcAdded(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:InputArcAdded" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs" /> instance containing the event data.</param>
        protected virtual void OnInputArcAdded(ScEventArgs args)
        {
            EventSet.Raise(InputArcAddedEventKey, this, args);
        }

        #endregion

        #region ElementRemoved

        private ScEvent scElementRemovedEvent; //не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey ElementRemovedEventKey = new EventKey();

        /// <summary>
        ///     Occurs when [element removed].
        /// </summary>
        public event ElementEventHandler ElementRemoved
        {
            add
            {
                //subscribe           
                scElementRemovedEvent = CreateEvent(ScEventType.ScEventRemoveElement);
                scElementRemovedEvent.ElementEvent += scElementRemovedEvent_ElementEvent;
                EventSet.Add(ElementRemovedEventKey, value);
            }
            remove
            {
                //delete
                EventSet.Remove(ElementRemovedEventKey, value);
                if (scElementRemovedEvent.UnSubscribe())
                {
                   // scElementRemovedEvent.Dispose();
                }
            }
        }


        private void scElementRemovedEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnElementRemoved(e);

          //  scElementRemovedEvent.Dispose();
        }


        /// <summary>
        ///     Raises the <see cref="E:ElementRemoved" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs" /> instance containing the event data.</param>
        protected virtual void OnElementRemoved(ScEventArgs args)
        {
            EventSet.Raise(ElementRemovedEventKey, this, args);
        }

        #endregion

        #endregion

        #region IDisposal

        /// <summary>
        ///     Gets a value indicating whether this <see cref="ScElement" /> is disposed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Console.WriteLine("call Dispose({0}) ScElement with {1}", disposing, this.ScAddress);


            if (!Disposed && ScMemoryContext.IsMemoryInitialized())
            {
              
                // Suppress finalization of this disposed instance.
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
                Disposed = true;
            }
        }

        /// <summary>
        ///     Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="ScElement" /> class.
        /// </summary>
        ~ScElement()
        {
            Dispose(false);
        }

        #endregion
    }
}