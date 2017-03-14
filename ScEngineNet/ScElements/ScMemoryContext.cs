using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Контекст памяти.
    ///     Это виртуальный интерфейс для доступа к памяти, которому устанавливается уровень доступа.
    ///     Контекст с меньшим уровнем доступа не имеет доступ к элементам, созданным в контекс.
    ///     Никогда не передавайте ScMemoryContext в другой поток и не используйте его из другого потока
    /// </summary>
    public class ScMemoryContext : IDisposable
    {
        private const string disposalExceptionMsg = "Был вызван метод Dispose и Ссылка на объект в памяти уже удалена";
        private const string memoryNotInitializedExceptionMsg = "Библиотека ScMemory.Net не инициализирована";
        internal IntPtr PtrScMemoryContext { get; private set; }

        /// <summary>
        ///     Возвращает установленный для контекста уровень доступа
        /// </summary>
        /// <value>
        ///     Уровень доступа <see cref="ScAccessLevels" />
        /// </value>
        public ScAccessLevels AccessLevel
        {
            get
            {
                if (PtrScMemoryContext == IntPtr.Zero)
                {
                    throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
                }

                var context = (WScMemoryContext) Marshal.PtrToStructure(PtrScMemoryContext, typeof (WScMemoryContext));
                return (ScAccessLevels) context.AccessLevels;
            }
        }

        #region initialize & service

        /// <summary>
        ///     Определяет, инициализирована ли память
        /// </summary>
        /// <returns>Вернет True, если память уже инициализирована</returns>
        public static bool IsMemoryInitialized()
        {
            return NativeMethods.sc_memory_is_initialized();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScMemoryContext" /> class.
        ///     Для создания экземпляра контекста, необходимо инициализировать память <see cref="ScMemory" />
        /// </summary>
        /// <param name="accessLevels"> Уровень доступа</param>
        public ScMemoryContext(ScAccessLevels accessLevels)
            //  : base(IntPtr.Zero, true)
        {
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            PtrScMemoryContext = NativeMethods.sc_memory_context_new((byte) accessLevels);
        }


        /// <summary>
        ///     Получение статистики
        /// </summary>
        /// <returns>Структуру <see cref="ScStat" />, содержащую статистику хранилища</returns>
        public ScStat GetStatistics()
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            ScStat stat;
            NativeMethods.sc_memory_stat(PtrScMemoryContext, out stat);

            return stat;
        }

        /// <summary>
        ///     Сохраняет состояние хранилища
        /// </summary>
        /// <returns>ScResult.SC_RESULT_OK, если состояние сохранить удалось</returns>
        public ScResult SaveState()
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            var result = NativeMethods.sc_memory_save(PtrScMemoryContext);

            return result;
        }

        #endregion

        #region Elements

        #region Common

        /// <summary>
        ///     Создает уникальный идентификатор для узла на основе его адреса
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Уникальный идентификатр узла</returns>
        public Identifier CreateUniqueIdentifier(ScNode node)
        {
            return Identifier.GetUnique(this, node);
        }

        /// <summary>
        ///     Создает уникальный идентификатор для узла на основе его адреса и преффикса
        /// </summary>
        /// <param name="prefix">Преффикс</param>
        /// <param name="node">Узел</param>
        /// <returns>Уникальный идентификатр узла</returns>
        public Identifier CreateUniqueIdentifier(string prefix, ScNode node)
        {
            return Identifier.GetUnique(this, prefix, node);
        }

        /// <summary>
        ///     Определяет, существует ли элемент с указанным адресом
        /// </summary>
        /// <param name="elementAddress">Адрес элемента</param>
        /// <returns></returns>
        public bool IsElementExist(ScAddress elementAddress)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return NativeMethods.sc_memory_is_element(PtrScMemoryContext, elementAddress.WScAddress);
        }

        public ScElement GetElement(ScAddress elementAddress)
        {
            ScElement element = null;
            ElementTypes elementType;
            var result = NativeMethods.sc_memory_get_element_type(PtrScMemoryContext, elementAddress.WScAddress,
                out elementType);
            if (result == ScResult.ScResultOk)
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
        ///     Создает новую дугу в хранилище.
        ///     Если такая дуга уже ест в хранилище, дубликат не создается, и возвращается имеющаяся дуга
        /// </summary>
        /// <param name="beginElement">Начальный элемент</param>
        /// <param name="endElement">Конечный элемент</param>
        /// <param name="arcType">Тип дуги </param>
        /// <returns>Созданную дугу</returns>
        public ScArc CreateArc(ScElement beginElement, ScElement endElement, ScTypes arcType)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            ScArc createdArc = null;
            if (arcType.IsArc)
            {
                if (beginElement != null && endElement != null)
                {
                    if (
                        !NativeMethods.sc_helper_check_arc(PtrScMemoryContext, beginElement.ScAddress.WScAddress,
                            endElement.ScAddress.WScAddress, arcType.ElementType))
                    {
                        createdArc =
                            new ScArc(
                                new ScAddress(NativeMethods.sc_memory_arc_new(PtrScMemoryContext, arcType.ElementType,
                                    beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress)), this);
                    }
                    else
                    {
                        var container = CreateIterator(beginElement, arcType, endElement);
                        createdArc = (ScArc) container.ElementAt(0)[1];
                    }
                }
            }
            return createdArc;
        }


        /// <summary>
        ///     Находит дугу по указанному адресу
        /// </summary>
        /// <param name="arcAddress">Адрес дуги</param>
        /// <returns>sc-дуга</returns>
        public ScArc FindArc(ScAddress arcAddress)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            ScArc createdArc = null;
            var scElement = GetElement(arcAddress);
            if (scElement.ElementType.IsArc)
            {
                createdArc = (ScArc) scElement;
            }
            return createdArc;
        }

        /// <summary>
        ///     Определяет, существует ли дуга с указанными параметрами
        /// </summary>
        /// <param name="beginElement">Начальный элемент</param>
        /// <param name="endElement">Конечный элемент</param>
        /// <param name="arcType">Тип дуги</param>
        /// <returns>True, если дуга существует</returns>
        public bool ArcIsExist(ScElement beginElement, ScElement endElement, ScTypes arcType)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            var result = false;
            if (beginElement != null && endElement != null)
            {
                result = NativeMethods.sc_helper_check_arc(PtrScMemoryContext, beginElement.ScAddress.WScAddress,
                    endElement.ScAddress.WScAddress, arcType.ElementType);
            }
            return result;
        }

        #endregion

        #region Nodes

        /// <summary>
        ///     Создает новый узел указанного типа
        /// </summary>
        /// <param name="nodeType">Тип узла</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ScTypes nodeType)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }
            ScNode createdNode = null;
            if (nodeType.IsNode)
            {
                createdNode =
                    new ScNode(
                        new ScAddress(NativeMethods.sc_memory_node_new(PtrScMemoryContext, nodeType.ElementType)), this);
            }
            return createdNode;
        }

        /// <summary>
        ///     СОздает узел с указанным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип узла</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ScTypes nodeType, Identifier sysIdentifier)
        {
            ////if (this.ptrScMemoryContext == IntPtr.Zero) { throw new ObjectDisposedException(this.ToString(), disposalException_msg); }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }


            var createdNode = FindNode(sysIdentifier);
            if (createdNode == null)
            {
                createdNode = CreateNode(nodeType);
                createdNode.SystemIdentifier = sysIdentifier;
            }
            return createdNode;
        }

        /// <summary>
        ///     Создает узел с указанным системным идентификатором и основным русскоязычным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип уза</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <param name="ruIdentifier">Основной русскоязычный идентификатор</param>
        /// <returns>Созданный узел</returns>
        public ScNode CreateNode(ScTypes nodeType, Identifier sysIdentifier, Identifier ruIdentifier)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }


            var createdNode = CreateNode(nodeType, sysIdentifier);
            createdNode.MainIdentifiers[ScDataTypes.Instance.LanguageRu] = new ScString(ruIdentifier.Value);
            return createdNode;
        }

        /// <summary>
        ///     Создает узел с указанным системным идентификатором и основным русскоязычным и англоязычным идентификатором
        /// </summary>
        /// <param name="nodeType">Тип уза</param>
        /// <param name="sysIdentifier">Системный идентификатор</param>
        /// <param name="ruIdentifier">Основной русскоязычный идентификатор</param>
        /// <param name="enIdentifier">Англоязычный идентификатор</param>
        /// <returns>
        ///     Созданный узел
        /// </returns>
        public ScNode CreateNode(ScTypes nodeType, Identifier sysIdentifier, Identifier ruIdentifier,
            Identifier enIdentifier)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }


            var createdNode = CreateNode(nodeType, sysIdentifier, ruIdentifier);
            createdNode.MainIdentifiers[ScDataTypes.Instance.LanguageEn] = new ScString(enIdentifier.Value);
            return createdNode;
        }


        /// <summary>
        ///     Ищет узел с указанным системным идентификатором
        /// </summary>
        /// <param name="identifier">Системный идентификатор</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(Identifier identifier)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            ScNode node = null;
            var bytes = identifier.GetBytes();
            WScAddress address;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, bytes, (uint) bytes.Length,
                out address);
            var scAddress = new ScAddress(address);
            if (scAddress.IsValid)
            {
                node = new ScNode(new ScAddress(address), this);
            }
            return node;
        }


        /// <summary>
        ///     Ищет узел по известному адресу
        /// </summary>
        /// <param name="nodeAddress">Адрес узла</param>
        /// <returns>Найденный узел</returns>
        public ScNode FindNode(ScAddress nodeAddress)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }


            var scElement = GetElement(nodeAddress);
            var createdNode = (ScNode) scElement;
            return createdNode;
        }

        #endregion

        #region Links

        /// <summary>
        ///     Создает новую sc-ссылку
        /// </summary>
        /// <returns>Созданная sc-ссылка</returns>
        public ScLink CreateLink()
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            var createdLink = new ScLink(new ScAddress(NativeMethods.sc_memory_link_new(PtrScMemoryContext)), this);
            return createdLink;
        }

        /// <summary>
        ///     Создает новую sc-ссылку с указанным контентом
        /// </summary>
        /// <param name="content">Контент</param>
        /// <returns>Созданная sc-ссылка</returns>
        public ScLink CreateLink(ScLinkContent content)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            var findLinks = FindLinks(content);
            if (findLinks.Count > 0)
            {
                return findLinks[0];
            }
            var createdLink = CreateLink();
            createdLink.LinkContent = content;

            return createdLink;
        }

        internal ScLinkContent GetLinkContent(ScLink link)
        {
            IntPtr streamPtr;
            NativeMethods.sc_memory_get_link_content(PtrScMemoryContext, link.ScAddress.WScAddress, out streamPtr);
            //определяем тип ссылки
            var classNodeidentifier = ScDataTypes.Instance.TypeBinary;
            var container = CreateIterator(ScTypes.NodeConstantClass, ScTypes.ArcAccessConstantPositivePermanent, link);
            foreach (var construction in container)
            {
                if (ScDataTypes.Instance.KeyLinkTypes.Contains(((ScNode) construction[0]).SystemIdentifier))
                {
                    var classNode = (ScNode) construction[0];
                    classNodeidentifier = classNode.SystemIdentifier;
                    break;
                }
            }

            return ScLinkContent.GetScContent(streamPtr, classNodeidentifier);
        }

        internal ScResult SetLinkContent(ScLinkContent content, ScLink link)
        {
            var result = NativeMethods.sc_memory_set_link_content(PtrScMemoryContext, link.ScAddress.WScAddress,
                content.ScStream);
            //delete arc from class_node

            var container = CreateIterator(ScTypes.NodeConstantClass, ScTypes.ArcAccessConstantPositivePermanent, link);
            foreach (var construction in container)
            {
                if (ScDataTypes.Instance.KeyLinkTypes.Contains(((ScNode) construction[0]).SystemIdentifier))
                {
                    construction[1].DeleteFromMemory(); //delete arc
                    break;
                }
            }
            // create classNode
            var classNode = FindNode(content.ClassNodeIdentifier);
            CreateArc(classNode, link, ScTypes.ArcAccessConstantPositivePermanent);

            return result;
        }


        /// <summary>
        ///     Ищет sc-ссылку по указанному адресу
        /// </summary>
        /// <param name="linkAddress">The link address.</param>
        /// <returns>Найденная ссылка</returns>
        public ScLink FindLink(ScAddress linkAddress)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            ScLink createdLink = null;
            var scElement = GetElement(linkAddress);
            if (scElement.ElementType.IsLink)
            {
                createdLink = (ScLink) scElement;
            }
            return createdLink;
        }

        /// <summary>
        ///     Ищет все ссылки, содержащие указанный контент.
        ///     Возвращаются ссылки, контент которых полностью совпадает.
        /// </summary>
        /// <param name="content">Контент</param>
        /// <returns>Коллекция ссылок</returns>
        public List<ScLink> FindLinks(ScLinkContent content)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            var links = new List<ScLink>();
            IntPtr adressesPtr;
            uint resulCount;

            NativeMethods.sc_memory_find_links_with_content(PtrScMemoryContext, content.ScStream, out adressesPtr,
                out resulCount);

            var addressesArray = NativeMethods.PtrToArray(typeof (WScAddress), adressesPtr, resulCount);
            for (uint index = 0; index < resulCount; index++)
            {
                links.Add(new ScLink(new ScAddress((WScAddress) addressesArray.GetValue(index)), this));
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
        ///     Creates the iterator.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScTypes t2)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, e1, t1, t2);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScTypes t1, ScTypes t2, ScElement e1)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, t1, t2, e1);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScElement e2)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, e1, t1, e2);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScTypes t4)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, t1, t2, e1, t3, t4);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e1">The e1.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScTypes t1, ScTypes t2, ScElement e1, ScTypes t3, ScElement e2)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, t1, t2, e1, t3, e2);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="t4">The t4.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScTypes t4)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, e1, t1, t2, t3, t4);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <param name="e2">The e2.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScTypes t2, ScTypes t3, ScElement e2)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, e1, t1, t2, t3, e2);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="t3">The t3.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScTypes t3)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, e1, t1, e2, t2, t3);
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="e2">The e2.</param>
        /// <param name="t2">The t2.</param>
        /// <param name="e3">The e3.</param>
        /// <returns></returns>
        public ScIterator CreateIterator(ScElement e1, ScTypes t1, ScElement e2, ScTypes t2, ScElement e3)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
            {
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            }
            if (IsMemoryInitialized() != true)
            {
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);
            }

            return new ScIterator(this, e1, t1, e2, t2, e3);
        }

        #endregion

        #region IDisposal

        /// <summary>
        ///     Gets a value indicating whether this <see cref="ScMemoryContext" /> is disposed.
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
            //    Console.WriteLine("call Dispose({0}) ScContext with {1}", disposing, this.ptrScMemoryContext);


            if (!Disposed && IsMemoryInitialized())
            {
                // Dispose of resources held by this instance.
                NativeMethods.sc_memory_context_free(PtrScMemoryContext);
                PtrScMemoryContext = IntPtr.Zero;


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
        ///     Finalizes an instance of the <see cref="ScMemoryContext" /> class.
        /// </summary>
        ~ScMemoryContext()
        {
            Dispose(false);
        }

        #endregion
    }
}