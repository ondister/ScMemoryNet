namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Результат выполнения нативной функции.
    /// </summary>
    public enum ScResult
    {
        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        SC_RESULT_ERROR = 0, // unknown error

        /// <summary>
        /// Успешное выполнение без всяких ошибок.
        /// </summary>
        SC_RESULT_OK = 1, // no any error

        /// <summary>
        /// Неправильные параметры функции.
        /// </summary>
        SC_RESULT_ERROR_INVALID_PARAMS = 3, // invalid function parameters error

        /// <summary>
        /// Неправильно указан тип элемента.
        /// </summary>
        SC_RESULT_ERROR_INVALID_TYPE = 5, // invalied type error

        /// <summary>
        /// Ошибка ввода вывода.
        /// </summary>
        SC_RESULT_ERROR_IO = 7, // input/output error

        /// <summary>
        /// Неверное состояние обрабатываемого объекта.
        /// </summary>
        SC_RESULT_ERROR_INVALID_STATE = 9, // invalid state of processed object

        /// <summary>
        /// Элемент не найден.
        /// </summary>
        SC_RESULT_ERROR_NOT_FOUND = 11, // item not found

        /// <summary>
        /// не достаточн прав для изменения или удаления элемента.
        /// </summary>
        SC_RESULT_ERROR_NO_WRITE_RIGHTS = 2, // no ritghs to change or delete object

        /// <summary>
        /// Недостаточно прав для чтения элемента.
        /// </summary>
        SC_RESULT_ERROR_NO_READ_RIGHTS = 4, // no ritghs to read object

        /// <summary>
        /// Недостаточно прав.
        /// </summary>
        SC_RESULT_ERROR_NO_RIGHTS = SC_RESULT_ERROR_NO_WRITE_RIGHTS | SC_RESULT_ERROR_NO_READ_RIGHTS
    }
}
