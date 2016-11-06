namespace ScEngineNet.Native
{
    /// <summary>
    /// Имплементация адреса элемента из нативной библиотеки. Используется только с нативными методами.
    /// </summary>
    internal struct WScAddress
    {
        /// <summary>
        /// The segment
        /// </summary>
        internal ushort Segment;
        /// <summary>
        /// The offset
        /// </summary>
        internal ushort Offset;
    }
}
