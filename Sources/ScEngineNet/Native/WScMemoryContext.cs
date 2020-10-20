using System.Runtime.InteropServices;

namespace ScEngineNet.Native
{
#warning realize
    //struct _sc_memory_context
    //{
    //    sc_uint32 id;
    //    sc_access_levels access_levels;
    //    sc_uint8 flags;
    //    GSList* pend_events;
    //};

    /// <summary>
    /// Имплементация виртуальной памяти. Практически не используется, вместо нее используется указатели.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WScMemoryContext
    {
        /// <summary>
        /// The identifier
        /// </summary>
        internal uint Id;
        /// <summary>
        /// The access levels
        /// </summary>
        internal byte AccessLevels;
    }
}
