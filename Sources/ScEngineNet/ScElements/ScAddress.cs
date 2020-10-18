using System;
using ScEngineNet.Native;

namespace ScEngineNet.ScElements
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

        internal WScAddress WScAddress { get; private set; }

        /// <summary>
        /// Возвращает известность адреса
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !Equals(Invalid);
            }
        }

        /// <summary>
        /// Сегмент.
        /// </summary>
        public ushort Segment { get; private set; }

        /// <summary>
        /// Смещение.
        /// </summary>
        public ushort Offset { get; private set; }

        /// <summary>
        /// Инициализирует новый sc-адрес, используя смещение и сегмент.
        /// </summary>
        /// <param name="segment">сегмент</param>
        /// <param name="offset">смещение</param>
        public ScAddress(ushort segment, ushort offset)
        {
           Segment = segment;
           Offset = offset;
           WScAddress = new WScAddress() { Offset = offset, Segment = segment };
        }

        internal ScAddress(WScAddress wScAddress)
        {
            Segment = wScAddress.Segment;
            Offset = wScAddress.Offset;
            WScAddress = wScAddress;
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
            return string.Format("segment: {0}, offset: {1}", Segment, Offset);
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

            return obj.Offset == Offset && obj.Segment == Segment;
        }

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="T:System.Object"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object"/></param>
        public override bool Equals(object obj)
        {
            var scAddress = obj as ScAddress;
            if ((ScAddress) scAddress == null)
                return false;
            return scAddress.Offset == Offset && scAddress.Segment == Segment;
        }

        /// <summary>
        /// Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return Convert.ToInt32(Offset.ToString() + Segment.ToString());
        }

        /// <summary>
        /// Оператор сравнения адресов
        /// </summary>
        /// <param name="scAddress1">Первый адрес</param>
        /// <param name="scAddress2">Второй адрес</param>
        /// <returns>Возвращает True, если адреса равны</returns>
        public static bool operator ==(ScAddress scAddress1, ScAddress scAddress2)
        {
            var isEqual = false;
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
