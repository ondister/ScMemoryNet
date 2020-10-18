namespace ScEngineNet.ScElements
{
    /// <summary>
    /// Результат выполнения нативной функции.
    /// </summary>
    public enum ScResult
    {
        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        ScResultError = 0, // unknown error

        /// <summary>
        /// Успешное выполнение без всяких ошибок.
        /// </summary>
        ScResultOk = 1, // no any error

        /// <summary>
        /// Неправильные параметры функции.
        /// </summary>
        ScResultErrorInvalidParams = 3, // invalid function parameters error

        /// <summary>
        /// Неправильно указан тип элемента.
        /// </summary>
        ScResultErrorInvalidType = 5, // invalied type error

        /// <summary>
        /// Ошибка ввода вывода.
        /// </summary>
        ScResultErrorIo = 7, // input/output error

        /// <summary>
        /// Неверное состояние обрабатываемого объекта.
        /// </summary>
        ScResultErrorInvalidState = 9, // invalid state of processed object

        /// <summary>
        /// Элемент не найден.
        /// </summary>
        ScResultErrorNotFound = 11, // item not found

        /// <summary>
        /// не достаточн прав для изменения или удаления элемента.
        /// </summary>
        ScResultErrorNoWriteRights = 2, // no ritghs to change or delete object

        /// <summary>
        /// Недостаточно прав для чтения элемента.
        /// </summary>
        ScResultErrorNoReadRights = 4, // no ritghs to read object

        /// <summary>
        /// Недостаточно прав.
        /// </summary>
        ScResultErrorNoRights = ScResultErrorNoWriteRights | ScResultErrorNoReadRights
    }
}
