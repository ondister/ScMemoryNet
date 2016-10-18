using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Sc-дуга. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScArc : ScElement
    {
        ScElement beginElement;

        /// <summary>
        /// Возвращает начальный элемент дуги
        /// </summary>
        /// <value>
        /// Начальный элемент. Его можно приводить к необходимому типу.
        /// </value>
        public ScElement BeginElement
        {
            get
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScArc", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
               
                this.beginElement = ScMemorySafeMethods.GetArcBeginElement(base.ScContext, this);
                return beginElement;
            }
        }

        private ScElement endElement;


        /// <summary>
        /// Возарвщает конечный элемент дуги
        /// </summary>
        /// <value>
        /// Конечный элемент. Его можно приводить к необходимому типу.
        /// </value>
        public ScElement EndElement
        {
            get
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScArc", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
               
                this.endElement = ScMemorySafeMethods.GetArcEndElement(base.ScContext, this);
                return this.endElement;
            }
        }

        internal ScArc(ScAddress arcAddress, ScMemoryContext scExtContent)
            : base(arcAddress, scExtContent)
        { }
    }
}
