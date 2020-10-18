using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;
using System.Diagnostics;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Содержимое sc-ссылки string
    /// </summary>

    public class ScRtf : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.Rtf; }
        }

        public override string ToString()
        {
            return Value.ToString();
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

        internal ScRtf(byte[] bytes) :
            base(bytes)
        { }

        internal ScRtf(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScRtf(string value) :
            base(ScEngineNet.TextEncoding.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ScRtf"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScRtf(string value)
        {
            return new ScRtf(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScRtf"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ScRtf value)
        {
            return value.Value;
        }

    }
}
