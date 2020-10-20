using System;
using System.Runtime.InteropServices;
using ScEngineNet.ScElements;

namespace ScEngineNet.Native
{
    internal static partial class NativeMethods
    {
#warning rea;ize
        //_SC_EXTERN sc_result sc_stream_write_data(const sc_stream* stream, sc_char *data, sc_uint32 data_len, sc_uint32 *written_bytes);
        //_SC_EXTERN sc_result sc_stream_seek(const sc_stream *stream, sc_stream_seek_origin seek_origin, sc_uint32 offset);
        //_SC_EXTERN sc_bool sc_stream_eof(const sc_stream* stream);
        //_SC_EXTERN sc_result sc_stream_get_position(const sc_stream *stream, sc_uint32 *position);
        //_SC_EXTERN sc_bool sc_stream_check_flag(const sc_stream *stream, sc_uint8 flag);


        //sc_stream* sc_stream_memory_new(const sc_char *buffer, sc_uint buffer_size, sc_uint8 flags, sc_bool data_owner);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_stream_memory_new(byte[] buffer, uint bufferSize, ScStreamFlag flags, bool dataOwner);

        //sc_result sc_stream_read_data(const sc_stream *stream, sc_char *data, sc_uint32 data_len, sc_uint32 *read_bytes)
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_stream_read_data(IntPtr stream, byte[] buffer, uint dataSize, out uint readBytes);

        //sc_result sc_stream_get_length(const sc_stream *stream, sc_uint32 *length)
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_stream_get_length(IntPtr stream, out uint readBytes);

        // _SC_EXTERN sc_result sc_stream_free(sc_stream *stream);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_stream_free(IntPtr stream);

        /*! Create file data stream
        * @param file_name Path to file for streaming
        * @param flags Data stream flags
        * @remarks Allocate and create file data stream. The returned stream pointer should be freed
        * with sc_stream_free function, when done using it.
        * @return Returns stream pointer if the stream was successfully created, or NULL if an error occurred
        */
        //_SC_EXTERN sc_stream* sc_stream_file_new(const sc_char *file_name, sc_uint8 flags);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_stream_file_new(string fileName, ScStreamFlag flags);
    }
}
