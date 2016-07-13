using System.Runtime.InteropServices;
using ScEngineNet.SafeElements;

namespace ScEngineNet.NativeElements
{
    /// <summary>
    /// Универсальный параметр итератора.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = ScEngineNet.DefaultCharset)]
    internal struct ScIteratorParam
    {
        /// <summary>
        /// Если параметр не элемент, а просто указывается тип, то необходимо подставить True, если параметр известный элемент, то False.
        /// </summary>
        [FieldOffset(0)]
        internal bool IsType;
        /// <summary>
        /// Указывается адрес, если параметр известный элемент
        /// </summary>
        [FieldOffset(4)]
        internal WScAddress Address;
        /// <summary>
        /// Указывается тип, если параметр неизвестный элемент.
        /// </summary>
        [FieldOffset(4)]
        internal ElementType Type;
    }
}
