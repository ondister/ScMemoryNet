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
        //_SC_EXTERN void sc_memory_params_clear(sc_memory_params*params);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern void sc_memory_params_clear(WScMemoryParams sc_memory_params);

        //_SC_EXTERN sc_memory_context* sc_memory_initialize(const sc_memory_params *params);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr sc_memory_initialize(WScMemoryParams sc_memory_params);

#warning realize
        //_SC_EXTERN sc_result sc_memory_init_ext(sc_char const * ext_path, const sc_char** enabled_list);
        //_SC_EXTERN void sc_memory_shutdown_ext();
        //_SC_EXTERN void sc_memory_context_pending_begin(sc_memory_context * ctx);
        //_SC_EXTERN void sc_memory_context_pending_end(sc_memory_context * ctx);
        //_SC_EXTERN sc_addr sc_memory_link_new2(sc_memory_context const * ctx, sc_bool is_const);

        //_SC_EXTERN void sc_memory_shutdown(sc_bool save_state);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_memory_shutdown(bool saveState);

        //_SC_EXTERN sc_memory_context* sc_memory_context_new(sc_uint8 levels);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_memory_context_new(byte levels);

        //_SC_EXTERN void sc_memory_context_free(sc_memory_context *ctx);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern void sc_memory_context_free(IntPtr context);

        //_SC_EXTERN sc_bool sc_memory_is_initialized();
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_memory_is_initialized();

        //_SC_EXTERN sc_bool sc_memory_is_element(sc_memory_context const * ctx, sc_addr addr);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_memory_is_element(IntPtr context,  WScAddress scAddress);

        //_SC_EXTERN sc_result sc_memory_element_free(sc_memory_context * ctx, sc_addr addr);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_element_free(IntPtr context,  WScAddress scAddress);

        //_SC_EXTERN sc_addr sc_memory_node_new(sc_memory_context const * ctx, sc_type type);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_memory_node_new(IntPtr context, ElementTypes elementType);

        //_SC_EXTERN sc_addr sc_memory_link_new(sc_memory_context const * ctx);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_memory_link_new(IntPtr context);

        //_SC_EXTERN sc_addr sc_memory_arc_new(sc_memory_context * ctx, sc_type type, sc_addr beg, sc_addr end);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_memory_arc_new(IntPtr context, ElementTypes elementType,  WScAddress beginElement,  WScAddress endElement);

        //_SC_EXTERN sc_result sc_memory_get_element_type(sc_memory_context const * ctx, sc_addr addr, sc_type * result);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_element_type(IntPtr context,  WScAddress scAddress, out ElementTypes elementType);


        //_SC_EXTERN sc_result sc_memory_change_element_subtype(sc_memory_context const * ctx, sc_addr addr, sc_type type);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_change_element_subtype(IntPtr context,  WScAddress scAddress, ElementTypes elementType);

        //_SC_EXTERN sc_result sc_memory_get_arc_begin(sc_memory_context const * ctx, sc_addr addr, sc_addr * result);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_arc_begin(IntPtr context, WScAddress scAddress, out WScAddress returnAddress);

        //_SC_EXTERN sc_result sc_memory_get_arc_end(sc_memory_context const * ctx, sc_addr addr, sc_addr * result);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_arc_end(IntPtr context,  WScAddress scAddress, out WScAddress returnAddress);

        //_SC_EXTERN sc_result sc_memory_get_arc_info(sc_memory_context const * ctx, sc_addr addr, sc_addr* result_start_addr, sc_addr * result_end_addr);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_arc_info(IntPtr context,WScAddress arcAddress,out WScAddress startAddress, out WScAddress endAddress);

        //_SC_EXTERN sc_result sc_memory_set_link_content(sc_memory_context * ctx, sc_addr addr, sc_stream const *stream);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_set_link_content(IntPtr context,  WScAddress scAddress, IntPtr stream);

        //_SC_EXTERN sc_result sc_memory_get_link_content(sc_memory_context const * ctx, sc_addr addr, sc_stream **stream);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_link_content(IntPtr context,  WScAddress scAddress, out IntPtr stream);

        //   sc_result sc_memory_find_links_with_content(sc_memory_context const * ctx, sc_stream const * stream, sc_addr **result, sc_uint32 *result_count);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_find_links_with_content(IntPtr context, IntPtr stream, out IntPtr resultAddresses, out uint resultCount);

        //Free buffer allocated for links content find result
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern void sc_memory_free_buff(IntPtr buffer);

        //_SC_EXTERN sc_result sc_memory_set_element_access_levels(sc_memory_context const * ctx, sc_addr addr, sc_access_levels access_levels, sc_access_levels * new_value);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_set_element_access_levels(IntPtr context,  WScAddress address, byte accessLevels, out byte installedAccessLevels);

        //_SC_EXTERN sc_result sc_memory_get_element_access_levels(sc_memory_context const * ctx, sc_addr addr, sc_access_levels * result);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_get_element_access_levels(IntPtr context,  WScAddress address, out byte installedAccessLevels);

        //_SC_EXTERN sc_result sc_memory_stat(sc_memory_context const * ctx, sc_stat *stat);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_memory_stat(IntPtr context, out ScStat statistics);

        //_SC_EXTERN sc_result sc_memory_save(sc_memory_context const * ctx);
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
