using System;
using ScEngineNet.Events;
using ScEngineNet.LinkContent;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Элемент Sc-ссылка. В ней можно хранить любое содержимое. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScLink : ScElement
    {
        internal ScLink(ScAddress linkAddress, ScMemoryContext scContext)
            : base(linkAddress, scContext)
        {
            contentChangeEvent = new ScEvent(ScAddress, ScEventType.ScEventContentChanged);
        }

        /// <summary>
        ///     Возвращает или устанавливает содержимое для ссылки. Тип содержимого устанавливается автоматически.
        /// </summary>
        /// <value>
        ///     Содержимое ссылки.
        /// </value>
        public ScLinkContent LinkContent
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("ScLink", DisposalExceptionMsg);
                }
                if (ScMemoryContext.IsMemoryInitialized() != true)
                {
                    throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
                }
                if (ScContext.PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ScContextInvalidException(ContextInvalidExceptionMsg);
                }

                return ScContext.GetLinkContent(this);
            }
            set
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("ScLink", DisposalExceptionMsg);
                }
                if (ScMemoryContext.IsMemoryInitialized() != true)
                {
                    throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
                }
                if (ScContext.PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ScContextInvalidException(ContextInvalidExceptionMsg);
                }
                ScContext.SetLinkContent(value, this);
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected new virtual void Dispose(bool disposing)
        {
          //  contentChangeEvent.Dispose();
            LinkContent.Dispose();
            base.Dispose(disposing);
        }

        #region ContentChangedEvent

        private ScEvent contentChangeEvent; //не забываем добавить в конструктор начальную инициализацию
        internal static readonly EventKey ContentChangeEventKey = new EventKey();

        /// <summary>
        ///     Occurs when [content changed].
        /// </summary>
        public event ElementEventHandler ContentChanged
        {
            add
            {
                //subscribe           
                contentChangeEvent = CreateEvent(ScEventType.ScEventContentChanged);
                contentChangeEvent.ElementEvent += contentChangeEvent_ElementEvent;
                EventSet.Add(ContentChangeEventKey, value);
            }
            remove
            {
                //delete
                EventSet.Remove(ContentChangeEventKey, value);
                //contentChangeEvent.Dispose();
            }
        }

        private void contentChangeEvent_ElementEvent(object sender, ScEventArgs e)
        {
            OnContentChangeEvent(e);
        }


        /// <summary>
        ///     Raises the <see cref="E:ContentChangeEvent" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ScEventArgs" /> instance containing the event data.</param>
        protected virtual void OnContentChangeEvent(ScEventArgs args)
        {
            EventSet.Raise(ContentChangeEventKey, this, args);
        }

        #endregion

        public override string ToString()
        {
            return LinkContent.ToString();
        }
    }
}