namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Уровень доступа для  <see cref="ScMemoryContext"/> 
    /// Добавьте сюда необходимый уровень, если очень хочется
    /// </summary>
    public enum ScAccessLevels : byte
    {
        /// <summary>
        /// The minimum level
        /// </summary>
        MinLevel = 0,
        /// <summary>
        /// The medium level
        /// </summary>
        MedLevel = 128,
        /// <summary>
        /// The maximum level
        /// </summary>
        MaxLevel = 255
    }
}
