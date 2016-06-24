using System;
using System.Runtime.InteropServices;

namespace ScEngineNet.NativeElements
{
    [StructLayout(LayoutKind.Sequential, CharSet = ScEngineNet.DefaultCharset)]
    internal struct ScIterator5
    {
        internal ScIterator5Type IteratorType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        internal ScIteratorParam[] IteratorParams;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        internal WScAddress[] Results;
        internal IntPtr IteratorMain;
        internal IntPtr IteratorAttr;
        internal uint TimeStamp;
        internal IntPtr Context;

    }
}
