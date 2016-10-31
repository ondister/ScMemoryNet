using ScEngineNet.NativeElements;
using ScEngineNet.NetHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Базовый класс для содержимого sc-ссылки
    /// </summary>
    public abstract class ScLinkContent : IDisposable, IEquatable<ScLinkContent>
    {

        private IntPtr scStream;
        private byte[] bytes;

       

        /// <summary>
        /// Ключевой узел, определяющий тип содержимого
        /// </summary>
        /// <value>
        /// Ключевой узел.
        /// </value>
        public abstract Identifier ClassNodeIdentifier
        {
            get;
        }

        /// <summary>
        /// Возвращает массив байт. Можно использовать статические конвертеры этого класса для преобразования байт в значение.
        /// </summary>
        /// <value>
        /// Массив байт
        /// </value>
        public byte[] Bytes
        {
            get { return bytes; }
        }

        internal IntPtr ScStream
        {
            get { return scStream; }
            set { this.SetStream(value); }
        }

        #region Конструкторы


        internal ScLinkContent(byte[] bytes)
        {
            this.bytes = bytes;
            scStream = NativeMethods.sc_stream_memory_new(this.bytes, (uint)bytes.Length, ScStreamFlag.SC_STREAM_FLAG_READ, false);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ScLinkContent"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        protected ScLinkContent(IntPtr Stream) :
            this(new byte[0])
        {
            this.SetStream(Stream);
        }

        internal ScLinkContent() :
            this(new byte[0])
        { }
        #endregion

        internal static ScLinkContent GetScContent(byte[] bytes, Identifier classNodeIdentifier)
        {
            Func<byte[], ScLinkContent> constructor;
            return ScLinkContentConstructors.TryGetValue(classNodeIdentifier, out constructor)
                ? constructor(bytes)
                : new ScObject(bytes);
        }
        internal static ScLinkContent GetScContent(IntPtr streamIntPtr, Identifier classNodeIdentifier)
        {
            Func<IntPtr, ScLinkContent> constructor;
            return ScLinkIntPtrConstructors.TryGetValue(classNodeIdentifier, out constructor)
                ? constructor(streamIntPtr)
                : new ScObject(streamIntPtr);
        }

        private static readonly Dictionary<Identifier, Func<byte[], ScLinkContent>> ScLinkContentConstructors = new Dictionary<Identifier, Func<byte[], ScLinkContent>>
        {
            { ScDataTypes.Instance.TypeString, bytes => new ScString(bytes) }, 
            { ScDataTypes.Instance.TypeBinary, bytes => new ScBinary(bytes) },
            { ScDataTypes.Instance.NumericInt, bytes => new ScInt32(bytes) },
            {  ScDataTypes.Instance.NumericDouble, bytes => new ScDouble(bytes) },
            {  ScDataTypes.Instance.NumericLong, bytes => new ScLong(bytes) },
            {  ScDataTypes.Instance.TypeBool, bytes => new ScBool(bytes) },
            {  ScDataTypes.Instance.NumericByte, bytes => new ScByte(bytes) }
        };



        private static readonly Dictionary<Identifier, Func<IntPtr, ScLinkContent>> ScLinkIntPtrConstructors = new Dictionary<Identifier, Func<IntPtr, ScLinkContent>>
        {
            { ScDataTypes.Instance.TypeString, streamIntPtr  => new ScString(streamIntPtr) }, 
            { ScDataTypes.Instance.TypeBinary, streamIntPtr => new ScBinary(streamIntPtr) },
            { ScDataTypes.Instance.NumericInt, streamIntPtr => new ScInt32(streamIntPtr) },
            { ScDataTypes.Instance.NumericDouble, streamIntPtr => new ScDouble(streamIntPtr) },
            { ScDataTypes.Instance.NumericLong, streamIntPtr => new ScLong(streamIntPtr) },
            { ScDataTypes.Instance.TypeBool, streamIntPtr => new ScBool(streamIntPtr) },
            { ScDataTypes.Instance.NumericByte, streamIntPtr => new ScByte(streamIntPtr) },
        };



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
                        this.bytes = buffer;
                    }
                }
            }
        }



        # region перегруженные операторы

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ScLinkContent"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(string value)
        {
            return new ScString(value);
        }


        /// <summary>
        /// Оператор присваивания для массива байт
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(byte[] value)
        {
            return new ScBinary(value);
        }


        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ScLinkContent"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(int value)
        {
            return new ScInt32(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="ScLinkContent"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(double value)
        {
            return new ScDouble(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="ScLinkContent"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(long value)
        {
            return new ScLong(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Boolean"/> to <see cref="ScLinkContent"/>.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(bool value)
        {
            return new ScBool(value);
        }


        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="ScLinkContent"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ScLinkContent(byte value)
        {
            return new ScByte(value);
        }



        #endregion



        #region Конвертация


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(byte[] bytes)
        {
            return ScEngineNet.TextEncoding.GetString(bytes);
        }


        /// <summary>
        /// To the int32.
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
                string stringData = ScLinkContent.ToString(content.Bytes);
                result = Int32.Parse(stringData);
            }
            return result;
        }

        /// <summary>
        /// To the binary.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static byte[] ToBinary(ScLinkContent content)
        {
            return content.Bytes;
        }


        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static double ToDouble(ScLinkContent content)
        {
            double result = double.NaN;
            if (content.Bytes.Length == 8)
            {
                result = BitConverter.ToDouble(content.Bytes, 0);
            }
            else
            {
                string stringData = ScLinkContent.ToString(content.Bytes);
                result = Double.Parse(stringData);
            }
            return result;
        }

        /// <summary>
        /// To the long.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static long ToLong(ScLinkContent content)
        {
            long result = 0;
            if (content.Bytes.Length == 8)
            {
                result = BitConverter.ToInt64(content.Bytes, 0);
            }
            else
            {
                string stringData = ScLinkContent.ToString(content.Bytes);
                result = long.Parse(stringData);
            }
            return result;
        }

        /// <summary>
        /// To the bool.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static bool ToBool(ScLinkContent content)
        {
            return BitConverter.ToBoolean(content.Bytes, 0);
        }

        /// <summary>
        /// To the byte.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static byte ToByte(ScLinkContent content)
        {
            return content.bytes[0];
        }

        #endregion

        #region Реализация сравнения

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="ScLinkContent"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="ScLinkContent"/></param>
        public bool Equals(ScLinkContent obj)
        {
            return obj != null && obj.Bytes.SequenceEqual(this.Bytes);
        }

        /// <summary>
        /// Определяет равен ли заданный объект <see cref="T:System.Object"/> текущему объекту
        /// </summary>
        /// <param name="obj">объект <see cref="T:System.Object"/></param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ScLinkContent ScLinkContentBinary = obj as ScLinkContent;
            if (ScLinkContentBinary as ScLinkContent == null)
                return false;
            return ScLinkContentBinary.Bytes.SequenceEqual(this.Bytes);
        }

        /// <summary>
        /// Возвращает Hash код текущего объекта
        /// </summary>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.Bytes);
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

      
      

           #region IDisposal
        private bool disposed;

          private void StreamFree()
        {
            NativeMethods.sc_stream_free(this.scStream);
            this.scStream = IntPtr.Zero;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ScEnumerator"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
          //  Console.WriteLine("call Dispose({0}) ScLinkContent with {1}", disposing, this.scStream);


            if (!disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                this.StreamFree();
                // Suppress finalization of this disposed instance.
                if (disposing)
                {

                    GC.SuppressFinalize(this);
                }
                disposed = true;
            }


        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScLinkContent"/> class.
        /// </summary>
        ~ScLinkContent()
        {
            Dispose(false);
        }
        #endregion


    }
}
