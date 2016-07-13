using ScEngineNet.NetHelpers;
using System;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Содержимое ссылки, тип которого не определен.
    /// Фактически является <see cref="ScBinary"/>, однако не имеет конструкторов, и существует для того, чтобы определить, что  тип контента найти не удалось.
    /// </summary>
    public class ScObject : ScLinkContent
    {
        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override ScNode ClassNode
        {
            get { return DataTypes.Binary; }
        }
        /// <summary>
        /// Возвращает значение ссылки. В данном случае массив байт, как и в случае с ScBinary
        /// </summary>
        /// <value>
        /// Значение
        /// </value>
        public byte[] Value
        {
            get { return base.Bytes; }
        }

        internal ScObject(byte[] bytes) :
            base(bytes)
        { }

        internal ScObject(IntPtr Stream) :
            base(Stream)
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte[]" /> to <see cref="ScObject" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScObject(byte[] value)
        {
            return new ScObject(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScObject"/> to <see cref="System.Byte[]"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte[](ScObject value)
        {
            return value.Value;
        }

    }
}
