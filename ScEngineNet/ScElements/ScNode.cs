using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.ScExceptions;
using System;

namespace ScEngineNet.ScElements
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
                
                return this.GetSystemIdentifier();
            }
            set 
            {
                if (this.Disposed == true) { throw new ObjectDisposedException("ScNode", disposalException_msg); }
                if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
                if (this.ScContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
               
                this.SetSystemIdentifier( value);
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

       private Identifier GetSystemIdentifier()
        {
            Identifier identifier = Identifier.Invalid;
            WScAddress linkAddress;
            if (NativeMethods.sc_helper_get_system_identifier_link(base.ScContext.PtrScMemoryContext, this.ScAddress.WScAddress, out linkAddress) == ScResult.SC_RESULT_OK)
            {
                identifier = ScLinkContent.ToString(base.ScContext.GetLinkContent(new ScLink(new ScAddress(linkAddress), base.ScContext)).Bytes);
            }
            return identifier;
        }

        private ScResult SetSystemIdentifier( Identifier identifier)
        {
            var result = ScResult.SC_RESULT_ERROR;
            byte[] bytes = identifier.GetBytes();
            result = NativeMethods.sc_helper_set_system_identifier(base.ScContext.PtrScMemoryContext, this.ScAddress.WScAddress, bytes, (uint)bytes.Length);
            return result;
        }
    }
}
