namespace ScEngineNet.Events
{
    /// <summary>
    /// Тип события
    /// </summary>
    public enum ScEventType
    {
        /// <summary>
        /// Неизвестный тип события
        /// </summary>
        ScEventUnknown = -1,

        /// <summary>
        /// Добавление исходящей дуги
        /// </summary>
        ScEventAddOutputArc = 0,
        /// <summary>
        /// Добавление входящей дуги
        /// </summary>
        ScEventAddInputArc = 1,
        /// <summary>
        /// Удаление исходящей дуги
        /// </summary>
        ScEventRemoveOutputArc = 2,
        /// <summary>
        /// Удаление входящей дуги
        /// </summary>
        ScEventRemoveInputArc = 3,

        /// <summary>
        /// Удаление элемента.
        /// </summary>
        ScEventRemoveElement = 4,

        /// <summary>
        /// Изменение содержимого sc-ссылки
        /// </summary>
        ScEventContentChanged = 5
    }
}
