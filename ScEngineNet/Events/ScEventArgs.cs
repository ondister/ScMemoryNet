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
        public ScEventType EventType { get; private set; }

        /// <summary>
        /// Возвращает элемент, подписанный на событие
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public ScElement Element { get; private set; }


        /// <summary>
        /// Возвращает элемент, на другом конце дуги
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public ScElement OtherElement { get; private set; }

        /// <summary>
        /// Возвращает входящую или исходящую дугу, если событие было подписано на добавление или удаление дуг.
        /// </summary>
        /// <value>
        /// Sc-дуга
        /// </value>
        public ScArc Arc { get; private set; }

        internal ScEventArgs(ScEventType eventType, ScElement element, ScArc arc, ScElement otherElement)
        {
            EventType = eventType;
            Element = element;
            Arc = arc;
            OtherElement = otherElement;
        }
    }
}
