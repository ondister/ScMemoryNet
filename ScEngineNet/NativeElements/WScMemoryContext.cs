using System.Runtime.InteropServices;

namespace ScEngineNet.NativeElements
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WScMemoryContext
    {
        public ushort Id;
        public byte AccessLevels;
    }
}
