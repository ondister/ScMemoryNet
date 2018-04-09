using System;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    ///     Содержимое sc-ссылки Bitmap
    /// </summary>
    public class ScBitmap : ScLinkContent
    {
        public ScBitmap(byte[] bytes) :
            base(bytes)
        {
        }

        internal ScBitmap(IntPtr stream) :
            base(stream)
        {
        }

       

        /// <summary>
        ///     Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        ///     Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.Bitmap; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }


        /// <summary>
        ///     Возвращает значение ссылки. В данном случае Bitmap
        /// </summary>
        /// <value>
        ///     Значение
        /// </value>
        public byte[] Value
        {
            get
            {
              return Bytes;
            }
        }


        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Drawing.Bitmap" /> to <see cref="ScBitmap" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScBitmap(byte[] value)
        {
            return new ScBitmap(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="ScBitmap" /> to <see cref="System.Drawing.Bitmap" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator byte[](ScBitmap value)
        {
            return value.Value;
        }
    }
}