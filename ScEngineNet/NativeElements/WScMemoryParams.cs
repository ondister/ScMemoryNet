using System.Runtime.InteropServices;

namespace ScEngineNet.NativeElements
{
    /// <summary>
    /// Имплементация параметров памяти при ее инициализации
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct WScMemoryParams
    {
        /// <summary>
        /// Путь к репозиторию
        /// </summary>
      
        internal string RepoPath;
        /// <summary>
        /// Путь к файлу конфигурации
        /// </summary>
      
        internal string ConfigFile;
        /// <summary>
        /// Путь к расширениям
        /// </summary>
       
        internal string ExtensionsPath;
        /// <summary>
        /// Очищать ли память перед инициализацией (стирает все содержимое памяти)
        /// </summary>
        internal bool Clear;
    }
}
