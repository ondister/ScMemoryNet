using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    ///     Содержимое sc-ссылки Bitmap
    /// </summary>
    public class ScBitmap : ScLinkContent
    {
        internal ScBitmap(byte[] bytes) :
            base(bytes)
        {
        }

        internal ScBitmap(IntPtr stream) :
            base(stream)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScBitmap" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScBitmap(Bitmap value) :
            base(BitmapToBytes(value))
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

        /// <summary>
        ///     Возвращает значение ссылки. В данном случае Bitmap
        /// </summary>
        /// <value>
        ///     Значение
        /// </value>
        public Bitmap Value
        {
            get
            {
                var stream = new MemoryStream(Bytes);
                var bitmap = (Bitmap) Image.FromStream(stream);
                return bitmap;
            }
        }

        private static byte[] BitmapToBytes(Bitmap bitmap)
        {
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            var array = stream.ToArray();
            return array;
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Drawing.Bitmap" /> to <see cref="ScBitmap" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScBitmap(Bitmap value)
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
        public static implicit operator Bitmap(ScBitmap value)
        {
            return value.Value;
        }
    }
}