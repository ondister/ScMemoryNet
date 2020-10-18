using System;
using System.Collections.Generic;
using System.Linq;
using ScEngineNet.Native;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScEngineNet.LinkContent
{
    /// <summary>
    ///     Базовый класс для содержимого sc-ссылки
    /// </summary>
    public abstract class ScLinkContent :  IEquatable<ScLinkContent>, IDisposable
    {
        private static readonly Dictionary<Identifier, Func<byte[], ScLinkContent>> ScLinkContentConstructors = new Dictionary
            <Identifier, Func<byte[], ScLinkContent>>
        {
            {ScDataTypes.Instance.TypeString, bytes => new ScString(bytes)},
            {ScDataTypes.Instance.LanguageEn, bytes => new ScLangEn(bytes)},
            {ScDataTypes.Instance.LanguageRu, bytes => new ScLangRu(bytes)},
            {ScDataTypes.Instance.TypeBinary, bytes => new ScBinary(bytes)},
            {ScDataTypes.Instance.NumericInt, bytes => new ScInt32(bytes)},
            {ScDataTypes.Instance.NumericDouble, bytes => new ScDouble(bytes)},
            {ScDataTypes.Instance.NumericLong, bytes => new ScLong(bytes)},
            {ScDataTypes.Instance.TypeBool, bytes => new ScBool(bytes)},
            {ScDataTypes.Instance.NumericByte, bytes => new ScByte(bytes)},
            {ScDataTypes.Instance.Date, bytes => new ScDate(bytes)},
            {ScDataTypes.Instance.DateTime, bytes => new ScDateTime(bytes)},
            {ScDataTypes.Instance.Time, bytes => new ScTime(bytes)},
            {ScDataTypes.Instance.TimeInterval, bytes => new ScTimeInterval(bytes)},
            {ScDataTypes.Instance.Bitmap, bytes => new ScBitmap(bytes)},
            {ScDataTypes.Instance.Rtf, bytes => new ScRtf(bytes)},
            {ScDataTypes.Instance.Wkt, bytes => new ScWkt(bytes)}
        };

      
        private static readonly Dictionary<Identifier, Func<IntPtr, ScLinkContent>> ScLinkIntPtrConstructors = new Dictionary
            <Identifier, Func<IntPtr, ScLinkContent>>
        {
            {ScDataTypes.Instance.TypeString, streamIntPtr => new ScString(streamIntPtr)},
            {ScDataTypes.Instance.LanguageRu, streamIntPtr => new ScLangRu(streamIntPtr)},
            {ScDataTypes.Instance.LanguageEn, streamIntPtr => new ScLangEn(streamIntPtr)},
            {ScDataTypes.Instance.TypeBinary, streamIntPtr => new ScBinary(streamIntPtr)},
            {ScDataTypes.Instance.NumericInt, streamIntPtr => new ScInt32(streamIntPtr)},
            {ScDataTypes.Instance.NumericDouble, streamIntPtr => new ScDouble(streamIntPtr)},
            {ScDataTypes.Instance.NumericLong, streamIntPtr => new ScLong(streamIntPtr)},
            {ScDataTypes.Instance.TypeBool, streamIntPtr => new ScBool(streamIntPtr)},
            {ScDataTypes.Instance.NumericByte, streamIntPtr => new ScByte(streamIntPtr)},
            {ScDataTypes.Instance.Date, streamIntPtr => new ScDate(streamIntPtr)},
            {ScDataTypes.Instance.DateTime, streamIntPtr => new ScDateTime(streamIntPtr)},
            {ScDataTypes.Instance.Time, streamIntPtr => new ScTime(streamIntPtr)},
            {ScDataTypes.Instance.TimeInterval, streamIntPtr => new ScTimeInterval(streamIntPtr)},
            {ScDataTypes.Instance.Bitmap, streamIntPtr => new ScBitmap(streamIntPtr)},
            {ScDataTypes.Instance.Rtf, streamIntPtr => new ScRtf(streamIntPtr)},
            {ScDataTypes.Instance.Wkt, streamIntPtr => new ScWkt(streamIntPtr)}
        };

        private IntPtr scStream;

        /// <summary>
        ///     Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        ///     Ключевой узел.
        /// </value>
        public abstract Identifier ClassNodeIdentifier { get; }

        /// <summary>
        ///     Возвращает массив байт. Можно использовать статические конвертеры этого класса для преобразования байт в значение.
        /// </summary>
        /// <value>
        ///     Массив байт
        /// </value>
        public byte[] Bytes { get; set; }

        internal IntPtr ScStream
        {
            get { return scStream; }
            set { SetStream(value); }
        }

        internal static ScLinkContent GetScContent(byte[] bytes, Identifier classNodeIdentifier)
        {
            Func<byte[], ScLinkContent> constructor;
            return ScLinkContentConstructors.TryGetValue(classNodeIdentifier, out constructor)
                ? constructor(bytes)
                : new ScBinary(bytes);
        }

        internal static ScLinkContent GetScContent(IntPtr streamIntPtr, Identifier classNodeIdentifier)
        {
            Func<IntPtr, ScLinkContent> constructor;
            return ScLinkIntPtrConstructors.TryGetValue(classNodeIdentifier, out constructor)
                ? constructor(streamIntPtr)
                : new ScBinary(streamIntPtr);
        }

        private void SetStream(IntPtr stream)
        {
            if (stream != IntPtr.Zero)
            {
                uint buffersize;
                if (NativeMethods.sc_stream_get_length(stream, out buffersize) == ScResult.ScResultOk)
                {
                    var buffer = new byte[buffersize];
                    uint receivedBytes;
                    if (NativeMethods.sc_stream_read_data(stream, buffer, buffersize, out receivedBytes) ==
                        ScResult.ScResultOk)
                    {
                        Bytes = buffer;
                    }
                }
            }
        }

        #region Конструкторы

        internal ScLinkContent(byte[] bytes)
        {
            Bytes = bytes;
            scStream = NativeMethods.sc_stream_memory_new(Bytes, (uint) bytes.Length, ScStreamFlag.SC_STREAM_FLAG_READ,false);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScLinkContent" /> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected ScLinkContent(IntPtr stream) :
            this(new byte[0])
        {
            SetStream(stream);
        }

        internal ScLinkContent() :
            this(new byte[0])
        {
        }



        #endregion

        # region перегруженные операторы

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.String" /> to <see cref="ScLinkContent" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(string value)
        {
            return new ScString(value);
        }


        /// <summary>
        ///     Оператор присваивания для массива байт
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(byte[] value)
        {
            return new ScBinary(value);
        }


        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Int32" /> to <see cref="ScLinkContent" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(int value)
        {
            return new ScInt32(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Double" /> to <see cref="ScLinkContent" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(double value)
        {
            return new ScDouble(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Int64" /> to <see cref="ScLinkContent" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(long value)
        {
            return new ScLong(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Boolean" /> to <see cref="ScLinkContent" />.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(bool value)
        {
            return new ScBool(value);
        }


        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Byte" /> to <see cref="ScLinkContent" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(byte value)
        {
            return new ScByte(value);
        }

        #endregion

        #region Конвертация


        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(byte[] bytes)
        {
            return ScEngineNet.TextEncoding.GetString(bytes);
        }
        

        /// <summary>
        ///     To the int32.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static int ToInt32(ScLinkContent content)
        {
            int result;
            if (content.Bytes.Length == 4)
            {
                result = BitConverter.ToInt32(content.Bytes, 0);
            }
            else
            {
                var stringData = ToString(content.Bytes);
                result = Int32.Parse(stringData);
            }
            return result;
        }

        /// <summary>
        ///     To the binary.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static byte[] ToBinary(ScLinkContent content)
        {
            return content.Bytes;
        }


        /// <summary>
        ///     To the double.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static double ToDouble(ScLinkContent content)
        {
            double result;
            if (content.Bytes.Length == 8)
            {
                result = BitConverter.ToDouble(content.Bytes, 0);
            }
            else
            {
                var stringData = ToString(content.Bytes);
                result = Double.Parse(stringData);
            }
            return result;
        }


        public static double ToDouble(byte[] bytes)
        {
            double result;
            if (bytes.Length == 8)
            {
                result = BitConverter.ToDouble(bytes, 0);
            }
            else
            {
                var stringData = ToString(bytes);
                result = Double.Parse(stringData);
            }
            return result;
        }


        /// <summary>
        ///     To the long.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static long ToLong(ScLinkContent content)
        {
            long result;
            if (content.Bytes.Length == 8)
            {
                result = BitConverter.ToInt64(content.Bytes, 0);
            }
            else
            {
                var stringData = ToString(content.Bytes);
                result = long.Parse(stringData);
            }
            return result;
        }

        public static long ToLong(byte[] bytes)
        {
            long result;
            if (bytes.Length == 8)
            {
                result = BitConverter.ToInt64(bytes, 0);
            }
            else
            {
                var stringData = ToString(bytes);
                result = long.Parse(stringData);
            }
            return result;
        }

        /// <summary>
        ///     To the bool.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static bool ToBool(ScLinkContent content)
        {
            return BitConverter.ToBoolean(content.Bytes, 0);
        }

        /// <summary>
        ///     To the byte.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static byte ToByte(ScLinkContent content)
        {
            return content.Bytes[0];
        }

        #endregion

        #region Реализация сравнения

        /// <summary>
        ///     Определяет равен ли заданный объект <see cref="ScLinkContent" /> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="ScLinkContent" /></param>
        public bool Equals(ScLinkContent obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return obj.Bytes.SequenceEqual(Bytes);
        }

        /// <summary>
        ///     Определяет равен ли заданный объект <see cref="T:System.Object" /> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object" /></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Equals(obj as ScLinkContent);
        }

        /// <summary>
        ///     Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return BitConverter.ToInt32(Bytes, 0);
        }

        /// <summary>
        ///     Оператор сравнения контента
        /// </summary>
        /// <param name="scLinkContent1">Первый  контент</param>
        /// <param name="scLinkContent2">Второй контент</param>
        /// <returns>Возвращает True, если контенты равны</returns>
        public static bool operator ==(ScLinkContent scLinkContent1, ScLinkContent scLinkContent2)
        {
            if (ReferenceEquals(scLinkContent1, null) || ReferenceEquals(scLinkContent2, null))
            {
                return ReferenceEquals(scLinkContent1, scLinkContent2);
            }
            return scLinkContent1.Equals(scLinkContent2);
        }

        /// <summary>
        ///     Оператор сравнения контента
        /// </summary>
        /// <param name="scLinkContent1">Первый контент</param>
        /// <param name="scLinkContent2">Второй контент</param>
        /// <returns>Возвращает True, если контенты равны</returns>
        public static bool operator !=(ScLinkContent scLinkContent1, ScLinkContent scLinkContent2)
        {
            if (ReferenceEquals(scLinkContent1, null) || ReferenceEquals(scLinkContent2, null))
            {
                return !ReferenceEquals(scLinkContent1, scLinkContent2);
            }
            return !scLinkContent1.Equals(scLinkContent2);
        }

       #endregion

        #region IDisposal

        private void StreamFree()
        {
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_stream_free(scStream);
               // scStream = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="ScEnumerator" /> is disposed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            //  Console.WriteLine("call Dispose({0}) ScLinkContent with {1}", disposing, this.scStream);


            if (!Disposed)
            {
                // Dispose of resources held by this instance.
                StreamFree();
                // Suppress finalization of this disposed instance.
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
                Disposed = true;
            }
        }

        /// <summary>
        ///     Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="ScLinkContent" /> class.
        /// </summary>
        ~ScLinkContent()
        {
            Dispose(false);
        }

        #endregion


        public abstract override string ToString();

    }
}