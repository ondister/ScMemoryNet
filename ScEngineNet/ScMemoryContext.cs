using ScEngineNet.NativeElements;
using ScEngineNet.NetHelpers;
using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ScEngineNet
{
    /// <summary>
    /// Контекст памяти.
    /// Это виртуальный интерфейс для доступа к памяти, которому устанавливается уровень доступа.
    /// Контекст с меньшим уровнем доступа не имеет доступ к элементам, созданным в контекс.
    /// </summary>
    public class ScMemoryContext : IDisposable
    {
        private IntPtr scMemoryContext;

        /// <summary>
        /// Возвращает установленный для контекста уровень доступа
        /// </summary>
        /// <value>
        /// Уровень доступа <see cref="ScAccessLevels"/>
        /// </value>
        public ScAccessLevels AccessLevel
        {
            get
            {
                var context = (WScMemoryContext)Marshal.PtrToStructure(scMemoryContext, typeof(WScMemoryContext));
                return (ScAccessLevels)context.AccessLevels;
            }
        }



        #region initialize

        /// <summary>
        /// Возвращает true если контекст инициализирован правильно.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            bool isValid = scMemoryContext != IntPtr.Zero;
            return isValid;
        }

        /// <summary>
        /// Определяет, инициализирована ли память
        /// </summary>
        /// <returns>Вернет True, если память уже инициализирована</returns>
        public static bool IsMemoryInitialized()
        {
            return NativeMethods.sc_memory_is_initialized();
        }

        internal ScMemoryContext(IntPtr context)
        {
            this.scMemoryContext = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScMemoryContext"/> class.
        /// Для создания экземпляра контекста, необходимо инициализировать память <see cref="ScMemory"/>
        /// </summary>
        /// <param name="accessLevels"> Уровень доступа</param>
        public ScMemoryContext(ScAccessLevels accessLevels)
        {
            if (ScMemoryContext.IsMemoryInitialized())
            {
                this.scMemoryContext = NativeMethods.sc_memory_context_new((byte)accessLevels);
            }
        }

        /// <summary>
        /// Удаляет контекст
        /// </summary>
        public void Delete()
        {
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_memory_context_free(this.scMemoryContext);
                this.scMemoryContext = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Получение статистики
        /// </summary>
        /// <returns>Структуру <see cref="ScStat"/>, содержащую статистику хранилища</returns>
        public ScStat GetStatistics()
        {
            var stat = new ScStat();
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_memory_stat(this.scMemoryContext, out stat);
            }
            return stat;
        }

        /// <summary>
        /// Сохраняет состояние хранилища
        /// </summary>
        /// <returns>ScResult.SC_RESULT_OK, если состояние сохранить удалось</returns>
        public ScResult SaveState()
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                result = NativeMethods.sc_memory_save(this.scMemoryContext);
            }
            return result;
        }

        #endregion

        #region Elements

        #region Common

        /// <summary>
        /// Создает уникальный идентификатор для узла на основе его адреса
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Уникальный идентификатр узла</returns>
        public Identifier CreateUniqueIdentifier(ScNode node)
        {
            return Identifier.GetUnique(this.scMemoryContext, node);
        }

        /// <summary>
        /// Создает уникальный идентификатор для узла на основе его адреса и преффикса
        /// </summary>
        /// <param name="prefix">Преффикс</param>
        /// <param name="node">Узел</param>
        /// <returns>Уникальный идентификатр узла</returns>
        public Identifier CreateUniqueIdentifier(string prefix, ScNode node)
        {
            return Identifier.GetUnique(this.scMemoryContext, prefix, node);
        }

        /// <summary>
        /// Определяет, существует ли элемент с указанным адресом
        /// </summary>
        /// <param name="elementAddress">Адрес элемента</param>
        /// <returns></returns>
        public bool IsElementExist(ScAddress elementAddress)
        {
            return ScMemorySafeMethods.IsElementExist(this.scMemoryContext, elementAddress);
        }


        #endregion

        #region Arcs

        /// <summary>
        /// Создает новую дугу в хранилище.
        /// Если такая дуга уже ест в хранилище, дубликат не создается, и возвращается имеющаяся дуга
        /// </summary>
        /// <param name="beginElement">Начальный элемент</param>
        /// <param name="endElement">Конечный элемент</param>
        /// <param name="arcType">Тип дуги </param>
        /// <returns>Созданную дугу</returns>
        public ScArc CreateArc(ScElement beginElement, ScElement endElement, ElementType arcType)
        {
            var createdArc = new ScArc(ScAddress.Invalid, this.scMemoryContext);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdArc = ScMemorySafeMethods.CreateArc(this.scMemoryContext, arcType, beginElement, endElement);
            }
            return createdArc;
        }

        /// <summary>
        /// Находит дугу по указанному адресу
        /// </summary>
        /// <param name="arcAddress">Адрес дуги</param>
        /// <returns>sc-дуга</returns>
        public ScArc FindArc(ScAddress arcAddress)
        {
            var createdArc = new ScArc(ScAddress.Invalid, this.scMemoryContext);
            var scElement = ScMemorySafeMethods.GetElement(arcAddress.WScAddress, this.scMemoryContext);
            if (scElement.ElementType.HasAnyType(ElementType.ArcMask_c))
            {
                createdArc = (ScArc)scElement;
            }
            return createdArc;

        }

        /// <summary>
        /// Определяет, существует ли дуга с указанными параметрами
        /// </summary>
        /// <param name="beginElement">Начальный элемент</param>
        /// <param name="endElement">Конечный элемент</param>
        /// <param name="arcType">Тип дуги</param>
        /// <returns>True, если дуга существует</returns>
        public bool ArcIsExist(ScElement beginElement, ScElement endElement, ElementType arcType)
        {
            bool result = false;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                result = NativeMethods.sc_helper_check_arc(this.scMemoryContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType);
            }
            return result;

        }

        #endregion

        #region Nodes

        /// <summary>
        /// Создает новый узел указанного типа
        /// </summary>
        /// <param name="nodeType">Тип узла</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ElementType nodeType)
        {
            var createdNode = new ScNode(ScAddress.Invalid, this.scMemoryContext);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdNode = new ScNode(new ScAddress(NativeMethods.sc_memory_node_new(this.scMemoryContext, nodeType)), this.scMemoryContext);
            }
            return createdNode;
        }

        /// <summary>
        /// СОздает узел с указанным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип узла</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ElementType nodeType, Identifier sysIdentifier)
        {
            ScNode createdNode = this.FindNode(sysIdentifier);
            if (createdNode.ScAddress == ScAddress.Invalid)
            {
                createdNode = this.CreateNode(nodeType);
                createdNode.SystemIdentifier = sysIdentifier;
            }
            return createdNode;
        }

        /// <summary>
        /// Создает узел с указанным системным идентификатором и основным русскоязычным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип уза</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <param name="ruIdentifier">Основной русскоязычный идентификатор</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ElementType nodeType, Identifier sysIdentifier, Identifier ruIdentifier)
        {
            ScNode createdNode = this.CreateNode(nodeType, sysIdentifier);
            createdNode.MainIdentifiers[NLanguages.Lang_ru] = new ScString(ruIdentifier.Value);
            return createdNode;
        }

        /// <summary>
        /// Создает узел с указанным системным идентификатором и основным русскоязычным и англоязычным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип уза</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <param name="ruIdentifier">Основной русскоязычный идентификатор</param>
        /// <param name="enIdentifier">Англоязычный идентификатор</param>
        /// <returns>
        /// Созданный узел
        /// </returns>
        public ScNode CreateNode(ElementType nodeType, Identifier sysIdentifier, Identifier ruIdentifier, Identifier enIdentifier)
        {
            ScNode createdNode = this.CreateNode(nodeType, sysIdentifier, ruIdentifier);
            createdNode.MainIdentifiers[NLanguages.Lang_en] = new ScString(enIdentifier.Value);
            return createdNode;
        }


        /// <summary>
        /// Ищет узел с указанным системным идентификатором
        /// </summary>
        /// <param name="identifier">Системный идентификатор</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(Identifier identifier)
        {
            return ScMemorySafeMethods.FindNode(this.scMemoryContext, identifier);
        }

        /// <summary>
        /// Ищет узел по известному адресу
        /// </summary>
        /// <param name="nodeAddress">Адрес узла</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(ScAddress nodeAddress)
        {
            var createdNode = new ScNode(ScAddress.Invalid, this.scMemoryContext);
            var scElement = ScMemorySafeMethods.GetElement(nodeAddress.WScAddress, this.scMemoryContext);
            if (scElement.ElementType.HasAnyType(ElementType.NodeOrStructureMask_c))
            {
                createdNode = (ScNode)scElement;
            }
            return createdNode;
        }

        #endregion

        #region Links

        /// <summary>
        /// Создает новую sc-ссылку
        /// </summary>
        /// <returns>Созданная sc-ссылка</returns>
        public ScLink CreateLink()
        {
            var createdLink = new ScLink(ScAddress.Invalid, this.scMemoryContext);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdLink = new ScLink(new ScAddress(NativeMethods.sc_memory_link_new(this.scMemoryContext)), this.scMemoryContext);
            }
            return createdLink;
        }

        /// <summary>
        ///Создает новую sc-ссылку с указанным контентом
        /// </summary>
        /// <param name="content">Контент</param>
        /// <returns>Созданная sc-ссылка</returns>
        public ScLink CreateLink(ScLinkContent content)
        {
            List<ScLink> findLinks = this.FindLinks(content);
            if (findLinks.Count > 0)
            {
                return findLinks[0];
            }
            ScLink createdLink = this.CreateLink();
            createdLink.LinkContent = content;

            return createdLink;
        }




        /// <summary>
        /// Ищет sc-ссылку по указанному адресу
        /// </summary>
        /// <param name="linkAddress">The link address.</param>
        /// <returns>Найденная ссылка</returns>
        public ScLink FindLink(ScAddress linkAddress)
        {
            var createdLink = new ScLink(ScAddress.Invalid, this.scMemoryContext);
            var scElement = ScMemorySafeMethods.GetElement(linkAddress.WScAddress, this.scMemoryContext);
            if (scElement.ElementType==ElementType.Link_a)
            {
                createdLink = (ScLink)scElement;
            }
            return createdLink;
        }

        /// <summary>
        /// Ищет все ссылки, содержащие указанный контент.
        /// Возвращаются ссылки, контент которых полностью совпадает.
        /// </summary>
        /// <param name="content">Контент</param>
        /// <returns>Коллекция ссылок</returns>
        public List<ScLink> FindLinks(ScLinkContent content)
        {
            List<ScLink> links = new List<ScLink>();
            IntPtr adressesPtr = IntPtr.Zero;
            uint resulCount = 0;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                NativeMethods.sc_memory_find_links_with_content(this.scMemoryContext, content.ScStream, out adressesPtr, out resulCount);
            }
            Array addressesArray = NativeMethods.PtrToArray(typeof(WScAddress), adressesPtr, resulCount);
            for (uint index = 0; index < resulCount; index++)
            {
                links.Add(new ScLink(new ScAddress((WScAddress)addressesArray.GetValue(index)), this.scMemoryContext));
            }
            NativeMethods.sc_memory_free_buff(adressesPtr);
            return links;
        }

        #endregion

        #endregion


        #region Iterators

        /// <summary>
        /// Creates the iterator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ElementType t1, ElementType t2)
        {
            return new ScIterator(this.scMemoryContext, e1, t1, t2);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ElementType t1, ElementType t2, ScElement e1)
        {
            return new ScIterator(this.scMemoryContext, t1, t2, e1);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ElementType t1, ScElement e2)
        {
            return new ScIterator(this.scMemoryContext, e1, t1, e2);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ElementType t1, ElementType t2, ScElement e1, ElementType t3, ElementType t4)
        {
            return new ScIterator(this.scMemoryContext, t1, t2, e1, t3, t4);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ElementType t1, ElementType t2, ScElement e1, ElementType t3, ScElement e2)
        {
            return new ScIterator(this.scMemoryContext, t1, t2, e1, t3, e2);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ElementType t1, ElementType t2, ElementType t3, ElementType t4)
        {
            return new ScIterator(this.scMemoryContext, e1, t1, t2, t3, t4);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ElementType t1, ElementType t2, ElementType t3, ScElement e2)
        {
            return new ScIterator(this.scMemoryContext, e1, t1, t2, t3, e2);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ElementType t1, ScElement e2, ElementType t2, ElementType t3)
        {
            return new ScIterator(this.scMemoryContext, e1, t1, e2, t2, t3);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e3">The e3.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ElementType t1, ScElement e2, ElementType t2, ScElement e3)
        {
            return new ScIterator(this.scMemoryContext, e1, t1, e2, t2, e3);
        }

        #endregion

        #region Члены IDisposable

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                //unmanaged
                this.Delete();
                this.scMemoryContext = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ScMemoryContext"/> class.
        /// </summary>
        ~ScMemoryContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
