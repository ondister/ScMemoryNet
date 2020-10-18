using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace ScEngineNet
{
    /// <summary>
    /// Информация о приложении.
    /// </summary>
    public static class ScEngineNet
    {
        /// <summary>
        /// Имя библиотеки по умолчанию
        /// </summary>
        internal const string ScMemoryDllName = "sc-memory.dll";

        /// <summary>
        /// Кодировка текста по умолчанию.
        /// </summary>
        internal static readonly Encoding TextEncoding = new UTF8Encoding();

        /// <summary>
        /// Региональные параметры
        /// </summary>
        internal static readonly CultureInfo CultureInfo= new CultureInfo("ru-Ru");

        /// <summary>
        /// Соглашение о вызове по умолчанию.
        /// </summary>
        internal const CallingConvention DefaultCallingConvention = CallingConvention.Cdecl;

        /// <summary>
        /// Кодировка по умолчанию.
        /// </summary>
        internal const CharSet DefaultCharset = CharSet.Unicode;


        /// <summary>
        /// Внутреннее имя библиотеки
        /// </summary>
        public static readonly string LibraryName = "Оболочка .Net для sc-memory";

    }
}
