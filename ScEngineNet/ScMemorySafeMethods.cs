using System;

using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;
using System.Collections.Generic;
using ScEngineNet.NetHelpers;

namespace ScEngineNet
{
    /// <summary>
    /// Безопасная имплементация нативных функций
    /// </summary>
    internal static class ScMemorySafeMethods
    {
        internal static ElementType GetElementType(IntPtr scExtContext, ScElement element)
        {
            var elementType = ElementType.Unknown;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                ScResult result = NativeMethods.sc_memory_get_element_type(scExtContext, element.ScAddress.WScAddress,
                    out elementType);
            }
            return elementType;
        }

        internal static bool IsElementExist(IntPtr scExtContext, ScAddress elementAddress)
        {
            bool isExist = false;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                isExist = NativeMethods.sc_memory_is_element(scExtContext, elementAddress.WScAddress);
            }
            return isExist;
        }

        internal static ScResult DeleteElement(IntPtr scExtContext, ScElement element)
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                result = NativeMethods.sc_memory_element_free(scExtContext, element.ScAddress.WScAddress);
            }
            return result;
        }

        internal static ScArc CreateArc(IntPtr scExtContext, ElementType arcType, ScElement beginElement, ScElement endElement)
        {
            ScArc arc = null;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                if (!NativeMethods.sc_helper_check_arc(scExtContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType))
                {
                    arc = new ScArc(new ScAddress(NativeMethods.sc_memory_arc_new(scExtContext, arcType, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress)), scExtContext);
                }
                else
                {
                    var tmpContext = new ScMemoryContext(scExtContext);
                    var container = tmpContext.CreateIterator(beginElement, arcType, endElement);
                    var constructions = container.GetAllConstructions();
                    arc = (ScArc)constructions[0].Elements[1];
                }
            }

            return arc;
        }


        internal static ScElement GetArcBeginElement(IntPtr scExtContext, ScArc arc)
        {
            var scElement = new ScElement(ScAddress.Invalid, scExtContext);
            if (ScMemoryContext.IsMemoryInitialized())
            {
                WScAddress wScAddress;
                NativeMethods.sc_memory_get_arc_begin(scExtContext, arc.ScAddress.WScAddress, out wScAddress);
                scElement = ScMemorySafeMethods.GetElement(wScAddress, scExtContext);
            }
            return scElement;
        }

        internal static ScElement GetElement(WScAddress elementAddress, IntPtr scExtContext)
        {
            ScElement element = null;
            var elementType = ElementType.Unknown;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                ScResult result = NativeMethods.sc_memory_get_element_type(scExtContext, elementAddress, out elementType);
                if (result == ScResult.SC_RESULT_OK)
                {

                    if (elementType == ElementType.Link_a)
                    {
                        element= new ScLink(new ScAddress(elementAddress), scExtContext);
                    }
                    else if (elementType.HasAnyType(ElementType.ArcMask_c))
                    {
                       element =new ScArc(new ScAddress(elementAddress), scExtContext);
                    }
                    else
                    {
                        element= new ScNode(new ScAddress(elementAddress), scExtContext);
                    }
                }
                
            }
            return element;
        }

        internal static ScElement GetArcEndElement(IntPtr scExtContext, ScArc arc)
        {
            var scElement = new ScElement(ScAddress.Invalid, scExtContext);
            if (ScMemoryContext.IsMemoryInitialized())
            {
                WScAddress wScAddress;
                NativeMethods.sc_memory_get_arc_end(scExtContext, arc.ScAddress.WScAddress, out wScAddress);
                scElement = ScMemorySafeMethods.GetElement(wScAddress, scExtContext);
            }
            return scElement;
        }

        internal static Identifier GetSystemIdentifier(IntPtr scExtContext, ScNode node)
        {
            Identifier identifier = Identifier.Invalid;
            WScAddress linkAddress;
            if (NativeMethods.sc_helper_get_system_identifier_link(scExtContext, node.ScAddress.WScAddress, out linkAddress) == ScResult.SC_RESULT_OK)
            {
                identifier = ScLinkContent.ToString(ScMemorySafeMethods.GetLinkContent(scExtContext, new ScLink(new ScAddress(linkAddress), scExtContext)).Bytes);
            }
            return identifier;
        }

        internal static ScResult SetSystemIdentifier(IntPtr scExtContext, ScNode node, Identifier identifier)
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                byte[] bytes = identifier.GetBytes();
                result = NativeMethods.sc_helper_set_system_identifier(scExtContext, node.ScAddress.WScAddress, bytes, (uint)bytes.Length);
            }
            return result;
        }

        internal static ScNode FindNode(IntPtr scExtContext, Identifier identifier)
        {
            var node = new ScNode(ScAddress.Invalid, scExtContext);
            byte[] bytes = identifier.GetBytes();
            if (ScMemoryContext.IsMemoryInitialized())
            {
                WScAddress address;
                NativeMethods.sc_helper_find_element_by_system_identifier(scExtContext, bytes, (uint)bytes.Length, out address);
                node = new ScNode(new ScAddress(address), scExtContext);
            }
            return node;
        }

        internal static ScLinkContent GetLinkContent(IntPtr scExtContext, ScLink link)
        {
            IntPtr streamPtr = IntPtr.Zero;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_memory_get_link_content(scExtContext, link.ScAddress.WScAddress, out streamPtr);

            }
            //определяем тип ссылки
            ScNode classNode = DataTypes.Binary;
            var tmpContext = new ScMemoryContext(scExtContext);

            var container = tmpContext.CreateIterator(ElementType.ClassNode_a, ElementType.PositiveConstantPermanentAccessArc_c, link);
            foreach (var construction in container)
            {
                if (DataTypes.KeyNodes.Contains((ScNode)construction.Elements[0]))
                {
                    classNode = (ScNode)construction.Elements[0];
                    break;
                }
            }


            return ScLinkContent.GetScContent(streamPtr, classNode);
        }

        internal static ScResult SetLinkContent(IntPtr scExtContext, ScLinkContent content, ScLink link)
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                result = NativeMethods.sc_memory_set_link_content(scExtContext, link.ScAddress.WScAddress, content.ScStream);
                //delete arc from class_node
                var tmpContext = new ScMemoryContext(scExtContext);
                var container = tmpContext.CreateIterator(ElementType.ClassNode_a, ElementType.PositiveConstantPermanentAccessArc_c, link);
                foreach (var construction in container)
                {
                    if (DataTypes.KeyNodes.Contains((ScNode)construction.Elements[0]))
                    {
                        construction.Elements[1].DeleteFromMemory();//delete arc
                        break;
                    }
                }
                // create classNode

                ScMemorySafeMethods.CreateArc(scExtContext, ElementType.PositiveConstantPermanentAccessArc_c, content.ClassNode, link);
            }
            return result;
        }




        internal static IntPtr CreateIterator3(IntPtr scExtContext, ScIterator3Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3)
        {
            IntPtr iterPtr = IntPtr.Zero;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                iterPtr = NativeMethods.sc_iterator3_new(scExtContext, iteratorType, p1, p2, p3);
            }
            return iterPtr;
        }

        internal static IntPtr CreateIterator5(IntPtr scExtContext, ScIterator5Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3, ScIteratorParam p4, ScIteratorParam p5)
        {
            IntPtr iterPtr = IntPtr.Zero;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                iterPtr = NativeMethods.sc_iterator5_new(scExtContext, iteratorType, p1, p2, p3, p4, p5);
            }
            return iterPtr;
        }
    }
}
