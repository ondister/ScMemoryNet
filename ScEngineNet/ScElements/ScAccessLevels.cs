﻿namespace ScEngineNet.ScElements
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
        MedLevel = 6,
        /// <summary>
        /// The maximum level
        /// </summary>
        MaxLevel = 15
    }
}
