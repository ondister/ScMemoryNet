using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Содержимое sc-ссылки Int32
    /// </summary>
    public class ScInt32 : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.NumericInt; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }


        /// <summary>
        /// Возвращает значение ссылки. В данном случае Int32
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public int Value
        {
            get { return ScLinkContent.ToInt32(base.Bytes); }
        }

        internal ScInt32(byte[] bytes) :
            base(bytes)
        { }

        internal ScInt32(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScInt32"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScInt32(int value) :
            base(BitConverter.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ScInt32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScInt32(int value)
        {
            return new ScInt32(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScInt32"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(ScInt32 value)
        {
            return value.Value;
        }

    }
}
