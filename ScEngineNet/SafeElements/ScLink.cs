using System;
using System.Collections.Generic;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Элемент Sc-ссылка. В ней можно хранить любое содержимое. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScLink : ScElement
    {
        /// <summary>
        /// Возвращает или устанавливает содержимое для ссылки. Тип содержимого устанавливается автоматически.
        /// </summary>
        /// <value>
        /// Содержимое ссылки.
        /// </value>
        public ScLinkContent LinkContent
        {
            get
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScLink", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
               
                return ScMemorySafeMethods.GetLinkContent(base.ScContext, this);
            }
            set
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScLink", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
                
                ScMemorySafeMethods.SetLinkContent(base.ScContext, value, this);
            }
        }



        internal ScLink(ScAddress linkAddress, ScMemoryContext scContext)
            : base(linkAddress,scContext)
        {
            this.contentChangeEvent = new ScEvent(this.ScAddress, ScEventType.SC_EVENT_CONTENT_CHANGED);
        }

     


        #region ContentChangedEvent
        private ScEvent contentChangeEvent;//не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey contentChangeEventKey = new EventKey();
        /// <summary>
        /// Occurs when [content changed].
        /// </summary>
        public event ElementEventHandler ContentChanged
        {
            add
            {
                //subscribe           
                this.contentChangeEvent = this.CreateEvent(ScEventType.SC_EVENT_CONTENT_CHANGED);
                contentChangeEvent.ElementEvent += contentChangeEvent_ElementEvent;
                base.EventSet.Add(contentChangeEventKey, value);
            }
            remove
            {
                //delete
                base.EventSet.Remove(contentChangeEventKey, value);
                this.contentChangeEvent.Dispose();
            }
        }

        void contentChangeEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnContentChangeEvent(e);
        }


        /// <summary>
        /// Raises the <see cref="E:ContentChangeEvent" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs"/> instance containing the event data.</param>
        protected virtual void OnContentChangeEvent(ScEventArgs args)
        {
            base.EventSet.Raise(contentChangeEventKey, this, args);
        }

        #endregion

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual new void Dispose(bool disposing)
        {
            this.contentChangeEvent.Dispose();
            this.LinkContent.Dispose();
            base.Dispose(disposing);

        }

    }
}
