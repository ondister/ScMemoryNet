using System;
using System.Runtime.InteropServices;
using ScEngineNet.ScElements;
using ScEngineNet.Events;

namespace ScEngineNet.Native
{
    /// <summary>
    /// Имплементация события из нативной библиотеки. Используется только с нативными методами.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = ScEngineNet.DefaultCharset)]
    internal struct WScEvent
    {
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

        internal fEventCallbackEx CallbackEx;
        /// <summary>
        /// Функция обратного вызова при удалении элемента
        /// </summary>
        internal fDeleteCallback DeleteCallback;

        internal volatile uint ref_count;
        //! Context lock 
        internal volatile IntPtr thread_lock;
        //! Access levels
        internal byte access_levels;
    
    }

   
}
