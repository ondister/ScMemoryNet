using System;
using System.Collections.Generic;
using System.Linq;
using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScExceptions;

namespace ScEngineNet.ScElements
{

    /// <summary>
    /// Пример создание контекста для собственной модели узлов в sc-памяти
    /// </summary>
    public class SapfirContext : ScMemoryContext
    {
        public SapfirContext(ScAccessLevels accessLevels) : base(accessLevels)
        {
        }


        /// <summary>
        ///     Поиск узла по части его основного идентификатора  без зависимости от регистра
        /// </summary>
        /// <param name="subString">Искомая подстрока</param>
        /// <param name="classIdentifier">Идентификатор языка</param>
        /// <param name="nrelIdentifier">Идентификатор узла отношения</param>
        /// <returns></returns>
        public List<ScNode> FindNodesBySubString(string subString, Identifier classIdentifier,
            Identifier nrelIdentifier)
        {
            if (PtrScMemoryContext == IntPtr.Zero)
                throw new ObjectDisposedException(ToString(), disposalExceptionMsg);
            if (IsMemoryInitialized() != true)
                throw new ScMemoryNotInitializeException(memoryNotInitializedExceptionMsg);

            var result = new List<ScNode>();

            var invariantString = subString.ToLowerInvariant();

            var classIdentifierNode = FindNode(classIdentifier);
            var nrelMainIdentifierNode = FindNode(nrelIdentifier);

            if (classIdentifierNode != null && nrelMainIdentifierNode != null)
            {
                var classIterator = CreateIterator(classIdentifierNode, ScTypes.ArcAccessConstantPositivePermanent,
                    ScTypes.NodeConstant);
                foreach (var classConstruction in classIterator)
                {
                    var node = classConstruction[2] as ScNode;

                    var linkIterator = CreateIterator(node, ScTypes.ArcCommon, ScTypes.Link,
                        ScTypes.ArcAccessConstantPositivePermanent, nrelMainIdentifierNode);
                    foreach (var linkConstruction in linkIterator)
                    {
                        var link = linkConstruction[2] as ScLink;

                        IntPtr streamPtr;
                        NativeMethods.sc_memory_get_link_content(PtrScMemoryContext, link.ScAddress.WScAddress,
                            out streamPtr);
                        var bytes = GetBytes(streamPtr);
                        if (bytes != null)
                        {
                            var linkContent = ScEngineNet.TextEncoding.GetString(bytes);
                            if (linkContent.ToLowerInvariant().Contains(invariantString) && !result.Contains(node))
                                result.Add(node);
                        }
                    }
                }
            }

            return result;
        }

