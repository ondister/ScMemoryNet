using System;

namespace ScEngineNet.ScExceptions
{
    /// <summary>
    /// Исключение при неинициализированной памяти
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ScMemoryNotInitializeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScMemoryNotInitializeException"/> class.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку.</param>
        public ScMemoryNotInitializeException(string message)
            : base(message)
        {
        }
    }
}
