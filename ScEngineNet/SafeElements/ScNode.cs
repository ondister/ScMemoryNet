using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Элемент sc-узел. Создается в классе <see cref="ScMemoryContext" />
    /// </summary>
    public class ScNode : ScElement, IDisposable
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


        internal ScNode(ScAddress nodeAddress, IntPtr scContext)
            : base(nodeAddress, scContext)
        {
            mainIdentifiers = new MainIdentifiers(this);
        }

        #region Члены IDisposable

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                //unmanaged
                this.mainIdentifiers.Dispose();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScNode"/> class.
        /// </summary>
        ~ScNode()
        {
            Dispose(false);
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
