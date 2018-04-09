

namespace ScMemoryNet.Models.ScNetExtension
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
        /// Инициализирует .net расширение./>
        /// </summary>
        /// <returns>Возвращает true, если инициализация успешна</returns>
        bool Initialize();

        /// <summary>
        /// Закрывает .net расширение./>
        /// </summary>
        /// <returns>Возвращает true, если деинициализация успена </returns>
        bool ShutDown();
    }
}
