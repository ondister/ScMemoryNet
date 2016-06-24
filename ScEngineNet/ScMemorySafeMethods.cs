using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;

namespace ScEngineNet
{
   internal static class ScMemorySafeMethods
    {

       internal static ElementType GetElementType(IntPtr scExtContext, ScElement element)
       {
           ElementType elementType = ElementType.Unknown;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
             ScResult result = NativeMethods.sc_memory_get_element_type(scExtContext, element.ScAddress.WScAddress, out elementType);
           }

           return elementType;
       }

       internal static bool IsElementExist(IntPtr scExtContext, ScAddress elementAddress)
       {
           bool isExist = false;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               isExist = NativeMethods.sc_memory_is_element(scExtContext, elementAddress.WScAddress);
           }

           return isExist;
       }


       internal static bool DeleteElement(IntPtr scExtContext, ScElement element)
       {
           ScResult result = ScResult.SC_RESULT_ERROR;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               result = NativeMethods.sc_memory_element_free(scExtContext, element.ScAddress.WScAddress);
           }
           return result.ToBool();
       }



       internal static ScElement GetArcBeginElement(IntPtr scExtContext, ScArc arc)
       {
           ScElement scElement = new ScElement(ScAddress.Invalid, scExtContext);
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               WScAddress wScAddress;
               NativeMethods.sc_memory_get_arc_begin(scExtContext, arc.ScAddress.WScAddress, out wScAddress);

               scElement = ScMemorySafeMethods.GetElement(scExtContext, wScAddress);
           }

           return scElement;
       }


       internal static ScElement GetElement(IntPtr scExtContext, WScAddress elementAddress)
       {
           ElementType elementType = ElementType.Unknown;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               ScResult result = NativeMethods.sc_memory_get_element_type(scExtContext, elementAddress, out elementType);
           }
           if (elementType == ElementType.Link_a)
           { 
           return new ScLink(new ScAddress(elementAddress), scExtContext);
           }

           else if (elementType.HasAnyType(ElementType.ArcMask_c))
           {
               return new ScArc(new ScAddress(elementAddress), scExtContext);
           }

           else 
           {
               return new ScNode(new ScAddress(elementAddress), scExtContext);
           }
       }


       internal static ScElement GetArcEndElement(IntPtr scExtContext, ScArc arc)
       {
           ScElement scElement = new ScElement(ScAddress.Invalid, scExtContext);
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               WScAddress wScAddress;
               NativeMethods.sc_memory_get_arc_end(scExtContext, arc.ScAddress.WScAddress, out wScAddress);
               scElement = ScMemorySafeMethods.GetElement(scExtContext, wScAddress);
           }

           return scElement;
       }


       internal static Identifier GetSystemIdentifier(IntPtr scExtContext, ScNode node)
       {
           Identifier identifier = Identifier.Invalid;
           WScAddress linkAddress;
           if (NativeMethods.sc_helper_get_system_identifier_link(scExtContext, node.ScAddress.WScAddress, out linkAddress) == ScResult.SC_RESULT_OK)
           {
               identifier = ScLinkContent.ToString(ScMemorySafeMethods.GetLinkContent(scExtContext, new ScLink(new ScAddress(linkAddress), scExtContext)).Content);
           }

           return identifier;
       }

       internal static bool SetSystemIdentifier(IntPtr scExtContext, ScNode node, Identifier identifier)
       {
           ScResult result = ScResult.SC_RESULT_ERROR;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               byte[] bytes = identifier.GetBytes();
               result = NativeMethods.sc_helper_set_system_identifier(scExtContext, node.ScAddress.WScAddress, bytes, (uint)bytes.Length);
           }
           return result == ScResult.SC_RESULT_OK ? true : false;
       }

       internal static ScNode FindNode(IntPtr scExtContext, Identifier identifier)
       {
           WScAddress address;
           ScNode node = new ScNode(ScAddress.Invalid, scExtContext);
           byte[] bytes = identifier.GetBytes();
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               NativeMethods.sc_helper_find_element_by_system_identifier(scExtContext, bytes, (uint)bytes.Length, out address);
               node = new ScNode(new ScAddress(address), scExtContext);
           }
           return node;
       }


       internal static ScLinkContent GetLinkContent(IntPtr scExtContext, ScLink link)
       {
           IntPtr streamPtr = IntPtr.Zero;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               NativeMethods.sc_memory_get_link_content(scExtContext, link.ScAddress.WScAddress, out streamPtr);
           }
           return new ScLinkContent(streamPtr);
       }

       internal static bool SetLinkContent(IntPtr scExtContext, ScLinkContent content, ScLink link)
       {
           ScResult result = ScResult.SC_RESULT_ERROR;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               result = NativeMethods.sc_memory_set_link_content(scExtContext, link.ScAddress.WScAddress, content.ScStream);
           }
           return result == ScResult.SC_RESULT_OK ? true : false;
       }


       internal static IntPtr CreateIterator3(IntPtr scExtContext, ScIterator3Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3)
       {
           IntPtr iterPtr = IntPtr.Zero;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               iterPtr = NativeMethods.sc_iterator3_new(scExtContext, iteratorType, p1, p2, p3);
           }
           return iterPtr;
       }

       internal static IntPtr CreateIterator5(IntPtr scExtContext, ScIterator5Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3, ScIteratorParam p4, ScIteratorParam p5)
       {
           IntPtr iterPtr = IntPtr.Zero;
           if (ScMemoryContext.IsMemoryInitialized() == true)
           {
               iterPtr = NativeMethods.sc_iterator5_new(scExtContext, iteratorType, p1, p2, p3, p4, p5);
           }
           return iterPtr;
       }

    }
}
