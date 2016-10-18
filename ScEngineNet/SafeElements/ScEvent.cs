using ScEngineNet.NativeElements;
using System;
using System.Runtime.InteropServices;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Делегат события
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ScEventArgs"/> instance containing the event data.</param>
    public delegate void ElementEventHandler(object sender, ScEventArgs e);
    /// <summary>
    /// Делегат удаления элемента, подписанного на событие
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ScEventArgs"/> instance containing the event data.</param>
    public delegate void ElementDeleteHandler(object sender, ScEventArgs e);

    /// <summary>
    /// Событие для элемента. Создается посредством вызова метода CreateEvent класса <see cref="ScMemoryContext" />
    /// </summary>
    internal class ScEvent : IDisposable
    {
        private const string disposalException_msg = "Был вызван метод Dispose и cсылка на объект в памяти уже удалена";
        private const string memoryNotInitializedException_msg = "Библиотека ScMemory.Net не инициализирована";
        private const string contextInvalidException_msg = "Указанная ссылка на ScContext не действительна";

        private IntPtr wScEvent;
        private ScAddress elementAddress;
        private ScMemoryContext context;
        private readonly ScEventType eventType;

        /// <summary>
        /// Событие элемента
        /// </summary>
        public event ElementEventHandler ElementEvent;
        /// <summary>
        /// Событие удаления элемента
        /// </summary>
        public event ElementDeleteHandler ElementDelete;

        internal void OnElementEvent(ScEventType eventType, ScAddress elementAddress, ScAddress arcAddress)
        {
            if (this.Disposed == true) { throw new ObjectDisposedException("ScEvent", disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.context.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            if (ElementEvent != null)
            {
                ScEventArgs args = new ScEventArgs(eventType, ScMemorySafeMethods.GetElement(elementAddress.WScAddress, this.context), new ScArc(arcAddress, this.context));
                ElementEvent(this, args);
            }
        }

        internal void OnElementDelete(ScAddress elementAddress)
        {
            if (this.Disposed == true) { throw new ObjectDisposedException("ScEvent", disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.context.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            if (ElementDelete != null)
            {
                ScEventArgs args = new ScEventArgs(ScEventType.SC_EVENT_REMOVE_ELEMENT, ScMemorySafeMethods.GetElement(elementAddress.WScAddress, this.context), new ScArc(ScAddress.Invalid, context));
                ElementDelete(this, args);
            }
        }

        /// <summary>
        /// Возвращает адрес элемента, подписанного на событие.
        /// </summary>
        /// <value>
        /// The element address.
        /// </value>
        public ScAddress ElementAddress
        {
            get { return elementAddress; }
        }



        /// <summary>
        /// Возвращает тип события.
        /// </summary>
        /// <value>
        /// Тип события <see cref="ScEventType"/>
        /// </value>
        public ScEventType EventType
        {
            get { return eventType; }
        }

        internal IntPtr WScEvent
        {
            get { return wScEvent; }
        }


        internal ScEvent(ScAddress elementAddress, ScEventType eventType)
        {
            this.elementAddress = elementAddress;
            this.eventType = eventType;
        }

        internal bool Subscribe(ScMemoryContext context)
        {



            this.context = context;
            fEventCallback cb = new fEventCallback(ECallback);
            fDeleteCallback db = new fDeleteCallback(DCallback);

            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.context.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            this.wScEvent = NativeMethods.sc_event_new(this.context.PtrScMemoryContext, this.elementAddress.WScAddress, this.eventType, IntPtr.Zero, cb, db);
            return this.wScEvent != IntPtr.Zero ? true : false;
        }


        private ScResult ECallback(ref WScEvent scEvent, WScAddress arg)
        {
            OnElementEvent(scEvent.Type, new ScAddress(scEvent.Element), new ScAddress(arg));
            return ScResult.SC_RESULT_OK;
        }

        private ScResult DCallback(ref WScEvent scEvent)
        {
            OnElementDelete(new ScAddress(scEvent.Element));
            return ScResult.SC_RESULT_OK;
        }


        #region IDisposal

        /// <summary>
        /// Удаляет событие
        /// </summary>
        /// <returns></returns>
        private bool Delete()
        {
            bool isDelete = false;
            if (ScMemoryContext.IsMemoryInitialized() == true && this.wScEvent != IntPtr.Zero)
            {
                isDelete = NativeMethods.sc_event_destroy(this.wScEvent) == ScResult.SC_RESULT_OK ? true : false;
                this.wScEvent = IntPtr.Zero;
            }
            else
            {
                isDelete = true;
            }
            return isDelete;
        }


        private bool disposed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ScEvent"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get { return disposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("call Dispose({0}) ScEvent with {1}", disposing, this.ToString());


            if (!disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                this.Delete();
                // Suppress finalization of this disposed instance.
                if (disposing)
                {

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

        ~ScEvent()
        {
            Dispose(false);
        }
        #endregion




    }
}
