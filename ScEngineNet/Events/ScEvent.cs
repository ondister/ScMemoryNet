using System;
using ScEngineNet.Native;
using ScEngineNet.ScElements;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.Events
{
    /// <summary>
    ///     Делегат события
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ScEventArgs" /> instance containing the event data.</param>
    public delegate void ElementEventHandler(object sender, ScEventArgs e);

    /// <summary>
    ///     Делегат удаления элемента, подписанного на событие
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ScEventArgs" /> instance containing the event data.</param>
    public delegate void ElementDeleteHandler(object sender, ScEventArgs e);

    /// <summary>
    ///     Событие для элемента. Создается посредством вызова метода CreateEvent класса <see cref="ScMemoryContext" />
    /// </summary>
    internal class ScEvent : IDisposable
    {
        private const string disposalExceptionMsg = "Был вызван метод Dispose и cсылка на объект в памяти уже удалена";
        private const string memoryNotInitializedExceptionMsg = "Библиотека ScMemory.Net не инициализирована";
        private const string contextInvalidExceptionMsg = "Указанная ссылка на ScContext не действительна";
        private fEventCallbackEx cb;
        private ScMemoryContext context;
        private fDeleteCallback db;

        internal ScEvent(ScAddress elementAddress, ScEventType eventType)
        {
            ElementAddress = elementAddress;
            EventType = eventType;
        }

        /// <summary>
        ///     Возвращает адрес элемента, подписанного на событие.
        /// </summary>
        /// <value>
        ///     The element address.
        /// </value>
        public ScAddress ElementAddress { get; }

        /// <summary>
        ///     Возвращает тип события.
        /// </summary>
        /// <value>
        ///     Тип события <see cref="ScEventType" />
        /// </value>
        public ScEventType EventType { get; }

        internal IntPtr WScEvent { get; private set; }

        /// <summary>
        ///     Событие элемента
        /// </summary>
        public event ElementEventHandler ElementEvent;

        /// <summary>
        ///     Событие удаления элемента
        /// </summary>
        public event ElementDeleteHandler ElementDelete;

        internal void OnElementEvent(ScEventType eventType, ScAddress elementAddress, ScAddress arcAddress)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("ScEvent", disposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (context.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }

            if (ElementEvent != null)
            {
                if (eventType != ScEventType.ScEventRemoveElement)
                {
                    var args = new ScEventArgs(eventType, context.GetElement(elementAddress),
                        new ScArc(arcAddress, context));
                    ElementEvent(this, args);
                }
                else
                {
                    var args = new ScEventArgs(eventType, null, null);
                    ElementEvent(null, args);
                }
            }
        }

        internal void OnElementDelete(ScAddress elementAddress)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("ScEvent", disposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (context.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }

            if (ElementDelete != null)
            {
                var args = new ScEventArgs(ScEventType.ScEventRemoveElement, null, null);
                ElementDelete(null, args);
            }
        }

        internal bool Subscribe(ScMemoryContext context)
        {
            this.context = context;

            cb = ECallback;
            db = DCallback;
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (this.context.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }

            WScEvent = NativeMethods.sc_event_new_ex(this.context.PtrScMemoryContext, ElementAddress.WScAddress,
                EventType, IntPtr.Zero, cb, db);
            return WScEvent != IntPtr.Zero;
        }

        //TODO:otherElement тоже переместить в события
        private ScResult ECallback(ref WScEvent scEvent, WScAddress arg, WScAddress otherElement)
        {
            OnElementEvent(scEvent.Type, new ScAddress(scEvent.Element), new ScAddress(arg));
            return ScResult.ScResultOk;
        }

        private ScResult DCallback(ref WScEvent scEvent)
        {
            OnElementDelete(new ScAddress(scEvent.Element));
            return ScResult.ScResultOk;
        }

        #region IDisposal

        /// <summary>
        ///     Удаляет событие
        /// </summary>
        /// <returns></returns>
        private bool Delete()
        {
            var isDelete = false;
            if (ScMemoryContext.IsMemoryInitialized() && WScEvent != IntPtr.Zero)
            {
                if (EventType != ScEventType.ScEventRemoveElement)
                {
                    isDelete = NativeMethods.sc_event_destroy(WScEvent) == ScResult.ScResultOk;
                    cb = null;
                    db = null;
                   WScEvent = IntPtr.Zero;
                }
            }
            else
            {
                isDelete = true;
            }
            return isDelete;
        }


        /// <summary>
        ///     Gets a value indicating whether this <see cref="ScEvent" /> is disposed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            // Console.WriteLine("call Dispose({0}) ScEvent with {1}", disposing, this.ToString());


            if (!Disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                Delete();
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

        ~ScEvent()
        {
            Dispose(false);
        }

        #endregion
    }
}