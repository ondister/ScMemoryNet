using System;
using System.Runtime.InteropServices;

using ScEngineNet.ScElements;

namespace ScEngineNet.Native
{
    /// <summary>
    /// Поток для чтения и записи в память. Эта структура не используется, вместо нее используется указатель.
    /// Так же не используются и делегаты, имплементирующие указаели на функции.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = ScEngineNet.DefaultCharset)]
    internal struct ScStream
    {
        internal IntPtr Handler;
        internal ScStreamFlag Flags;
        internal fStreamRead ReadFunction;
        internal fStreamWrite WriteFunction;
        internal fStreamSeek SeekFunction;
        internal fStreamTell TellFunction;
        internal fStreamFreeHandler FreeFunction;
        internal fStreamEof EofFunction;
    }
    
    //  typedef sc_result (*fStreamRead)(const sc_stream *stream, sc_char *data, sc_uint32 length, sc_uint32 *bytes_read);
    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention,CharSet=ScEngineNet.DefaultCharset)]
    internal delegate ScResult fStreamRead(IntPtr stream, byte[] data, uint lenth, out uint bytesRead);

    //typedefsc_result (*fStreamWrite)(const sc_stream *stream, sc_char *data, sc_uint32 length, sc_uint32 *bytes_written);
    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    internal delegate ScResult fStreamWrite(IntPtr stream, byte[] data, uint lenth, out uint bytesWritten);

    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    //typedef sc_result (*fStreamSeek)(const sc_stream *stream, sc_stream_seek_origin origin, sc_uint32 offset);
    internal delegate ScResult fStreamSeek(IntPtr stream, ScStreamSeekOrigin origin, uint offset);

    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    //typedef sc_result (*fStreamTell)(const sc_stream *stream, sc_uint32 *position);
    internal delegate ScResult fStreamTell(IntPtr stream, out uint position);

    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    //sc_result (*fStreamFreeHandler)(const sc_stream *stream);
    internal delegate bool fStreamFreeHandler(IntPtr stream);

    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    //typedef sc_bool (*fStreamEof)(const sc_stream *stream);
    internal delegate bool fStreamEof(IntPtr stream);

  
}
