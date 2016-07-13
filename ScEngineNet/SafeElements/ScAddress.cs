using System;

using ScEngineNet.NativeElements;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Адрес SC-элемента в памяти.
    /// </summary>
    public class ScAddress : IEquatable<ScAddress>
    {
        /// <summary>
        /// Возвращает неизвестный  Sc адрес
        /// </summary>
        public static readonly ScAddress Invalid = new ScAddress(0, 0);

        private readonly WScAddress wScAddress;

        internal WScAddress WScAddress
        {
            get { return wScAddress; }
        }

        /// <summary>
        /// Возвращает известность адреса
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !this.Equals(ScAddress.Invalid);
            }
        }

        /// <summary>
        /// Сегмент.
        /// </summary>
        public ushort Segment
        { get { return segment; } }

        /// <summary>
        /// Смещение.
        /// </summary>
        public ushort Offset
        { get { return offset; } }

        private readonly ushort segment;
        private readonly ushort offset;

        /// <summary>
        /// Инициализирует новый sc-адрес, используя смещение и сегмент.
        /// </summary>
        /// <param name="segment">сегмент</param>
        /// <param name="offset">смещение</param>
        public ScAddress(ushort segment, ushort offset)
        {
            this.segment = segment;
            this.offset = offset;
            this.wScAddress = new WScAddress() { Offset = offset, Segment = segment };
        }

        internal ScAddress(WScAddress wScAddress)
        {
            this.segment = wScAddress.Segment;
            this.offset = wScAddress.Offset;
            this.wScAddress = wScAddress;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("segment: {0}, offset: {1}", segment, offset);
        }

        #region Реализация сравнения

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="ScAddress"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="ScAddress"/></param>
        public bool Equals(ScAddress obj)
        {
            if (obj == null)
                return false;

            return obj.Offset == this.Offset && obj.Segment == this.Segment;
        }

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="T:System.Object"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object"/></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ScAddress scAddress = obj as ScAddress;
            if (scAddress as ScAddress == null)
                return false;
            return scAddress.Offset == this.Offset && scAddress.Segment == this.Segment;
        }

        /// <summary>
        /// Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.Offset.ToString() + this.Segment.ToString());
        }

        /// <summary>
        /// Оператор сравнения адресов
        /// </summary>
        /// <param name="scAddress1">Первый адрес</param>
        /// <param name="scAddress2">Второй адрес</param>
        /// <returns>Возвращает True, если адреса равны</returns>
        public static bool operator ==(ScAddress scAddress1, ScAddress scAddress2)
        {
            bool isEqual = false;
            if (((object)scAddress1 != null) && ((object)scAddress2 != null))
            {
                isEqual = scAddress1.Equals(scAddress2);
            }
            return isEqual;
        }

        /// <summary>
        /// Оператор сравнения адресов
        /// </summary>
        /// <param name="scAddress1">Первый адрес</param>
        /// <param name="scAddress2">Второй адрес</param>
        /// <returns>Возвращает True, если адреса равны</returns>
        public static bool operator !=(ScAddress scAddress1, ScAddress scAddress2)
        {
            return !(scAddress1 == scAddress2);
        }

        #endregion
    }
}
