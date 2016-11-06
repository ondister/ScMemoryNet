using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Бинарное содержимое sc-ссылки
    /// Этот тип содержимого можно использовать для хранения файлов внутри ссылки, или изображений
    /// </summary>
    public class ScBinary : ScLinkContent
    {

        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.TypeBinary; }
        }

        /// <summary>
        /// Получает значение. В данном случае массив байт
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public byte[] Value
        {
            get { return base.Bytes; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScBinary"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public ScBinary(byte[] bytes) :
            base(bytes)
        { }

        internal ScBinary(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="ScBinary"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScBinary(byte[] value)
        {
            return new ScBinary(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScBinary"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte[](ScBinary value)
        {
            return value.Value;
        }

    }
}
