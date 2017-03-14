using System;
using System.Collections;
using System.Collections.Generic;
using ScEngineNet.Native;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Энумератор для итераторов библиотеки
    /// </summary>
    public class ScEnumerator : IEnumerator<ScConstruction>
    {
        private const string disposalExceptionMsg = "Был вызван метод Dispose и cсылка на объект в памяти уже удалена";
        private const string memoryNotInitializedExceptionMsg = "Библиотека ScMemory.Net не инициализирована";
        private const string contextInvalidExceptionMsg = "Указанная ссылка на ScContext не действительна";
        private IntPtr iterator;
        private readonly ScIterator3Type iterator3Type;
        private readonly ScIterator5Type iterator5Type;
        private readonly ScIteratorParam p1;
        private readonly ScIteratorParam p2;
        private readonly ScIteratorParam p3;
        private readonly ScIteratorParam p4;
        private readonly ScIteratorParam p5;
        private readonly ScMemoryContext scContext;

        private bool Delete()
        {
            const bool isDeleted = false;
            if (iterator3Type != ScIterator3Type.sc_iterator3_unknown)
            {
                NativeMethods.sc_iterator3_free(iterator);
                iterator = IntPtr.Zero;
            }
            else
            {
                NativeMethods.sc_iterator5_free(iterator);
                iterator = IntPtr.Zero;
            }
            return isDeleted;
        }

        #region Конструкторы

        private ScEnumerator(ScMemoryContext scContext)
        {
            this.scContext = scContext;
        }


        internal ScEnumerator(ScMemoryContext scContext, ScIterator3Type iterator3Type, ScIteratorParam p1,
            ScIteratorParam p2, ScIteratorParam p3)
            : this(scContext)
        {
            this.iterator3Type = iterator3Type;
            iterator5Type = ScIterator5Type.sc_iterator5_unknown;

            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            iterator = NativeMethods.sc_iterator3_new(this.scContext.PtrScMemoryContext, this.iterator3Type, this.p1, this.p2,
                this.p3);
        }

        internal ScEnumerator(ScMemoryContext scContext, ScIterator5Type iterator5Type, ScIteratorParam p1,
            ScIteratorParam p2, ScIteratorParam p3, ScIteratorParam p4, ScIteratorParam p5)
            : this(scContext)
        {
            iterator3Type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5Type = iterator5Type;

            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
            iterator = NativeMethods.sc_iterator5_new(this.scContext.PtrScMemoryContext, this.iterator5Type, this.p1, this.p2,
                this.p3, this.p4, this.p5);
        }

        #endregion

        #region IEnumerator<Construction>

        private ScConstruction currentConstruction;

        /// <summary>
        ///     Получает элемент коллекции, соответствующий текущей позиции перечислителя.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ScConstruction Current
        {
            get
            {
                if (iterator == IntPtr.Zero || currentConstruction == null)
                {
                    throw new InvalidOperationException();
                }

                return currentConstruction;
            }
        }

        private object Current1
        {
            get { return Current; }
        }

        object IEnumerator.Current
        {
            get { return Current1; }
        }

        /// <summary>
        ///     Перемещает перечислитель к следующему элементу коллекции.
        /// </summary>
        /// <returns>
        ///     Значение true, если перечислитель был успешно перемещен к следующему элементу; значение false, если перечислитель
        ///     достиг конца коллекции.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="ScMemoryNotInitializeException"></exception>
        /// <exception cref="ScContextInvalidException"></exception>
        public bool MoveNext()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (scContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }

            currentConstruction = new ScConstruction();
            if (iterator3Type != ScIterator3Type.sc_iterator3_unknown)
            {
                if (NativeMethods.sc_iterator3_next(iterator) == false)
                {
                    return false;
                }
                for (uint element = 0; element < 3; element++)
                {
                    currentConstruction.AddElement(
                        scContext.GetElement(new ScAddress(NativeMethods.sc_iterator3_value(iterator, element))));
                }
            }
            else
            {
                if (NativeMethods.sc_iterator5_next(iterator) == false)
                {
                    return false;
                }
                for (uint element = 0; element < 5; element++)
                {
                    currentConstruction.AddElement(
                        scContext.GetElement(new ScAddress(NativeMethods.sc_iterator5_value(iterator, element))));
                }
            }
            return true;
        }


        /// <summary>
        ///     Устанавливает перечислитель в его начальное положение, т. е. перед первым элементом коллекции.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="ScMemoryNotInitializeException"></exception>
        /// <exception cref="ScContextInvalidException"></exception>
        public void Reset()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (ScMemoryContext.IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            if (scContext.PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ScContextInvalidException(contextInvalidExceptionMsg);
            }

            //делаем новый указатель на итератор
            if (iterator3Type != ScIterator3Type.sc_iterator3_unknown)
            {
                NativeMethods.sc_iterator3_free(iterator);

                iterator = NativeMethods.sc_iterator3_new(scContext.PtrScMemoryContext, iterator3Type, p1, p2, p3);
            }
            if (iterator5Type != ScIterator5Type.sc_iterator5_unknown)
            {
                NativeMethods.sc_iterator5_free(iterator);
                iterator = NativeMethods.sc_iterator5_new(scContext.PtrScMemoryContext, iterator5Type, p1, p2, p3, p4,
                    p5);
            }
            currentConstruction = null;
        }

        #endregion

        #region IDisposal

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
            //   Console.WriteLine("call Dispose({0}) ScEnumerator with {1}", disposing, this.iterator);


            if (!Disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                Delete();
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
        ///     Finalizes an instance of the <see cref="ScEnumerator" /> class.
        /// </summary>
        ~ScEnumerator()
        {
            Dispose(false);
        }

        #endregion
    }
}