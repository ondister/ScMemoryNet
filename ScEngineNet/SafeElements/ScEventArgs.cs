using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Аргумент sc-события
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ScEventArgs : EventArgs
    {
        private readonly ScEventType eventType;
        private readonly ScElement element;
        private readonly ScArc arc;

        /// <summary>
        /// Возвращает тип события
        /// </summary>
        /// <value>
        /// Тип события <see cref="ScEventType"/>
        /// </value>
        public ScEventType EventType
        {
            get { return eventType; }
        }

        /// <summary>
        /// Возвращает элемент, подписанный на событие
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public ScElement Element
        {
            get { return this.element; }
        }

        /// <summary>
        /// Возвращает входящую или исходящую дугу, если событие было подписано на добавление или удаление дуг.
        /// </summary>
        /// <value>
        /// Sc-дуга
        /// </value>
        public ScArc Arc
        {
            get { return this.arc; }
        }

        internal ScEventArgs(ScEventType eventType, ScElement element, ScArc arc)
        {
            this.eventType = eventType;
            this.element = element;
            this.arc = arc;
        }
    }
}
