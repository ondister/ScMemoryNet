using ScEngineNet.ScElements;


namespace ScEngineNet.ExtensionsNet
{

    /// <summary>
    /// Интерфейс для .net расширения для памяти. Вы можете написать свое расширение, используя этот интерфейс.
    /// </summary>
    public interface IScExtensionNet
    {
        /// <summary>
        /// Имя .net расширения
        /// </summary>
        /// <value>
        /// Возвращает имя для .net расширения
        /// </value>
        string NetExtensionName { get; }
        /// <summary>
        /// Краткое описание .net расширения.
        /// </summary>
        /// <value>
        /// Возвращает описание .net расширения
        /// </value>
        string NetExtensionDescription { get; }
        /// <summary>
        /// Инициализирует .net расширение. При реализации создайте в этом методе новый <see cref="ScMemoryContext"/>
        /// </summary>
        /// <returns>Возвращает <see cref="ScResult"/></returns>
        ScResult Initialize();
        /// <summary>
        /// Закрывает .net расширение. При реализации вызовите метод Delete() экземпляра <see cref="ScMemoryContext"/>
        /// </summary>
        /// <returns>Возвращает <see cref="ScResult"/></returns>
        ScResult ShutDown();
    }
}
