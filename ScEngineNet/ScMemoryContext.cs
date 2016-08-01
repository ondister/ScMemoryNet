using ScEngineNet.NativeElements;
using ScEngineNet.NetHelpers;
using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace ScEngineNet
{
    /// <summary>
    /// Контекст памяти.
    /// Это виртуальный интерфейс для доступа к памяти, которому устанавливается уровень доступа.
    /// Контекст с меньшим уровнем доступа не имеет доступ к элементам, созданным в контекс.
    /// </summary>
    public class ScMemoryContext:SafeHandle
    {
        private IntPtr ptrScMemoryContext;

        internal IntPtr PtrScMemoryContext
        {
            get { return ptrScMemoryContext; }
        }

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
                var context = (WScMemoryContext)Marshal.PtrToStructure(PtrScMemoryContext, typeof(WScMemoryContext));
                return (ScAccessLevels)context.AccessLevels;
            }
        }



        #region initialize


        /// <summary>
        /// Определяет, инициализирована ли память
        /// </summary>
        /// <returns>Вернет True, если память уже инициализирована</returns>
        public static bool IsMemoryInitialized()
        {
            return NativeMethods.sc_memory_is_initialized();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScMemoryContext"/> class.
        /// Для создания экземпляра контекста, необходимо инициализировать память <see cref="ScMemory"/>
        /// </summary>
        /// <param name="accessLevels"> Уровень доступа</param>
        public ScMemoryContext(ScAccessLevels accessLevels)
            : base(IntPtr.Zero, true)
        {
            GC.KeepAlive(this);
            if (ScMemoryContext.IsMemoryInitialized())
            {
                this.ptrScMemoryContext = NativeMethods.sc_memory_context_new((byte)accessLevels);
                
            }

        }

        /// <summary>
        /// Удаляет контекст
        /// Этот метод не обязательно вызывать, так как он вызывается сборщиком мусора
        /// </summary>
        public void Delete()
        {
            if (ScMemoryContext.IsMemoryInitialized())
            {
               NativeMethods.sc_memory_context_free(this.PtrScMemoryContext);
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
                NativeMethods.sc_memory_stat(this.PtrScMemoryContext, out stat);
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
                result = NativeMethods.sc_memory_save(this.PtrScMemoryContext);
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
            return Identifier.GetUnique(this, node);
        }

        /// <summary>
        /// Создает уникальный идентификатор для узла на основе его адреса и преффикса
        /// </summary>
        /// <param name="prefix">Преффикс</param>
        /// <param name="node">Узел</param>
        /// <returns>Уникальный идентификатр узла</returns>
        public Identifier CreateUniqueIdentifier(string prefix, ScNode node)
        {
            return Identifier.GetUnique(this, prefix, node);
        }

        /// <summary>
        /// Определяет, существует ли элемент с указанным адресом
        /// </summary>
        /// <param name="elementAddress">Адрес элемента</param>
        /// <returns></returns>
        public bool IsElementExist(ScAddress elementAddress)
        {
            return ScMemorySafeMethods.IsElementExist(this, elementAddress);
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
            ScArc createdArc = null;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdArc = ScMemorySafeMethods.CreateArc(this, arcType, beginElement, endElement);
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
            ScArc createdArc = null;
            var scElement = ScMemorySafeMethods.GetElement(arcAddress.WScAddress, this);
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
                result = NativeMethods.sc_helper_check_arc(this.PtrScMemoryContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType);
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
            ScNode createdNode = null;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdNode = new ScNode(new ScAddress(NativeMethods.sc_memory_node_new(this.PtrScMemoryContext, nodeType)), this);
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
            return ScMemorySafeMethods.FindNode(this, identifier);
        }

        /// <summary>
        /// Ищет узел по известному адресу
        /// </summary>
        /// <param name="nodeAddress">Адрес узла</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(ScAddress nodeAddress)
        {
            ScNode createdNode = null;
            var scElement = ScMemorySafeMethods.GetElement(nodeAddress.WScAddress, this);
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
            var createdLink = new ScLink(ScAddress.Invalid, this);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdLink = new ScLink(new ScAddress(NativeMethods.sc_memory_link_new(this.PtrScMemoryContext)), this);
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
            var createdLink = new ScLink(ScAddress.Invalid, this);
            var scElement = ScMemorySafeMethods.GetElement(linkAddress.WScAddress, this);
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
                NativeMethods.sc_memory_find_links_with_content(this.PtrScMemoryContext, content.ScStream, out adressesPtr, out resulCount);
            }
            Array addressesArray = NativeMethods.PtrToArray(typeof(WScAddress), adressesPtr, resulCount);
            for (uint index = 0; index < resulCount; index++)
            {
                links.Add(new ScLink(new ScAddress((WScAddress)addressesArray.GetValue(index)), this));
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
            return new ScIterator(this, e1, t1, t2);
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
            return new ScIterator(this, t1, t2, e1);
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
            return new ScIterator(this, e1, t1, e2);
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
            return new ScIterator(this, t1, t2, e1, t3, t4);
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
            return new ScIterator(this, t1, t2, e1, t3, e2);
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
            return new ScIterator(this, e1, t1, t2, t3, t4);
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
            return new ScIterator(this, e1, t1, t2, t3, e2);
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
            return new ScIterator(this, e1, t1, e2, t2, t3);
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
            return new ScIterator(this, e1, t1, e2, t2, e3);
        }

        #endregion

        #region Члены SafeHandle        
        /// <summary>
        /// При переопределении в производном классе получает значение, показывающее, допустимо ли значение дескриптора.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override bool IsInvalid
        {
            get { return this.PtrScMemoryContext == IntPtr.Zero; }
        }
        /// <summary>
        /// При переопределении в производном классе выполняет код, необходимый для освобождения дескриптора.
        /// </summary>
        /// <returns>
        /// Значение true, если дескриптор освобождается успешно, в противном случае, в случае катастрофической ошибки — значение  false.В таком случае создается управляющий помощник по отладке releaseHandleFailed MDA.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            this.Delete();
            return !IsInvalid;
        }
     
        #endregion

     
    }
}
