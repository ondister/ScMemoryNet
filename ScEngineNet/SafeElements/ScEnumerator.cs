﻿using ScEngineNet.NativeElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Итератор для поиска конструкций по шаблону.
    /// Создается в методе CreateIterator класса <see cref="ScMemoryContext" />
    /// </summary>
    public class ScEnumerator :  IEnumerator<ScConstruction>,IDisposable
    {
        private const string disposalException_msg = "Был вызван метод Dispose и cсылка на объект в памяти уже удалена";
        private const string memoryNotInitializedException_msg = "Библиотека ScMemory.Net не инициализирована";
        private const string contextInvalidException_msg = "Указанная ссылка на ScContext не действительна";

        private readonly ScMemoryContext scContext;
        private readonly ScIterator3Type iterator3type;
        private readonly ScIterator5Type iterator5type;
        private IntPtr iterator = IntPtr.Zero;

        private ScIteratorParam p1;
        private ScIteratorParam p2;
        private ScIteratorParam p3;
        private ScIteratorParam p4;
        private ScIteratorParam p5;




        #region Конструкторы

        private ScEnumerator(ScMemoryContext scContext)
        {
            this.scContext = scContext;
        }


        internal ScEnumerator(ScMemoryContext scContext, ScIterator3Type iterator3Type,ScIteratorParam p1,ScIteratorParam p2,ScIteratorParam p3)
            : this(scContext)
        {
            this.iterator3type = iterator3Type;
            this.iterator5type = ScIterator5Type.sc_iterator5_unknown;

            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
           
            this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, p1, p2, p3);
        }

        internal ScEnumerator(ScMemoryContext scContext, ScIterator5Type iterator5Type, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3, ScIteratorParam p4, ScIteratorParam p5)
            : this(scContext)
        {
            this.iterator3type = ScIterator3Type.sc_iterator3_unknown;
            this.iterator5type = iterator5Type;
           
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }
           
            this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, p1, p2, p3, p4, p5);
        }

        #endregion


        #region IEnumerator<Construction>
        private ScConstruction currentConstruction;

        public ScConstruction Current
        {
            get
            {
                if (iterator == null || currentConstruction == null)
                {
                    throw new InvalidOperationException();
                }

                return currentConstruction;
            }
        }

        private object Current1
        {

            get { return this.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current1; }
        }

        public bool MoveNext()
        {
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            this.currentConstruction = new ScConstruction();
            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
                if (NativeMethods.sc_iterator3_next(iterator) == false)
                {
                    return false;
                }
                for (uint element = 0; element < 3; element++)
                {
                    this.currentConstruction.Elements.Add(ScMemorySafeMethods.GetElement(new ScAddress(NativeMethods.sc_iterator3_value(this.iterator, element)).WScAddress, this.scContext));
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
                    this.currentConstruction.Elements.Add(ScMemorySafeMethods.GetElement(new ScAddress(NativeMethods.sc_iterator5_value(this.iterator, element)).WScAddress, this.scContext));
                }
            }
            return true;

        }




        public void Reset()
        {
            if (this.Disposed == true) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            if (this.scContext.PtrScMemoryContext == IntPtr.Zero) { throw new ScContextInvalidException(contextInvalidException_msg); }

            //делаем новый указатель на итератор
            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
                NativeMethods.sc_iterator3_free(this.iterator);

                this.iterator = ScMemorySafeMethods.CreateIterator3(this.scContext, this.iterator3type, this.p1, this.p2, this.p3);
            }
            if (this.iterator5type != ScIterator5Type.sc_iterator5_unknown)
            {
                NativeMethods.sc_iterator5_free(this.iterator);
                this.iterator = ScMemorySafeMethods.CreateIterator5(this.scContext, this.iterator5type, this.p1, this.p2, this.p3, this.p4, this.p5);
            }
            currentConstruction = null;

        }

        #endregion


       
        
        private bool Delete()
        {
            bool isDeleted = false;
            if (this.iterator3type != ScIterator3Type.sc_iterator3_unknown)
            {
             NativeMethods.sc_iterator3_free(this.iterator);
             this.iterator = IntPtr.Zero;
            }
            else
            {
                NativeMethods.sc_iterator5_free(this.iterator);
                this.iterator = IntPtr.Zero;
            }
            return isDeleted;
        }


          #region IDisposal
        private bool disposed;

        public bool Disposed
        {
            get { return disposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("call Dispose({0}) ScEnumerator with {1}", disposing, this.iterator);


            if (!disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                this.Delete();
                // Suppress finalization of this disposed instance.
                if (disposing)
                {
                   
                    GC.SuppressFinalize(this);
                }
                disposed = true;
            }


        }

        public void Dispose()
        {
            Dispose(true);

        }

        ~ScEnumerator()
        {
            Dispose(false);
        }
        #endregion
       



    }
}