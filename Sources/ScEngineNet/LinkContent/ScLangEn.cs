using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Содержимое sc-ссылки string
    /// </summary>
    public class ScLangEn: ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.LanguageEn; }
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

        internal ScLangEn(byte[] bytes) :
            base(bytes)
        { }

        internal ScLangEn(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScLangEn(string value) :
            base(ScEngineNet.TextEncoding.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ScString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLangEn(string value)
        {
            return new ScLangEn(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ScLangEn value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
