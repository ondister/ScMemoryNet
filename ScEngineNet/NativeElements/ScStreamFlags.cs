namespace ScEngineNet.NativeElements
{
    /// <summary>
    /// Флаги для потока чтения-записи
    /// </summary>
    internal enum ScStreamFlag : byte
    {
        
        /// <summary>
        /// Data can be read from stream
        /// </summary>
        SC_STREAM_FLAG_READ = 1,
        
        /// <summary>
        /// Data can be written into stream
        /// </summary>
        SC_STREAM_FLAG_WRITE = 2,
        
        /// <summary>
        /// Stream opened for appending (compatible just with SC_STREAM_WRITE flag)
        /// </summary>
        SC_STREAM_FLAG_APPEND = 4,
       
        /// <summary>
        ///Seek support (SEEK_SET, SEEK_CUR, SEEK_END)
        /// </summary>
        SC_STREAM_FLAG_SEEK = 8,
        
        /// <summary>
        ///Tell support (returns current position)
        /// </summary>
        SC_STREAM_FLAG_TELL = 16
    }
}
