using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Элемент sc-узел. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScNode : ScElement
    {
        private MainIdentifiers mainIdentifiers;

        /// <summary>
        /// Возвращает основные идентификаторы узла.
        /// </summary>
        /// <value>
        /// Коллекция основных идентификаторов.
        /// </value>
        public MainIdentifiers MainIdentifiers
        {
            get { return mainIdentifiers; }
        }
        /// <summary>
        /// Возвращает системный идентификатор узла
        /// </summary>
        /// <value>
        /// Системный идентификатор
        /// </value>
        public Identifier SystemIdentifier
        {
            get 
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScNode", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
                
                return ScMemorySafeMethods.GetSystemIdentifier(base.ScContext, this);
            }
            set 
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScNode", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
               
                ScMemorySafeMethods.SetSystemIdentifier(base.ScContext, this, value);
            }
        }


        internal ScNode(ScAddress nodeAddress, ScMemoryContext scContext)
            : base(nodeAddress, scContext)
        {
            mainIdentifiers = new MainIdentifiers(this);
        }

        /// <summary>
        /// Текстовая константа для узла. Используется при создании идентификаторов
        /// </summary>
        public static readonly String InstancePreffix = "inst_";
    }
}
