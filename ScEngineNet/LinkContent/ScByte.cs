using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Содержимое sc-ссылки byte
    /// </summary>
    public class ScByte : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.NumericByte; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }


        /// <summary>
        /// Возвращает значение ссылки. В данном случае byte
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public byte Value
        {
            get { return ScLinkContent.ToByte(base.Bytes); }
        }

        internal ScByte(byte[] bytes) :
            base(bytes)
        { }

        internal ScByte(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScByte"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScByte(byte value) :
            base(BitConverter.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="ScByte"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScByte(byte value)
        {
            return new ScByte(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScByte"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte(ScByte value)
        {
            return value.Value;
        }

    }
}
