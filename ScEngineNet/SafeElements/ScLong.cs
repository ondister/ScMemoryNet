using ScEngineNet.NetHelpers;
using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Содержимое sc-ссылки long
    /// </summary>
    public class ScLong : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.NumericLong; }
        }
        /// <summary>
        /// Возвращает значение ссылки. В данном случае long
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public long Value
        {
            get { return ScLinkContent.ToLong(base.Bytes); }
        }

        internal ScLong(byte[] bytes) :
            base(bytes)
        { }

        internal ScLong(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScLong"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScLong(long value) :
            base(BitConverter.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="ScLong"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLong(long value)
        {
            return new ScLong(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScLong"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator long(ScLong value)
        {
            return value.Value;
        }

    }
}
