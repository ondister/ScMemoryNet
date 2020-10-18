using System;
using System.Globalization;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    ///     Содержимое sc-ссылки компонент Date DateTime
    /// </summary>
    public class ScDate : ScLinkContent
    {
        internal ScDate(byte[] bytes) :
            base(bytes)
        {
        }

        internal ScDate(IntPtr stream) :
            base(stream)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScDate" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScDate(DateTime value) :
            base(ScEngineNet.TextEncoding.GetBytes(value.ToString(ScEngineNet.CultureInfo)))
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
            get { return ScDataTypes.Instance.Date; }
        }

        public override string ToString()
        {
            return Value.ToString(ScEngineNet.CultureInfo);
        }


        /// <summary>
        ///     Возвращает значение ссылки. В данном случае DateTime
        /// </summary>
        /// <value>
        ///     Значение
        /// </value>
        public DateTime Value
        {
            get
            {
                var stringContent = ToString(Bytes);
                DateTime date;
                DateTime.TryParse(stringContent, ScEngineNet.CultureInfo, DateTimeStyles.None, out date);
                return date.Date;
            }
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.DateTime" /> to <see cref="ScDate" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScDate(DateTime value)
        {
            return new ScDate(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="ScDate" /> to <see cref="System.DateTime" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator DateTime(ScDate value)
        {
            return value.Value;
        }

      

     }
}