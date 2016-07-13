using System.Runtime.InteropServices;

namespace ScEngineNet.NativeElements
{
    /// <summary>
    /// Имплементация виртуальной памяти. Практически не используется, вместо нее используется указатели.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WScMemoryContext
    {
        /// <summary>
        /// The identifier
        /// </summary>
        internal ushort Id;
        /// <summary>
        /// The access levels
        /// </summary>
        internal byte AccessLevels;
    }
}
