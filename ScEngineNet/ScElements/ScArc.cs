using System;
using ScEngineNet.Native;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Sc-дуга. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScArc : ScElement
    {
        private ScElement beginElement;
        private ScElement endElement;

        internal ScArc(ScAddress arcAddress, ScMemoryContext scExtContent)
            : base(arcAddress, scExtContent)
        {
        }

        /// <summary>
        ///     Возвращает начальный элемент дуги
        /// </summary>
        /// <value>
        ///     Начальный элемент. Его можно приводить к необходимому типу.
        /// </value>
        public ScElement BeginElement
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("ScArc", DisposalExceptionMsg);
                }
                if (ScMemoryContext.IsMemoryInitialized() != true)
                {
                    throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
                }
                if (ScContext.PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ScContextInvalidException(ContextInvalidExceptionMsg);
                }

                beginElement = GetArcBeginElement();
                return beginElement;
            }
        }

        /// <summary>
        ///     Возарвщает конечный элемент дуги
        /// </summary>
        /// <value>
        ///     Конечный элемент. Его можно приводить к необходимому типу.
        /// </value>
        public ScElement EndElement
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("ScArc", DisposalExceptionMsg);
                }
                if (ScMemoryContext.IsMemoryInitialized() != true)
                {
                    throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
                }
                if (ScContext.PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ScContextInvalidException(ContextInvalidExceptionMsg);
                }

                endElement = GetArcEndElement();
                return endElement;
            }
        }

        private ScElement GetArcBeginElement()
        {
            WScAddress wScAddress;
            NativeMethods.sc_memory_get_arc_begin(ScContext.PtrScMemoryContext, ScAddress.WScAddress, out wScAddress);
            var scElement = ScContext.GetElement(new ScAddress(wScAddress));
            return scElement;
        }

        private ScElement GetArcEndElement()
        {
            WScAddress wScAddress;
            NativeMethods.sc_memory_get_arc_end(ScContext.PtrScMemoryContext, ScAddress.WScAddress, out wScAddress);
            var scElement = ScContext.GetElement(new ScAddress(wScAddress));
            return scElement;
        }
    }
}