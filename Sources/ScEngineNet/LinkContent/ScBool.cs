using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    /// Логическое содержимое sc-ссылки
    /// </summary>
    public class ScBool : ScLinkContent
    {

        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public override Identifier ClassNodeIdentifier
        {
            get { return ScDataTypes.Instance.TypeBool; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }


        /// <summary>
        /// Возвращает значение ссылки. В данном случае или True или False
        /// </summary>
        /// <value>
        ///Значение
        /// </value>
        public bool Value
        {
            get { return ToBool(base.Bytes); }
        }

        internal ScBool(byte[] bytes) :
            base(bytes)
        { }

        internal ScBool(IntPtr stream) :
            base(stream)
        { }

        public static bool True
        {
            get { return new ScBool(true); }
        }

        public static bool False
        {
            get { return new ScBool(false); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScBool"/> class.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public ScBool(bool value) :
            base(BitConverter.GetBytes(value))
        { }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Boolean"/> to <see cref="ScBool"/>.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScBool(bool value)
        {
            return new ScBool(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ScBool"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator bool(ScBool value)
        {
            return value.Value;
        }

        
    }
}
