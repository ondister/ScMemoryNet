using System;
using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Элемент sc-узел. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScNode : ScElement
    {
        /// <summary>
        ///     Текстовая константа для узла. Используется при создании идентификаторов
        /// </summary>
        public static readonly String InstancePreffix = "inst_";

        internal ScNode(ScAddress nodeAddress, ScMemoryContext scContext)
            : base(nodeAddress, scContext)
        {
            MainIdentifiers = new MainIdentifiers(this);
        }

        /// <summary>
        ///     Возвращает основные идентификаторы узла.
        /// </summary>
        /// <value>
        ///     Коллекция основных идентификаторов.
        /// </value>
        public MainIdentifiers MainIdentifiers { get; private set; }

        /// <summary>
        ///     Возвращает системный идентификатор узла
        /// </summary>
        /// <value>
        ///     Системный идентификатор
        /// </value>
        public Identifier SystemIdentifier
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("ScNode", DisposalExceptionMsg);
                }
                if (ScMemoryContext.IsMemoryInitialized() != true)
                {
                    throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
                }
                if (ScContext.PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ScContextInvalidException(ContextInvalidExceptionMsg);
                }

                return GetSystemIdentifier();
            }
            set
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("ScNode", DisposalExceptionMsg);
                }
                if (ScMemoryContext.IsMemoryInitialized() != true)
                {
                    throw new ScMemoryNotInitializeException(MemoryNotInitializedExceptionMsg);
                }
                if (ScContext.PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ScContextInvalidException(ContextInvalidExceptionMsg);
                }

                SetSystemIdentifier(value);
            }
        }

        private Identifier GetSystemIdentifier()
        {
            var identifier = Identifier.Invalid;
            WScAddress linkAddress;
            if (
                NativeMethods.sc_helper_get_system_identifier_link(ScContext.PtrScMemoryContext, ScAddress.WScAddress,
                    out linkAddress) == ScResult.ScResultOk)
            {
                identifier =
                    ScLinkContent.ToString(
                        ScContext.GetLinkContent(new ScLink(new ScAddress(linkAddress), ScContext)).Bytes);
            }
            return identifier;
        }

        private void SetSystemIdentifier(Identifier identifier)
        {
            var bytes = identifier.GetBytes();
            NativeMethods.sc_helper_set_system_identifier(ScContext.PtrScMemoryContext, ScAddress.WScAddress,
                bytes, (uint) bytes.Length);
        }
    }
}