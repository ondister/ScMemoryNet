using System;
using System.Runtime.InteropServices;

namespace ScEngineNet.NativeElements
{
    [StructLayout(LayoutKind.Sequential, CharSet = ScEngineNet.DefaultCharset)]
    internal struct ScIterator3
    {
        internal ScIterator3Type IteratorType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        internal ScIteratorParam[] IteratorParams;
        internal IntPtr Context;
        internal bool Finished;

    }
}
