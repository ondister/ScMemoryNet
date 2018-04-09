using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Содержимое sc-ссылки double
    /// </summary>
    public class ScDouble : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.NumericDouble; }
        }

        public override string ToString()
        {
            return Value.ToString(ScEngineNet.CultureInfo);
        }


        /// <summary>
        /// Возвращает значение ссылки. В данном случае double
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public double Value
        {
            get { return ScLinkContent.ToDouble(base.Bytes); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScDouble"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        internal ScDouble(byte[] bytes) :
            base(bytes)
        { }

        internal ScDouble(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScDouble"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScDouble(double value) :
            base(BitConverter.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="ScDouble"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScDouble(double value)
        {
            return new ScDouble(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScDouble"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator double(ScDouble value)
        {
            return value.Value;
        }

    }
}
