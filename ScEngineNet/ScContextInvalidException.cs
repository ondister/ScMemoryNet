using System;

namespace ScEngineNet
{
    /// <summary>
    /// Исключение при неправильном значении контекста
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ScContextInvalidException:Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScContextInvalidException"/> class.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку.</param>
        public ScContextInvalidException(string message)
            :base(message)
        { 
        }
    }
}
