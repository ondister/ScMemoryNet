using System;
using System.Globalization;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    ///     Содержимое sc-ссылки  TimeSpan 
    /// </summary>
    public class ScTimeInterval : ScLinkContent
    {
        internal ScTimeInterval(byte[] bytes) :
            base(bytes)
        {
        }

        internal ScTimeInterval(IntPtr stream) :
            base(stream)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScDate" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ScTimeInterval(TimeSpan value) :
            base(BitConverter.GetBytes(value.Ticks))
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
            get { return ScDataTypes.Instance.TimeInterval; }
        }

        public override string ToString()
        {
            return Value.ToString();
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
                var ticks = ToLong(Bytes);
                var timeSpan= new TimeSpan(ticks);
               
                return timeSpan;
            }
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.DateTime" /> to <see cref="ScDate" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScTimeInterval(TimeSpan value)
        {
            return new ScTimeInterval(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="ScDate" /> to <see cref="System.DateTime" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator TimeSpan(ScTimeInterval value)
        {
            return value.Value;
        }
    }
}