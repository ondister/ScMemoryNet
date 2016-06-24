namespace ScEngineNet.NativeElements
{
    internal enum ScStreamFlag : byte
    {
        //! Data can be read from stream
        SC_STREAM_FLAG_READ = 1,
        //! Data can be written into stream
        SC_STREAM_FLAG_WRITE = 2,
        //! Stream opened for appending (compatible just with SC_STREAM_WRITE flag)
        SC_STREAM_FLAG_APPEND = 4,
        //! Seek support (SEEK_SET, SEEK_CUR, SEEK_END)
        SC_STREAM_FLAG_SEEK = 8,
        //! Tell support (returns current position)
        SC_STREAM_FLAG_TELL = 16
    }
}
