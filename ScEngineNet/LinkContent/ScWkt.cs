using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Содержимое sc-ссылки well known text
    /// </summary>
    public class ScWkt: ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.Wkt; }
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

        internal ScWkt(byte[] bytes) :
            base(bytes)
        { }

        internal ScWkt(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScWkt(string value) :
            base(ScEngineNet.TextEncoding.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ScWkt"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScWkt(string value)
        {
            return new ScWkt(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScWkt"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ScWkt value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
