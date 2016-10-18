using System;

namespace ScEngineNet
{
    /// <summary>
    /// Исключение при неинициализированной памяти
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ScMemoryNotInitializeException : Exception
    {
        public ScMemoryNotInitializeException(string message)
            : base(message)
        {
        }
    }
}
