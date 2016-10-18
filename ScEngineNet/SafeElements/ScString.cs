using ScEngineNet.NetHelpers;
using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Содержимое sc-ссылки string
    /// </summary>
    public class ScString : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.TypeString; }
        }
        /// <summary>
        /// Возвращает значение ссылки. В данном случае string
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public string Value
        {
            get { return ScLinkContent.ToString(base.Bytes); }
        }

        internal ScString(byte[] bytes) :
            base(bytes)
        { }

        internal ScString(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScString(string value) :
            base(ScEngineNet.TextEncoding.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ScString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScString(string value)
        {
            return new ScString(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ScString value)
        {
            return value.Value;
        }

    }
}
