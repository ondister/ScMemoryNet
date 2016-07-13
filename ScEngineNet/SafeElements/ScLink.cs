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
                return ScMemorySafeMethods.GetLinkContent(base.scContext, this);
            }
            set
            {
                ScMemorySafeMethods.SetLinkContent(base.scContext, value, this);
            }
        }

       

        internal ScLink(ScAddress linkAddress, IntPtr scContext)
            : base(linkAddress,scContext)
        { }
    }
}
