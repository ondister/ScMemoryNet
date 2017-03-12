using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.Events
{
    /// <summary>
    /// Аргумент sc-события
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ScEventArgs : EventArgs
    {
        /// <summary>
        /// Возвращает тип события
        /// </summary>
        /// <value>
        /// Тип события <see cref="ScEventType"/>
        /// </value>
        public ScEventType EventType { get; }

        /// <summary>
        /// Возвращает элемент, подписанный на событие
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public ScElement Element { get; }

        /// <summary>
        /// Возвращает входящую или исходящую дугу, если событие было подписано на добавление или удаление дуг.
        /// </summary>
        /// <value>
        /// Sc-дуга
        /// </value>
        public ScArc Arc { get; }

        internal ScEventArgs(ScEventType eventType, ScElement element, ScArc arc)
        {
            EventType = eventType;
            Element = element;
           Arc = arc;
        }
    }
}
