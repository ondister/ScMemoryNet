using System.Collections.Generic;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Конструкция из 3-х или 5-ти элементов
    /// </summary>
   public  class ScConstruction
    {
        private readonly List<ScElement> elements;

        /// <summary>
        /// Возвращает коллекцию элементов
        /// </summary>
        /// <value>
        /// Коллекция элементов
        /// </value>
        public List<ScElement> Elements
        {
            get { return elements; }
        }

        internal ScConstruction()
        {
            elements = new List<ScElement>();
        }
    }
}
