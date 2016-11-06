using System.Collections.Generic;

namespace ScEngineNet.ScElements
{
    /// <summary>
    /// Конструкция из 3-х или 5-ти элементов
    /// </summary>
    public class ScConstruction
    {
        private readonly List<ScElement> elements;


        /// <summary>
        /// Возвращает  <see cref="ScElement"/> по указанному индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        public ScElement this[int index]
        {
            get { return this.elements[index]; }
        }

        internal void AddElement(ScElement element)
        {
            this.elements.Add(element);
        }

        internal void Clear()
        {
            this.elements.Clear();
        }

        internal ScConstruction()
        {
            this.elements = new List<ScElement>();
        }
    }
}
