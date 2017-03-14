using System.Runtime.InteropServices;
using ScEngineNet.ScElements;

namespace ScEngineNet.Native
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
        internal ElementTypes Type;
    }
}
