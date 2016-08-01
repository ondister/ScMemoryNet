using ScEngineNet.NativeElements;
using System;
using System.Runtime.InteropServices;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ScEventArgs"/> instance containing the event data.</param>
    public delegate void ElementEventHandler(object sender, ScEventArgs e);
    /// <summary>
    /// ������� �������� ��������, ������������ �� �������
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ScEventArgs"/> instance containing the event data.</param>
    public delegate void ElementDeleteHandler(object sender, ScEventArgs e);
    
    /// <summary>
    /// ������� ��� ��������. ��������� ����������� ������ ������ CreateEvent ������ <see cref="ScMemoryContext" />
    /// </summary>
    public class ScEvent:SafeHandle
    {
        private IntPtr wScEvent;
        private ScAddress elementAddress;
        private ScMemoryContext context;
        private readonly ScEventType eventType;

        /// <summary>
        /// ������� ��������
        /// </summary>
        public event ElementEventHandler ElementEvent;
        /// <summary>
        /// ������� �������� ��������
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
        /// ���������� ����� ��������, ������������ �� �������.
        /// </summary>
        /// <value>
        /// The element address.
        /// </value>
        public ScAddress ElementAddress
        {
            get { return elementAddress; }
        }



        /// <summary>
        /// ���������� ��� �������.
        /// </summary>
        /// <value>
        /// ��� ������� <see cref="ScEventType"/>
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
            :base(IntPtr.Zero,true)
        {
            this.elementAddress = elementAddress;
            this.eventType = eventType;
        }

        internal bool Subscribe(ScMemoryContext context)
        {
            fEventCallback cb = new fEventCallback(ECallback);
            fDeleteCallback db = new fDeleteCallback(DCallback);
            this.context = context;
            this.wScEvent = NativeMethods.sc_event_new(this.context.PtrScMemoryContext, this.elementAddress.WScAddress, this.eventType, IntPtr.Zero, cb, db);
            return this.wScEvent != IntPtr.Zero ? true : false;
        }

        /// <summary>
        /// ������� �������
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


        /// <summary>
        /// ��� ��������������� � ����������� ������ �������� ��������, ������������, ��������� �� �������� �����������.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override bool IsInvalid
        {
            get { return this.wScEvent==IntPtr.Zero; }
        }
        /// <summary>
        /// ��� ��������������� � ����������� ������ ��������� ���, ����������� ��� ������������ �����������.
        /// </summary>
        /// <returns>
        /// �������� true, ���� ���������� ������������� �������, � ��������� ������, � ������ ���������������� ������ � ��������  false.� ����� ������ ��������� ����������� �������� �� ������� releaseHandleFailed MDA.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            this.Delete();
            return !IsInvalid;
        }
    }
}