        private byte[] GetBytes(IntPtr stream)
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
                        return buffer;
                }
            }
            return null;
        }


        #region Поиск тегов по типам данным
        /// <summary>
        ///     Поиск по строковым типам данных
        /// </summary>
        /// <param name="classIdentifier"> Подходят только строковые</param>
        /// <param name="tagRelation"></param>
        /// <param name="textString"></param>
        /// <param name="compareType"></param>
        /// <returns></returns>
        public List<ScNode> FindTagByString(Identifier classIdentifier, Identifier tagRelation, string textString,
            ComparerTypeEnum compareType)
        {
            var result = new List<ScNode>();

            var invariantString = textString.ToLowerInvariant();

            #region Находим адреса параметров

            var classBytes = classIdentifier.GetBytes();
            WScAddress classAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, classBytes,
                (uint) classBytes.Length,
                out classAddress);

            var relationBytes = tagRelation.GetBytes();
            WScAddress relationAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, relationBytes,
                (uint) relationBytes.Length,
                out relationAddress);

            #endregion

            #region Находим все конструкции, где участвует тег

            var p1 = new ScIteratorParam {Address = relationAddress, IsType = false};
            var p2 = new ScIteratorParam {IsType = true, Type = ElementTypes.PositiveConstantPermanentAccessArcC};
            var p3 = new ScIteratorParam {IsType = true, Type = ElementTypes.ConstantCommonArcC};
            var arcIterator =
                NativeMethods.sc_iterator3_new(PtrScMemoryContext, ScIterator3Type.sc_iterator3_f_a_a, p1, p2, p3);
            while (NativeMethods.sc_iterator3_next(arcIterator))
            {
                var arcAdress = NativeMethods.sc_iterator3_value(arcIterator, 2);
                WScAddress linkAddres;
                if (NativeMethods.sc_memory_get_arc_end(PtrScMemoryContext, arcAdress, out linkAddres) ==
                    ScResult.ScResultOk)
                {
                    //далее 3 варианта событий: либо это линка, либо нода, либо тупла. Нас интересует только линка
                    ElementTypes type;
                    NativeMethods.sc_memory_get_element_type(PtrScMemoryContext, linkAddres, out type);
                    if (type == ElementTypes.LinkA)
                    {
                        var par1 = new ScIteratorParam {Address = classAddress, IsType = false};
                        var par2 = new ScIteratorParam
                        {
                            IsType = true,
                            Type = ElementTypes.PositiveConstantPermanentAccessArcC
                        };
                        var par3 = new ScIteratorParam {Address = linkAddres, IsType = false};
                        var iterator = NativeMethods.sc_iterator3_new(PtrScMemoryContext,
                            ScIterator3Type.sc_iterator3_f_a_f, par1, par2, par3);

                        while (NativeMethods.sc_iterator3_next(iterator))
                        {
                            var linkAdress = NativeMethods.sc_iterator3_value(iterator, 2);
                            IntPtr streamPtr;
                            NativeMethods.sc_memory_get_link_content(PtrScMemoryContext, linkAdress, out streamPtr);
                            var linkBytes = GetBytes(streamPtr);
                            if (linkBytes != null)
                            {
                                var linkContent = ScEngineNet.TextEncoding.GetString(linkBytes);
                                var isEquel = false;

                                if (compareType == ComparerTypeEnum.PartOfEqual)
                                    isEquel =
                                        linkContent.IndexOf(invariantString,
                                            StringComparison.InvariantCultureIgnoreCase) >= 0;
                                if (compareType == ComparerTypeEnum.Equal)
                                    isEquel = linkContent.Equals(invariantString,
                                        StringComparison.InvariantCultureIgnoreCase);

                                if (isEquel)
                                {
                                    WScAddress nodeAddress;
                                    if (NativeMethods.sc_memory_get_arc_begin(PtrScMemoryContext, arcAdress,
                                            out nodeAddress) == ScResult.ScResultOk)
                                    {
                                        var node = new ScNode(new ScAddress(nodeAddress), this);
                                        if (node.ElementType == ScTypes.NodeConstant ||
                                            node.ElementType ==
                                            ScTypes.NodeConstantTuple) //здесь определяем, какие в итоге узлы добавлять
                                            result.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            return result;
        }

        public List<ScNode> FindTagByDate(Identifier classIdentifier, Identifier tagRelation, DateTime startDateTime,
            DateTime endDateTime, ComparerTypeEnum compareType)
        {
            var result = new List<ScNode>();

            #region Находим адреса параметров

            var classBytes = classIdentifier.GetBytes();
            WScAddress classAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, classBytes,
                (uint) classBytes.Length,
                out classAddress);

            var relationBytes = tagRelation.GetBytes();
            WScAddress relationAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, relationBytes,
                (uint) relationBytes.Length,
                out relationAddress);

            #endregion

            #region Находим все конструкции, где участвует тег

            var p1 = new ScIteratorParam {Address = relationAddress, IsType = false};
            var p2 = new ScIteratorParam {IsType = true, Type = ElementTypes.PositiveConstantPermanentAccessArcC};
            var p3 = new ScIteratorParam {IsType = true, Type = ElementTypes.ConstantCommonArcC};
            var arcIterator =
                NativeMethods.sc_iterator3_new(PtrScMemoryContext, ScIterator3Type.sc_iterator3_f_a_a, p1, p2, p3);
            while (NativeMethods.sc_iterator3_next(arcIterator))
            {
                var arcAdress = NativeMethods.sc_iterator3_value(arcIterator, 2);
                WScAddress linkAddres;
                if (NativeMethods.sc_memory_get_arc_end(PtrScMemoryContext, arcAdress, out linkAddres) ==
                    ScResult.ScResultOk)
                {
                    //далее 3 варианта событий: либо это линка, либо нода, либо тупла. Нас интересует только линка
                    ElementTypes type;
                    NativeMethods.sc_memory_get_element_type(PtrScMemoryContext, linkAddres, out type);
                    if (type == ElementTypes.LinkA)
                    {
                        var par1 = new ScIteratorParam {Address = classAddress, IsType = false};
                        var par2 = new ScIteratorParam
                        {
                            IsType = true,
                            Type = ElementTypes.PositiveConstantPermanentAccessArcC
                        };
                        var par3 = new ScIteratorParam {Address = linkAddres, IsType = false};
                        var iterator = NativeMethods.sc_iterator3_new(PtrScMemoryContext,
                            ScIterator3Type.sc_iterator3_f_a_f, par1, par2, par3);

                        while (NativeMethods.sc_iterator3_next(iterator))
                        {
                            var linkAdress = NativeMethods.sc_iterator3_value(iterator, 2);
                            IntPtr streamPtr;
                            NativeMethods.sc_memory_get_link_content(PtrScMemoryContext, linkAdress, out streamPtr);
                            var linkBytes = GetBytes(streamPtr);
                            if (linkBytes != null)
                            {
                                var linkContent = ScDateTime.ToDateTime(linkBytes);
                                var isEqual = false;

                                switch (compareType)
                                {
                                    case ComparerTypeEnum.UnknownWrapper:
                                        break;
                                    case ComparerTypeEnum.Equal:
                                        isEqual = startDateTime.Equals(linkContent);
                                        break;
                                    case ComparerTypeEnum.PartOfEqual:
                                        break;
                                    case ComparerTypeEnum.Less:
                                        isEqual = linkContent <= startDateTime;
                                        break;
                                    case ComparerTypeEnum.More:
                                        isEqual = linkContent >= startDateTime;
                                        break;
                                    case ComparerTypeEnum.Between:
                                        isEqual = linkContent >= startDateTime && linkContent <= endDateTime;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(compareType), compareType, null);
                                }

                                if (isEqual)
                                {
                                    WScAddress nodeAddress;
                                    if (NativeMethods.sc_memory_get_arc_begin(PtrScMemoryContext, arcAdress,
                                            out nodeAddress) == ScResult.ScResultOk)
                                    {
                                        var node = new ScNode(new ScAddress(nodeAddress), this);
                                        if (node.ElementType == ScTypes.NodeConstant ||
                                            node.ElementType ==
                                            ScTypes.NodeConstantTuple) //здесь определяем, какие в итоге узлы добавлять
                                            result.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            return result;
        }
        
     
        public List<ScNode> FindTagByNumeric(Identifier classIdentifier, Identifier tagRelation, byte[] startValue,
            byte[] endValue, ComparerTypeEnum compareType)
        {
            var result = new List<ScNode>();

            #region Находим адреса параметров

            var classBytes = classIdentifier.GetBytes();
            WScAddress classAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, classBytes,
                (uint)classBytes.Length,
                out classAddress);

            var relationBytes = tagRelation.GetBytes();
            WScAddress relationAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, relationBytes,
                (uint)relationBytes.Length,
                out relationAddress);

            #endregion

            #region Находим все конструкции, где участвует тег

            var p1 = new ScIteratorParam { Address = relationAddress, IsType = false };
            var p2 = new ScIteratorParam { IsType = true, Type = ElementTypes.PositiveConstantPermanentAccessArcC };
            var p3 = new ScIteratorParam { IsType = true, Type = ElementTypes.ConstantCommonArcC };
            var arcIterator =
                NativeMethods.sc_iterator3_new(PtrScMemoryContext, ScIterator3Type.sc_iterator3_f_a_a, p1, p2, p3);
            while (NativeMethods.sc_iterator3_next(arcIterator))
            {
                var arcAdress = NativeMethods.sc_iterator3_value(arcIterator, 2);
                WScAddress linkAddres;
                if (NativeMethods.sc_memory_get_arc_end(PtrScMemoryContext, arcAdress, out linkAddres) ==
                    ScResult.ScResultOk)
                {
                    //далее 3 варианта событий: либо это линка, либо нода, либо тупла. Нас интересует только линка
                    ElementTypes type;
                    NativeMethods.sc_memory_get_element_type(PtrScMemoryContext, linkAddres, out type);
                    if (type == ElementTypes.LinkA)
                    {
                        var par1 = new ScIteratorParam { Address = classAddress, IsType = false };
                        var par2 = new ScIteratorParam
                        {
                            IsType = true,
                            Type = ElementTypes.PositiveConstantPermanentAccessArcC
                        };
                        var par3 = new ScIteratorParam { Address = linkAddres, IsType = false };
                        var iterator = NativeMethods.sc_iterator3_new(PtrScMemoryContext,
                            ScIterator3Type.sc_iterator3_f_a_f, par1, par2, par3);

                        while (NativeMethods.sc_iterator3_next(iterator))
                        {
                            var linkAdress = NativeMethods.sc_iterator3_value(iterator, 2);
                            IntPtr streamPtr;
                            NativeMethods.sc_memory_get_link_content(PtrScMemoryContext, linkAdress, out streamPtr);
                            var linkBytes = GetBytes(streamPtr);
                            if (linkBytes != null)
                            {
                                var linkContent = ScLinkContent.ToDouble(linkBytes);
                                var startDoubleValue = ScLinkContent.ToDouble(startValue);
                                var isEqual = false;

                                switch (compareType)
                                {
                                    case ComparerTypeEnum.UnknownWrapper:
                                        break;
                                    case ComparerTypeEnum.Equal:
                                        isEqual = (linkContent == startDoubleValue);
                                        break;
                                    case ComparerTypeEnum.PartOfEqual:
                                        break;
                                    case ComparerTypeEnum.Less:
                                        isEqual = linkContent <= startDoubleValue;
                                        break;
                                    case ComparerTypeEnum.More:
                                        isEqual = linkContent >= startDoubleValue;
                                        break;
                                    case ComparerTypeEnum.Between:
                                        var endDoubleValue = ScLinkContent.ToDouble(endValue);
                                        isEqual = (linkContent >= startDoubleValue && linkContent <= endDoubleValue);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(compareType), compareType, null);
                                }

                                if (isEqual)
                                {
                                    WScAddress nodeAddress;
                                    if (NativeMethods.sc_memory_get_arc_begin(PtrScMemoryContext, arcAdress,
                                            out nodeAddress) == ScResult.ScResultOk)
                                    {
                                        var node = new ScNode(new ScAddress(nodeAddress), this);
                                        if (node.ElementType == ScTypes.NodeConstant ||
                                            node.ElementType ==
                                            ScTypes.NodeConstantTuple) //здесь определяем, какие в итоге узлы добавлять
                                            result.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            return result;
        }


        public List<ScNode> FindTagByTimeInterval(Identifier classIdentifier, Identifier tagRelation, byte[] startValue,
            byte[] endValue, ComparerTypeEnum compareType)
        {
            var result = new List<ScNode>();

            #region Находим адреса параметров

            var classBytes = classIdentifier.GetBytes();
            WScAddress classAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, classBytes,
                (uint)classBytes.Length,
                out classAddress);

            var relationBytes = tagRelation.GetBytes();
            WScAddress relationAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, relationBytes,
                (uint)relationBytes.Length,
                out relationAddress);

            #endregion

            #region Находим все конструкции, где участвует тег

            var p1 = new ScIteratorParam { Address = relationAddress, IsType = false };
            var p2 = new ScIteratorParam { IsType = true, Type = ElementTypes.PositiveConstantPermanentAccessArcC };
            var p3 = new ScIteratorParam { IsType = true, Type = ElementTypes.ConstantCommonArcC };
            var arcIterator =
                NativeMethods.sc_iterator3_new(PtrScMemoryContext, ScIterator3Type.sc_iterator3_f_a_a, p1, p2, p3);
            while (NativeMethods.sc_iterator3_next(arcIterator))
            {
                var arcAdress = NativeMethods.sc_iterator3_value(arcIterator, 2);
                WScAddress linkAddres;
                if (NativeMethods.sc_memory_get_arc_end(PtrScMemoryContext, arcAdress, out linkAddres) ==
                    ScResult.ScResultOk)
                {
                    //далее 3 варианта событий: либо это линка, либо нода, либо тупла. Нас интересует только линка
                    ElementTypes type;
                    NativeMethods.sc_memory_get_element_type(PtrScMemoryContext, linkAddres, out type);
                    if (type == ElementTypes.LinkA)
                    {
                        var par1 = new ScIteratorParam { Address = classAddress, IsType = false };
                        var par2 = new ScIteratorParam
                        {
                            IsType = true,
                            Type = ElementTypes.PositiveConstantPermanentAccessArcC
                        };
                        var par3 = new ScIteratorParam { Address = linkAddres, IsType = false };
                        var iterator = NativeMethods.sc_iterator3_new(PtrScMemoryContext,
                            ScIterator3Type.sc_iterator3_f_a_f, par1, par2, par3);

                        while (NativeMethods.sc_iterator3_next(iterator))
                        {
                            var linkAdress = NativeMethods.sc_iterator3_value(iterator, 2);
                            IntPtr streamPtr;
                            NativeMethods.sc_memory_get_link_content(PtrScMemoryContext, linkAdress, out streamPtr);
                            var linkBytes = GetBytes(streamPtr);
                            if (linkBytes != null)
                            {
                                var linkContent = ScLinkContent.ToLong(linkBytes);
                                var startLongValue = ScLinkContent.ToLong(startValue);
                                var isEqual = false;

                                switch (compareType)
                                {
                                    case ComparerTypeEnum.UnknownWrapper:
                                        break;
                                    case ComparerTypeEnum.Equal:
                                        isEqual = (linkContent == startLongValue);
                                        break;
                                    case ComparerTypeEnum.PartOfEqual:
                                        break;
                                    case ComparerTypeEnum.Less:
                                        isEqual = linkContent <= startLongValue;
                                        break;
                                    case ComparerTypeEnum.More:
                                        isEqual = linkContent >= startLongValue;
                                        break;
                                    case ComparerTypeEnum.Between:
                                        var endLongValue = ScLinkContent.ToLong(endValue);
                                        isEqual = (linkContent >= startLongValue && linkContent <= endLongValue);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(compareType), compareType, null);
                                }

                                if (isEqual)
                                {
                                    WScAddress nodeAddress;
                                    if (NativeMethods.sc_memory_get_arc_begin(PtrScMemoryContext, arcAdress,
                                            out nodeAddress) == ScResult.ScResultOk)
                                    {
                                        var node = new ScNode(new ScAddress(nodeAddress), this);
                                        if (node.ElementType == ScTypes.NodeConstant ||
                                            node.ElementType ==
                                            ScTypes.NodeConstantTuple) //здесь определяем, какие в итоге узлы добавлять
                                            result.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            return result;
        }

        #endregion

        /// <summary>
        /// Находит все узлы с тегом определенного класса и типа
        /// </summary>
        /// <param name="classIdentifier"></param>
        /// <param name="tagRelation"></param>
        /// <param name="elementType"> Тип узла значения тега</param>
        /// <returns></returns>
        public List<ScNode> FindTagByNode(Identifier classIdentifier, Identifier tagRelation, ScTypes elementType)
        {
            var result = new List<ScNode>();
            
            #region Находим адреса параметров

            var classBytes = classIdentifier.GetBytes();
            WScAddress classAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, classBytes,
                (uint)classBytes.Length,
                out classAddress);

            var relationBytes = tagRelation.GetBytes();
            WScAddress relationAddress;
            NativeMethods.sc_helper_find_element_by_system_identifier(PtrScMemoryContext, relationBytes,
                (uint)relationBytes.Length,
                out relationAddress);

            #endregion

            #region Находим все конструкции, где участвует тег

            var p1 = new ScIteratorParam { Address = relationAddress, IsType = false };
            var p2 = new ScIteratorParam { IsType = true, Type = ElementTypes.PositiveConstantPermanentAccessArcC };
            var p3 = new ScIteratorParam { IsType = true, Type = ElementTypes.ConstantCommonArcC };
            var arcIterator =
                NativeMethods.sc_iterator3_new(PtrScMemoryContext, ScIterator3Type.sc_iterator3_f_a_a, p1, p2, p3);
            while (NativeMethods.sc_iterator3_next(arcIterator))
            {
                var arcAdress = NativeMethods.sc_iterator3_value(arcIterator, 2);
                WScAddress nodeAddres;
                if (NativeMethods.sc_memory_get_arc_end(PtrScMemoryContext, arcAdress, out nodeAddres) ==
                    ScResult.ScResultOk)
                {
                    //далее 3 варианта событий: либо это линка, либо нода, либо тупла. Нас интересует указанного типа
                    ElementTypes type;
                    NativeMethods.sc_memory_get_element_type(PtrScMemoryContext, nodeAddres, out type);
                    if (type == elementType.ElementType)
                    {
                        var par1 = new ScIteratorParam { Address = classAddress, IsType = false };
                        var par2 = new ScIteratorParam
                        {
                            IsType = true,
                            Type = ElementTypes.PositiveConstantPermanentAccessArcC
                        };
                        var par3 = new ScIteratorParam { Address = nodeAddres, IsType = false };
                        var iterator = NativeMethods.sc_iterator3_new(PtrScMemoryContext,
                            ScIterator3Type.sc_iterator3_f_a_f, par1, par2, par3);

                        while (NativeMethods.sc_iterator3_next(iterator))
                        {
                                     WScAddress nodeAddress;
                                    if (NativeMethods.sc_memory_get_arc_begin(PtrScMemoryContext, arcAdress,
                                            out nodeAddress) == ScResult.ScResultOk)
                                    {
                                        var node = new ScNode(new ScAddress(nodeAddress), this);
                                        if (node.ElementType == ScTypes.NodeConstant ||
                                            node.ElementType ==
                                            ScTypes.NodeConstantTuple) //здесь определяем, какие в итоге узлы добавлять
                                            result.Add(node);
                                    }
                                
                            }
                        }
                   
                }
            }

            #endregion

            return result;
        }



    }
}