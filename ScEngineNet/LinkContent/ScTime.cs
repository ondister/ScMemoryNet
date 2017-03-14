using System;
using System.Globalization;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    ///     Содержимое sc-ссылки компонент Date DateTime
    /// </summary>
    public class ScTime : ScLinkContent
    {
        internal ScTime(byte[] bytes) :
            base(bytes)
        {
        }

        internal ScTime(IntPtr stream) :
            base(stream)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScDate" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScTime(TimeSpan value) :
            base(ScEngineNet.TextEncoding.GetBytes(value.ToString()))
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
            get { return ScDataTypes.Instance.Time; }
        }

        /// <summary>
        ///     Возвращает значение ссылки. В данном случае DateTime
        /// </summary>
        /// <value>
        ///     Значение
        /// </value>
        public TimeSpan Value
        {
            get
            {
                var stringContent = ToString(Bytes);
                DateTime date;
                DateTime.TryParse(stringContent, ScEngineNet.CultureInfo, DateTimeStyles.None, out date);
                return date.TimeOfDay;
            }
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.DateTime" /> to <see cref="ScDate" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScTime(TimeSpan value)
        {
            return new ScTime(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="ScDate" /> to <see cref="System.DateTime" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator TimeSpan(ScTime value)
        {
            return value.Value;
        }
    }
}