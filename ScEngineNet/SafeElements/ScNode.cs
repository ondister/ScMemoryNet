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
            get { return ScMemorySafeMethods.GetSystemIdentifier(base.scContext, this); }
            set { ScMemorySafeMethods.SetSystemIdentifier(base.scContext, this, value); }
        }


        internal ScNode(ScAddress nodeAddress, ScMemoryContext scContext)
            : base(nodeAddress, scContext)
        {
            mainIdentifiers = new MainIdentifiers(this);
        }

        public static readonly String InstancePreffix = "inst_";
    }
}
