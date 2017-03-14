using ScEngineNet.ScElements;
using System;
using System.Runtime.InteropServices;

namespace ScEngineNet.Native
{
    /// <summary>
    /// Нативные функции библиотеки sc-memory.dll
    /// </summary>
    internal static partial class NativeMethods
    {
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern void sc_memory_params_clear(WScMemoryParams sc_memory_params);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr sc_memory_initialize(WScMemoryParams sc_memory_params);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_memory_shutdown(bool saveState);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_memory_context_new(byte levels);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern void sc_memory_context_free(IntPtr context);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_memory_is_initialized();

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_memory_is_element(IntPtr context,  WScAddress scAddress);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_element_free(IntPtr context,  WScAddress scAddress);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_memory_node_new(IntPtr context, ElementTypes elementType);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_memory_link_new(IntPtr context);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_memory_arc_new(IntPtr context, ElementTypes elementType,  WScAddress beginElement,  WScAddress endElement);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_element_type(IntPtr context,  WScAddress scAddress, out ElementTypes elementType);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_change_element_subtype(IntPtr context,  WScAddress scAddress, ElementTypes elementType);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_arc_begin(IntPtr context, WScAddress scAddress, out WScAddress returnAddress);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_arc_end(IntPtr context,  WScAddress scAddress, out WScAddress returnAddress);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_arc_info(IntPtr context,WScAddress arcAddress,out WScAddress startAddress, out WScAddress endAddress);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_set_link_content(IntPtr context,  WScAddress scAddress, IntPtr stream);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_link_content(IntPtr context,  WScAddress scAddress, out IntPtr stream);

        //   sc_result sc_memory_find_links_with_content(sc_memory_context const * ctx, sc_stream const * stream, sc_addr **result, sc_uint32 *result_count);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_find_links_with_content(IntPtr context, IntPtr stream, out IntPtr resultAddresses, out uint resultCount);

        //Free buffer allocated for links content find result
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern void sc_memory_free_buff(IntPtr buffer);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_set_element_access_levels(IntPtr context,  WScAddress address, byte accessLevels, out byte installedAccessLevels);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_element_access_levels(IntPtr context,  WScAddress address, out byte installedAccessLevels);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_stat(IntPtr context, out ScStat statistics);

        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_save(IntPtr context);

        /// <summary>
        /// Converts an unmanaged array to a managed <see cref="Array"/>.
        /// </summary>
        /// <param name="structureType">The .NET <see cref="Type"/> that best represents
        /// the type of element stored in the unmanaged array. Only structs are supported.</param>
        /// <param name="arrayPtr">The address of the unmanaged array.</param>
        /// <param name="length">The number of elements in the unmanaged array.</param>
        /// <returns>An <see cref="Array"/> of <paramref name="structureType"/> where each
        /// index in the managed array contains an element copied from the unmanaged array.</returns>
        internal static Array PtrToArray(Type structureType, IntPtr arrayPtr, uint length)
        {
            if (structureType == null)
                throw new ArgumentNullException("Where is a structureType");
            if (!structureType.IsValueType)
                throw new ArgumentException("Only struct types are supported.", "structureType");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be equal to or greater than zero.");
            if (arrayPtr == IntPtr.Zero)
                return null;
            int size = Marshal.SizeOf(structureType);
            Array array = Array.CreateInstance(structureType, length);
            for (int i = 0; i < length; i++)
            {
                IntPtr offset = new IntPtr((long)arrayPtr + (size * i));
                object value = Marshal.PtrToStructure(offset, structureType);
                array.SetValue(value, i);
            }
            return array;
        }
    }
}
