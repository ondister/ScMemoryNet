using System;
using System.Linq;

using ScEngineNet.NativeElements;

namespace ScEngineNet.SafeElements
{
    public class ScLinkContent : IEquatable<ScLinkContent>, IDisposable
    {
        private IntPtr scStream;
        private byte[] content;

        public byte[] Content
        {
            get { return content; }
        }

        internal IntPtr ScStream
        {
            get { return scStream; }
            set { this.SetStream(value); }
        }

        public ScLinkContent(byte[] Content)
        {
            this.content = Content;
            scStream = NativeMethods.sc_stream_memory_new(this.content, (uint)Content.Length, ScStreamFlag.SC_STREAM_FLAG_READ, false);
        }

        public ScLinkContent(string Content) :
            this(ScEngineNet.TextEncoding.GetBytes(Content))
        { }

        internal ScLinkContent(IntPtr Stream) :
            this(new byte[0])
        {
            this.SetStream(Stream);
        }

        internal ScLinkContent() :
            this(new byte[0])
        { }

        private void SetStream(IntPtr stream)
        {
            if (stream != IntPtr.Zero)
            {
                uint buffersize = 0;
                if (NativeMethods.sc_stream_get_length(stream, out buffersize) == ScResult.SC_RESULT_OK)
                {
                    var buffer = new byte[buffersize];
                    uint receivedBytes = 0;
                    if (NativeMethods.sc_stream_read_data(stream, buffer, buffersize, out receivedBytes) == ScResult.SC_RESULT_OK)
                    {
                        this.content = buffer;
                    }
                }
            }
        }

        /// <summary>
        /// Инициализирует новое содержимое ссылки из типа Double.
        /// </summary>
        /// <param name="value">значение</param>
        public ScLinkContent(double value)
            : this(BitConverter.GetBytes(value))
        { }

        /// <summary>
        /// Инициализирует новое содержимое ссылки из типа Int.
        /// </summary>
        /// <param name="value">значение</param>
        public ScLinkContent(int value)
            : this(BitConverter.GetBytes(value))
        { }

        #region Статические члены

        /// <summary>
        /// Возвращает <see cref="System.String" /> из массива байт
        /// </summary>
        /// <param name="data">Массив байт</param>
        /// <returns>
        /// A <see cref="System.String" /> Строка содержимого ссылки
        /// </returns>
        public static string ToString(byte[] data)
        {
            return ScEngineNet.TextEncoding.GetString(data);
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static int ToInt32(byte[] data)
        {
            int result;
            if (data.Length == 4)
            {
                result = BitConverter.ToInt32(data, 0);
            }
            else
            {
                string stringData = ScLinkContent.ToString(data);
                result = Int32.Parse(stringData);
            }
            return result;
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static double ToDouble(byte[] data)
        {
            double result = double.NaN;
            if (data.Length == 8)
            {
                result = BitConverter.ToDouble(data, 0);
            }
            else
            {
                string stringData = ScLinkContent.ToString(data);
                result = Double.Parse(stringData);
            }
            return result;
        }

        #endregion

        #region Реализация сравнения

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="ScLinkContent"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="ScLinkContent"/></param>
        public bool Equals(ScLinkContent obj)
        {
            return obj != null && obj.Content.SequenceEqual(this.Content);
        }

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="T:System.Object"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object"/></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ScLinkContent scLinkContent = obj as ScLinkContent;
            if (scLinkContent as ScLinkContent == null)
                return false;
            return scLinkContent.Content.SequenceEqual(this.Content);
        }

        /// <summary>
        /// Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.Content);
        }

        /// <summary>
        /// Оператор сравнения контента
        /// </summary>
        /// <param name="scLinkContent1">Первый  контент</param>
        /// <param name="scLinkContent2">Второй контент</param>
        /// <returns>Возвращает True, если контенты равны</returns>
        public static bool operator ==(ScLinkContent scLinkContent1, ScLinkContent scLinkContent2)
        {
            bool isEqual = false;
            if (((object)scLinkContent1 != null) && ((object)scLinkContent2 != null))
            {
                isEqual = scLinkContent1.Equals(scLinkContent2);
            }

            return isEqual;
        }

        /// <summary>
        /// Оператор сравнения контента
        /// </summary>
        /// <param name="scLinkContent1">Первый контент</param>
        /// <param name="scLinkContent2">Второй контент</param>
        /// <returns>Возвращает True, если контенты равны</returns>
        public static bool operator !=(ScLinkContent scLinkContent1, ScLinkContent scLinkContent2)
        {
            return !(scLinkContent1 == scLinkContent2);
        }

        #endregion

        private void StreamFree()
        {
            NativeMethods.sc_stream_free(this.scStream);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.content = null;
                }
                //unmanaged
                this.StreamFree();
                this.scStream = IntPtr.Zero;
            }
        }

        ~ScLinkContent()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
