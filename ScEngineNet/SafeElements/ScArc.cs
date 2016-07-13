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
                this.beginElement = ScMemorySafeMethods.GetArcBeginElement(base.scContext, this);
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
                this.endElement = ScMemorySafeMethods.GetArcEndElement(base.scContext, this);
                return this.endElement;
            }
        }

        internal ScArc(ScAddress arcAddress, IntPtr scExtContent)
            : base(arcAddress, scExtContent)
        { }
    }
}
