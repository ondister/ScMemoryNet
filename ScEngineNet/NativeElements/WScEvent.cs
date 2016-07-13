using System;
using System.Runtime.InteropServices;

using ScEngineNet.SafeElements;

namespace ScEngineNet.NativeElements
{
    /// <summary>
    /// Имплементация события из нативной библиотеки. Используется только с нативными методами.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = ScEngineNet.DefaultCharset)]
    internal struct WScEvent
    {
        /// <summary>
        /// Содержимое памяти
        /// </summary>
        internal IntPtr ScMemoryContext;
        /// <summary>
        /// Адрес элемента, которые подписывается на событие
        /// </summary>
        internal WScAddress Element;
        /// <summary>
        /// Тип события
        /// </summary>
        internal ScEventType Type;
        /// <summary>
        /// Указатель на дополнительные данные (массив символов)
        /// </summary>
        internal IntPtr Data;
        /// <summary>
        /// Функция обратного вызова при произошедшем событии
        /// </summary>
        internal fEventCallback Callback;
        /// <summary>
        /// Функция обратного вызова при удалении элемента
        /// </summary>
        internal fDeleteCallback DeleteCallback;
    }
}
