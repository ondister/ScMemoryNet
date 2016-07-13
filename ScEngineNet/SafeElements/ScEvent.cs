using ScEngineNet.NativeElements;
using System;

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
    public class ScEvent : IDisposable
    {
        private IntPtr wScEvent;
        private ScAddress elementAddress;
        private IntPtr context;
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
            if (ElementEvent != null)
            {
                ScEventArgs args = new ScEventArgs(eventType, ScMemorySafeMethods.GetElement(elementAddress.WScAddress, this.context), new ScArc(arcAddress, this.context));
                ElementEvent(this, args);
            }
        }

        internal void OnElementDelete(ScAddress elementAddress)
        {
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

        internal ScEvent(IntPtr wScEvent)
        {
            this.wScEvent = wScEvent;
            if (wScEvent != IntPtr.Zero)
            {
                this.elementAddress = new ScAddress(NativeMethods.sc_event_get_element(this.wScEvent));
                this.eventType = NativeMethods.sc_event_get_type(this.wScEvent);
            }
        }

        internal ScEvent(ScAddress elementAddress, ScEventType eventType)
        {
            this.elementAddress = elementAddress;
            this.eventType = eventType;
        }

        internal bool Subscribe(IntPtr context)
        {
            fEventCallback cb = new fEventCallback(ECallback);
            fDeleteCallback db = new fDeleteCallback(DCallback);
            this.context = context;
            this.wScEvent = NativeMethods.sc_event_new(this.context, this.elementAddress.WScAddress, this.eventType, IntPtr.Zero, cb, db);
            return this.wScEvent != IntPtr.Zero ? true : false;
        }

        /// <summary>
        /// Удаляет событие
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            bool isDelete = false;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                isDelete = NativeMethods.sc_event_destroy(this.wScEvent) == ScResult.SC_RESULT_OK ? true : false;
            }
            else
            {
                isDelete = true;
            }
            return isDelete;
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

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.elementAddress = null;
                }
                //unmanaged
                this.Delete();
                wScEvent = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScEvent"/> class.
        /// </summary>
        ~ScEvent()
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
    }
}
