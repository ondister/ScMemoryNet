using ScEngineNet.NativeElements;
using System;

namespace ScEngineNet.SafeElements
{
    public delegate void ElementEventHandler(object sender, ScEventArgs e);
    public delegate void ElementDeleteHandler(object sender, ScEventArgs e);
    public class ScEvent : IDisposable
    {
        private IntPtr wScEvent;
        private ScAddress elementAddress;
        private IntPtr context;
        public event ElementEventHandler ElementEvent;
        public event ElementDeleteHandler ElementDelete;


        internal void OnElementEvent(ScEventType eventType, ScAddress elementAddress, ScAddress arcAddress)
        {
            if (ElementEvent != null)
            {
                ScEventArgs args = new ScEventArgs(eventType, new ScElement(elementAddress,this.context), new ScArc(arcAddress,this.context));

                ElementEvent(this, args);
            }

        }

        internal void OnElementDelete(ScAddress elementAddress)
        {
            if (ElementDelete != null)
            {
                ScEventArgs args = new ScEventArgs(ScEventType.SC_EVENT_REMOVE_ELEMENT, new ScElement(elementAddress, this.context), new ScArc(ScAddress.Invalid, context));
                ElementDelete(this, args);
            }
        }



        public ScAddress ElementAddress
        {
            get { return elementAddress; }
        }
        private ScEventType eventType;

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

        ~ScEvent()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }






    }

}

