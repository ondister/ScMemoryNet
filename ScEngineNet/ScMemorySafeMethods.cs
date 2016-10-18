using ScEngineNet.NativeElements;
using ScEngineNet.NetHelpers;
using ScEngineNet.SafeElements;
using System;
using System.Linq;

namespace ScEngineNet
{
    /// <summary>
    /// Безопасная имплементация нативных функций
    /// </summary>
    internal static class ScMemorySafeMethods
    {
        internal static ElementType GetElementType(ScMemoryContext scExtContext, ScElement element)
        {
            var elementType = ElementType.Unknown;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                ScResult result = NativeMethods.sc_memory_get_element_type(scExtContext.PtrScMemoryContext, element.ScAddress.WScAddress,
                    out elementType);
            }
            return elementType;
        }

        internal static bool IsElementExist(ScMemoryContext scExtContext, ScAddress elementAddress)
        {
            bool isExist = false;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                isExist = NativeMethods.sc_memory_is_element(scExtContext.PtrScMemoryContext, elementAddress.WScAddress);
            }
            return isExist;
        }

        internal static ScResult DeleteElement(ScMemoryContext scExtContext, ScElement element)
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                result = NativeMethods.sc_memory_element_free(scExtContext.PtrScMemoryContext, element.ScAddress.WScAddress);
            }
            return result;
        }

        internal static ScArc CreateArc(ScMemoryContext scExtContext, ElementType arcType, ScElement beginElement, ScElement endElement)
        {
            ScArc arc = null;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                if (!NativeMethods.sc_helper_check_arc(scExtContext.PtrScMemoryContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType))
                {
                    arc = new ScArc(new ScAddress(NativeMethods.sc_memory_arc_new(scExtContext.PtrScMemoryContext, arcType, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress)), scExtContext);
                }
                else
                {

                    var container = scExtContext.CreateIterator(beginElement, arcType, endElement);
                    arc = (ScArc)container.ElementAt(0)[1];
                }
            }

            return arc;
        }


        internal static ScElement GetArcBeginElement(ScMemoryContext scExtContext, ScArc arc)
        {
            ScElement scElement = null;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                WScAddress wScAddress;
                NativeMethods.sc_memory_get_arc_begin(scExtContext.PtrScMemoryContext, arc.ScAddress.WScAddress, out wScAddress);
                scElement = ScMemorySafeMethods.GetElement(wScAddress, scExtContext);
            }
            return scElement;
        }

        internal static ScElement GetElement(WScAddress elementAddress, ScMemoryContext scExtContext)
        {
            ScElement element = null;
            var elementType = ElementType.Unknown;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                ScResult result = NativeMethods.sc_memory_get_element_type(scExtContext.PtrScMemoryContext, elementAddress, out elementType);
                if (result == ScResult.SC_RESULT_OK)
                {

                    if (elementType == ElementType.Link_a)
                    {
                        element = new ScLink(new ScAddress(elementAddress), scExtContext);
                    }
                    else if (elementType.HasAnyType(ElementType.ArcMask_c))
                    {
                        element = new ScArc(new ScAddress(elementAddress), scExtContext);
                    }
                    else
                    {
                        element = new ScNode(new ScAddress(elementAddress), scExtContext);
                    }
                }

            }
            return element;
        }

        internal static ScElement GetArcEndElement(ScMemoryContext scExtContext, ScArc arc)
        {
            var scElement = new ScElement(ScAddress.Invalid, scExtContext);
            if (ScMemoryContext.IsMemoryInitialized())
            {
                WScAddress wScAddress;
                NativeMethods.sc_memory_get_arc_end(scExtContext.PtrScMemoryContext, arc.ScAddress.WScAddress, out wScAddress);
                scElement = ScMemorySafeMethods.GetElement(wScAddress, scExtContext);
            }
            return scElement;
        }

        internal static Identifier GetSystemIdentifier(ScMemoryContext scExtContext, ScNode node)
        {
            Identifier identifier = Identifier.Invalid;
            WScAddress linkAddress;
            if (NativeMethods.sc_helper_get_system_identifier_link(scExtContext.PtrScMemoryContext, node.ScAddress.WScAddress, out linkAddress) == ScResult.SC_RESULT_OK)
            {
                identifier = ScLinkContent.ToString(ScMemorySafeMethods.GetLinkContent(scExtContext, new ScLink(new ScAddress(linkAddress), scExtContext)).Bytes);
            }
            return identifier;
        }

        internal static ScResult SetSystemIdentifier(ScMemoryContext scExtContext, ScNode node, Identifier identifier)
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                byte[] bytes = identifier.GetBytes();
                result = NativeMethods.sc_helper_set_system_identifier(scExtContext.PtrScMemoryContext, node.ScAddress.WScAddress, bytes, (uint)bytes.Length);
            }
            return result;
        }

        internal static ScNode FindNode(ScMemoryContext scExtContext, Identifier identifier)
        {
            var node = new ScNode(ScAddress.Invalid, scExtContext);
            byte[] bytes = identifier.GetBytes();
            if (ScMemoryContext.IsMemoryInitialized())
            {
                WScAddress address;
                NativeMethods.sc_helper_find_element_by_system_identifier(scExtContext.PtrScMemoryContext, bytes, (uint)bytes.Length, out address);
                node = new ScNode(new ScAddress(address), scExtContext);
            }
            return node;
        }

        internal static ScLinkContent GetLinkContent(ScMemoryContext scExtContext, ScLink link)
        {
            IntPtr streamPtr = IntPtr.Zero;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_memory_get_link_content(scExtContext.PtrScMemoryContext, link.ScAddress.WScAddress, out streamPtr);

            }
            //определяем тип ссылки
            Identifier classNodeidentifier = ScDataTypes.Instance.TypeBinary;
            var container = scExtContext.CreateIterator(ElementType.ClassNode_a, ElementType.PositiveConstantPermanentAccessArc_c, link);
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

        internal static ScResult SetLinkContent(ScMemoryContext scExtContext, ScLinkContent content, ScLink link)
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                result = NativeMethods.sc_memory_set_link_content(scExtContext.PtrScMemoryContext, link.ScAddress.WScAddress, content.ScStream);
                //delete arc from class_node

                var container = scExtContext.CreateIterator(ElementType.ClassNode_a, ElementType.PositiveConstantPermanentAccessArc_c, link);
                foreach (var construction in container)
                {
                    if (ScDataTypes.Instance.KeyLinkTypes.Contains(((ScNode)construction[0]).SystemIdentifier))
                    {
                        construction[1].DeleteFromMemory();//delete arc
                        break;
                    }
                }
                // create classNode

                ScMemorySafeMethods.CreateArc(scExtContext, ElementType.PositiveConstantPermanentAccessArc_c, scExtContext.FindNode(content.ClassNodeIdentifier), link);
            }
            return result;
        }




        internal static IntPtr CreateIterator3(ScMemoryContext scExtContext, ScIterator3Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3)
        {
            IntPtr iterPtr = IntPtr.Zero;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                iterPtr = NativeMethods.sc_iterator3_new(scExtContext.PtrScMemoryContext, iteratorType, p1, p2, p3);
            }
            return iterPtr;
        }

        internal static IntPtr CreateIterator5(ScMemoryContext scExtContext, ScIterator5Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3, ScIteratorParam p4, ScIteratorParam p5)
        {
            IntPtr iterPtr = IntPtr.Zero;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                iterPtr = NativeMethods.sc_iterator5_new(scExtContext.PtrScMemoryContext, iteratorType, p1, p2, p3, p4, p5);
            }
            return iterPtr;
        }
    }
}
