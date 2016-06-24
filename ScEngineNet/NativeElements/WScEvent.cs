using System;
using System.Runtime.InteropServices;

using ScEngineNet.SafeElements;

namespace ScEngineNet.NativeElements
{
    [StructLayout(LayoutKind.Sequential, CharSet = ScEngineNet.DefaultCharset)]
    internal struct WScEvent
    {
        internal IntPtr ScMemoryContent;
        internal WScAddress Element;
        internal ScEventType Type;
        internal IntPtr Data;
        internal fEventCallback Callback;
        internal fDeleteCallback DeleteCallback;
    }
}
