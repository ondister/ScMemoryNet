namespace ScEngineNet.Native
{
    /// <summary>
    /// Перечислитель указывает направление поиска в потоке. Не используется.
    /// </summary>
    internal enum ScStreamSeekOrigin
    {
        SC_STREAM_SEEK_SET = 1,
        SC_STREAM_SEEK_CUR,
        SC_STREAM_SEEK_END
    }
}
