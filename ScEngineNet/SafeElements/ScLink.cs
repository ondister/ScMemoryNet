﻿using System;
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
                return ScMemorySafeMethods.GetLinkContent(base.scContext, this);
            }
            set
            {
                ScMemorySafeMethods.SetLinkContent(base.scContext, value, this);
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

      
        protected virtual void OnContentChangeEvent(ScEventArgs args)
        {
            base.EventSet.Raise(contentChangeEventKey, this, args);
        }

        #endregion


    }
}
