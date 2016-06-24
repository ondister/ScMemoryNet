using System.Runtime.InteropServices;
using ScEngineNet.SafeElements;

namespace ScEngineNet.NativeElements
{

    [StructLayout(LayoutKind.Explicit, CharSet = ScEngineNet.DefaultCharset)]
    internal struct ScIteratorParam
    {
        [FieldOffset(0)]
        internal bool IsType;
        [FieldOffset(4)]
        internal WScAddress Address;
        [FieldOffset(4)]
        internal ElementType Type;
       
    }
}
