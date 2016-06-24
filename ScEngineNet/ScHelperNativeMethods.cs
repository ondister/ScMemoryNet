using System;
using System.Runtime.InteropServices;

using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;
namespace ScEngineNet
{
    internal static partial class NativeMethods
    {
        //_SC_EXTERN sc_result sc_helper_find_element_by_system_identifier(sc_memory_context const * ctx, const sc_char* data, sc_uint32 len, sc_addr *result_addr);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_helper_find_element_by_system_identifier(IntPtr context, byte[] data, uint dataLenght, out WScAddress address);
        
        //_SC_EXTERN sc_result sc_helper_set_system_identifier(sc_memory_context const * ctx, sc_addr addr, const sc_char* data, sc_uint32 len);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_helper_set_system_identifier(IntPtr context, WScAddress address, byte[] data, uint dataLenght);

        //_SC_EXTERN sc_result sc_helper_get_system_identifier_link(sc_memory_context const * ctx, sc_addr el, sc_addr *sys_idtf_addr);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_helper_get_system_identifier_link(IntPtr context,  WScAddress elementAddress, out WScAddress linkAddress);

        //_SC_EXTERN sc_result sc_helper_get_keynode(sc_memory_context const * ctx, sc_keynode keynode, sc_addr *keynode_addr);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_helper_get_keynode(IntPtr context, KeyNode keyNode, out WScAddress keyNodeAddress);

        //_SC_EXTERN sc_bool sc_helper_resolve_system_identifier(sc_memory_context const * ctx, const char *system_idtf, sc_addr *result);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_helper_resolve_system_identifier(IntPtr context, byte[] systemIdentifier, out WScAddress keyNodeAddress);

        //_SC_EXTERN sc_bool sc_helper_check_arc(sc_memory_context const * ctx, sc_addr beg_el, sc_addr end_el, sc_type arc_type);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_helper_check_arc(IntPtr context,  WScAddress beginAddress,  WScAddress endAddress, ElementType arcType);


        //_SC_EXTERN sc_bool sc_helper_check_version_equal(sc_uint8 major, sc_uint8 minor, sc_uint8 patch);
        //на момент написания версия была 0.2.0
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_helper_check_version_equal(byte major,byte minor,byte path);
    }
}
