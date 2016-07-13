namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Тип события
    /// </summary>
    public enum ScEventType
    {
        /// <summary>
        /// Неизвестный тип события
        /// </summary>
        SC_EVENT_UNKNOWN = -1,

        /// <summary>
        /// Добавление исходящей дуги
        /// </summary>
        SC_EVENT_ADD_OUTPUT_ARC = 0,
        /// <summary>
        /// Добавление входящей дуги
        /// </summary>
        SC_EVENT_ADD_INPUT_ARC = 1,
        /// <summary>
        /// Удаление исходящей дуги
        /// </summary>
        SC_EVENT_REMOVE_OUTPUT_ARC = 2,
        /// <summary>
        /// Удаление входящей дуги
        /// </summary>
        SC_EVENT_REMOVE_INPUT_ARC = 3,

        /// <summary>
        /// Удаление элемента.
        /// </summary>
        SC_EVENT_REMOVE_ELEMENT = 4,

        /// <summary>
        /// Изменение содержимого sc-ссылки
        /// </summary>
        SC_EVENT_CONTENT_CHANGED = 5
    }
}
