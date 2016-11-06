using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using ScEngineNet.ScExceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ScEngineNet.ScElements
{

    /// <summary>
    /// Контекст памяти.
    /// Это виртуальный интерфейс для доступа к памяти, которому устанавливается уровень доступа.
    /// Контекст с меньшим уровнем доступа не имеет доступ к элементам, созданным в контекс.
    /// Никогда не передавайте ScMemoryContext в другой поток и не используйте его из другого потока
    /// </summary>
    public class ScMemoryContext : IDisposable
    {
        private IntPtr ptrScMemoryContext;

        private const string disposalException_msg = "Был вызван метод Dispose и Ссылка на объект в памяти уже удалена";
        private const string memoryNotInitializedException_msg = "Библиотека ScMemory.Net не инициализирована";



        internal IntPtr PtrScMemoryContext
        {
            get
            {
                return ptrScMemoryContext;
            }
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
                if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }

                var context = (WScMemoryContext)Marshal.PtrToStructure(PtrScMemoryContext, typeof(WScMemoryContext));
                return (ScAccessLevels)context.AccessLevels;
            }
        }



        #region initialize & service


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
        /// Для создания экземпляра контекста, необходимо инициализировать память <see cref="ScMemoryNet.ScMemory"/>
        /// </summary>
        /// <param name="accessLevels"> Уровень доступа</param>
        public ScMemoryContext(ScAccessLevels accessLevels)
        //  : base(IntPtr.Zero, true)
        {
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            this.ptrScMemoryContext = NativeMethods.sc_memory_context_new((byte)accessLevels);
        }


        /// <summary>
        /// Получение статистики
        /// </summary>
        /// <returns>Структуру <see cref="ScStat"/>, содержащую статистику хранилища</returns>
        public ScStat GetStatistics()
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            var stat = new ScStat();
            NativeMethods.sc_memory_stat(this.PtrScMemoryContext, out stat);

            return stat;
        }

        /// <summary>
        /// Сохраняет состояние хранилища
        /// </summary>
        /// <returns>ScResult.SC_RESULT_OK, если состояние сохранить удалось</returns>
        public ScResult SaveState()
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            var result = NativeMethods.sc_memory_save(this.PtrScMemoryContext);

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
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            return NativeMethods.sc_memory_is_element(this.ptrScMemoryContext, elementAddress.WScAddress);
        }

        public ScElement GetElement(ScAddress elementAddress)
        {
            ScElement element = null;
            var elementType = ElementTypes.Unknown;
            ScResult result = NativeMethods.sc_memory_get_element_type(this.PtrScMemoryContext, elementAddress.WScAddress, out elementType);
            if (result == ScResult.SC_RESULT_OK)
            {
                if (new ScTypes(elementType).IsLink)
                {
                    element = new ScLink(elementAddress, this);
                }
                else if (new ScTypes(elementType).IsArc)
                {
                    element = new ScArc(elementAddress, this);
                }
                else if (new ScTypes(elementType).IsNode)
                {
                    element = new ScNode(elementAddress, this);
                }
            }
            return element;
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
        public ScArc CreateArc(ScElement beginElement, ScElement endElement, ScTypes arcType)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            ScArc createdArc = null;
            if (arcType.IsArc)
            {
                if (beginElement != null && endElement != null)
                {
                    if (!NativeMethods.sc_helper_check_arc(this.PtrScMemoryContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType.ElementType))
                    {
                        createdArc = new ScArc(new ScAddress(NativeMethods.sc_memory_arc_new(this.PtrScMemoryContext, arcType.ElementType, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress)), this);
                    }
                    else
                    {

                        var container = this.CreateIterator(beginElement, arcType, endElement);
                        createdArc = (ScArc)container.ElementAt(0)[1];
                    }
                }
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
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            ScArc createdArc = null;
            var scElement = this.GetElement(arcAddress);
            if (scElement.ElementType.IsArc)
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
        public bool ArcIsExist(ScElement beginElement, ScElement endElement, ScTypes arcType)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            bool result = false;
            if (beginElement != null && endElement != null)
            {
                result = NativeMethods.sc_helper_check_arc(this.PtrScMemoryContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType.ElementType);
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
        public ScNode CreateNode(ScTypes nodeType)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }
            ScNode createdNode = null;
            if (nodeType.IsNode)
            {
                createdNode = new ScNode(new ScAddress(NativeMethods.sc_memory_node_new(this.PtrScMemoryContext, nodeType.ElementType)), this);
            }
            return createdNode;
        }

        /// <summary>
        /// СОздает узел с указанным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип узла</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ScTypes nodeType, Identifier sysIdentifier)
        {
            ////if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }


            ScNode createdNode = this.FindNode(sysIdentifier);
            if (createdNode == null)
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
        public ScNode CreateNode(ScTypes nodeType, Identifier sysIdentifier, Identifier ruIdentifier)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }


            ScNode createdNode = this.CreateNode(nodeType, sysIdentifier);
            createdNode.MainIdentifiers[ScDataTypes.Instance.LanguageRu] = new ScString(ruIdentifier.Value);
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
        public ScNode CreateNode(ScTypes nodeType, Identifier sysIdentifier, Identifier ruIdentifier, Identifier enIdentifier)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }


            ScNode createdNode = this.CreateNode(nodeType, sysIdentifier, ruIdentifier);
            createdNode.MainIdentifiers[ScDataTypes.Instance.LanguageEn] = new ScString(enIdentifier.Value);
            return createdNode;
        }


        /// <summary>
        /// Ищет узел с указанным системным идентификатором
        /// </summary>
        /// <param name="identifier">Системный идентификатор</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(Identifier identifier)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            ScNode node = null;
            byte[] bytes = identifier.GetBytes();
            WScAddress address;
            NativeMethods.sc_helper_find_element_by_system_identifier(this.PtrScMemoryContext, bytes, (uint)bytes.Length, out address);
            var scAddress = new ScAddress(address);
            if (scAddress.IsValid)
            {
                node = new ScNode(new ScAddress(address), this);
            }
            return node;
        }


        /// <summary>
        /// Ищет узел по известному адресу
        /// </summary>
        /// <param name="nodeAddress">Адрес узла</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(ScAddress nodeAddress)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }


            ScNode createdNode = null;
            var scElement = this.GetElement(nodeAddress);
            createdNode = (ScNode)scElement;
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
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            ScLink createdLink = new ScLink(new ScAddress(NativeMethods.sc_memory_link_new(this.PtrScMemoryContext)), this);
            return createdLink;
        }

        /// <summary>
        ///Создает новую sc-ссылку с указанным контентом
        /// </summary>
        /// <param name="content">Контент</param>
        /// <returns>Созданная sc-ссылка</returns>
        public ScLink CreateLink(ScLinkContent content)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            List<ScLink> findLinks = this.FindLinks(content);
            if (findLinks.Count > 0)
            {
                return findLinks[0];
            }
            ScLink createdLink = this.CreateLink();
            createdLink.LinkContent = content;

            return createdLink;
        }

        internal ScLinkContent GetLinkContent(ScLink link)
        {
            IntPtr streamPtr = IntPtr.Zero;
            NativeMethods.sc_memory_get_link_content(this.PtrScMemoryContext, link.ScAddress.WScAddress, out streamPtr);
            //определяем тип ссылки
            Identifier classNodeidentifier = ScDataTypes.Instance.TypeBinary;
            var container = this.CreateIterator(ScTypes.NodeConstantClass, ScTypes.ArcAccessConstantPositivePermanent, link);
            foreach (var construction in container)
            {
                if (ScDataTypes.Instance.KeyLinkTypes.Contains(((ScNode)construction[0]).SystemIdentifier))
                {
                    var classNode = (ScNode)construction[0];
                    classNodeidentifier = classNode.SystemIdentifier;
                    break;
                }
            }

            return ScLinkContent.GetScContent(streamPtr, classNodeidentifier);
        }

       internal ScResult SetLinkContent( ScLinkContent content, ScLink link)
        {
            var result = ScResult.SC_RESULT_ERROR;
            result = NativeMethods.sc_memory_set_link_content(this.PtrScMemoryContext, link.ScAddress.WScAddress, content.ScStream);
            //delete arc from class_node

            var container = this.CreateIterator(ScTypes.NodeConstantClass, ScTypes.ArcAccessConstantPositivePermanent, link);
            foreach (var construction in container)
            {
                if (ScDataTypes.Instance.KeyLinkTypes.Contains(((ScNode)construction[0]).SystemIdentifier))
                {
                    construction[1].DeleteFromMemory();//delete arc
                    break;
                }
            }
            // create classNode
            var classNode = this.FindNode(content.ClassNodeIdentifier);
            this.CreateArc(classNode, link, ScTypes.ArcAccessConstantPositivePermanent);

            return result;
        }


        /// <summary>
        /// Ищет sc-ссылку по указанному адресу
        /// </summary>
        /// <param name="linkAddress">The link address.</param>
        /// <returns>Найденная ссылка</returns>
        public ScLink FindLink(ScAddress linkAddress)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            ScLink createdLink = null;
            var scElement = this.GetElement(linkAddress);
            if (scElement.ElementType.IsLink)
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
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            List<ScLink> links = new List<ScLink>();
            IntPtr adressesPtr = IntPtr.Zero;
            uint resulCount = 0;

            NativeMethods.sc_memory_find_links_with_content(this.PtrScMemoryContext, content.ScStream, out adressesPtr, out resulCount);

            Array addressesArray = NativeMethods.PtrToArray(typeof(WScAddress), adressesPtr, resulCount);
            for (uint index = 0; index < resulCount; index++)
            {
                links.Add(new ScLink(new ScAddress((WScAddress)addressesArray.GetValue(index)), this));
            }

            if (links.Count != 0)
            {
                NativeMethods.sc_memory_free_buff(adressesPtr);
            }
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
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScTypes t2)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            return new ScIterator(this, e1, t1, t2);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScTypes t1, ScTypes t2, ScElement e1)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            return new ScIterator(this, t1, t2, e1);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScElement e2)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

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
        public ScIterator CreateIterator(ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScTypes t4)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

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
        public ScIterator CreateIterator(ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScElement e2)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

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
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScTypes t4)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

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
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScElement e2)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

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
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScTypes t3)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

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
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScElement e3)
        {
            if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (ScMemoryContext.IsMemoryInitialized() != true) { throw new ScMemoryNotInitializeException(memoryNotInitializedException_msg); }

            return new ScIterator(this, e1, t1, e2, t2, e3);
        }


        #endregion



        #region IDisposal
        private bool disposed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ScMemoryContext"/> is disposed.
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
            //    Console.WriteLine("call Dispose({0}) ScContext with {1}", disposing, this.ptrScMemoryContext);


            if (!disposed && ScMemoryContext.IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                NativeMethods.sc_memory_context_free(this.ptrScMemoryContext);
                this.ptrScMemoryContext = IntPtr.Zero;


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
        /// Finalizes an instance of the <see cref="ScMemoryContext"/> class.
        /// </summary>
        ~ScMemoryContext()
        {
            Dispose(false);
        }
        #endregion


    }
}
