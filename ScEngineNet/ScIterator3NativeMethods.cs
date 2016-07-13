using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;
using System;
using System.Runtime.InteropServices;

namespace ScEngineNet
{
    internal static partial class NativeMethods
    {
        // /*! Create iterator to find output arcs for specified element
        // * @param el sc-addr of element to iterate output arcs
        // * @param arc_type Type of output arc to iterate (0 - all types)
        // * @param end_type Type of end element for output arcs, to iterate
        // * @return If iterator created, then return pointer to it; otherwise return null
        // */
        //_SC_EXTERN sc_iterator3* sc_iterator3_f_a_a_new(const sc_memory_context *ctx, sc_addr el, sc_type arc_type, sc_type end_type);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator3_f_a_a_new(IntPtr context, WScAddress BeginElementAddress, ElementType arcType, ElementType endElementType);

        ///*! Create iterator to find input arcs for specified element
        // * @param beg_type Type of begin element for input arcs, to iterate
        // * @param arc_type Type of input arc to iterate (0 - all types)
        // * @param el sc-addr of element to iterate input arcs
        // * @return If iterator created, then return pointer to it; otherwise return null
        // */
        //_SC_EXTERN sc_iterator3* sc_iterator3_a_a_f_new(const sc_memory_context *ctx, sc_type beg_type, sc_type arc_type, sc_addr el);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator3_a_a_f_new(IntPtr context, ElementType beginElementType, ElementType arcType, WScAddress EndElementAddress);

        ///*! Create iterator to find arcs between two specified elements
        // * @param el_beg sc-addr of begin element
        // * @param arc_type Type of arcs to iterate (0 - all types)
        // * @param el_end sc-addr of end element
        // * @return If iterator created, then return pointer to it; otherwise return null
        // */
        //_SC_EXTERN sc_iterator3* sc_iterator3_f_a_f_new(const sc_memory_context *ctx, sc_addr el_beg, sc_type arc_type, sc_addr el_end);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator3_f_a_f_new(IntPtr context, WScAddress beginElementAddress, ElementType arcType, WScAddress endElementAddress);
       
        ///*! Create new sc-iterator-3
        // * @param type Iterator type (search template)
        // * @param p1 First iterator parameter
        // * @param p2 Second iterator parameter
        // * @param p3 Third iterator parameter
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator3* sc_iterator3_new(const sc_memory_context *ctx, sc_iterator3_type type, sc_iterator_param p1, sc_iterator_param p2, sc_iterator_param p3);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator3_new(IntPtr context, ScIterator3Type iteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3);
        
        ///*! Destroy iterator and free allocated memory
        // * @param it Pointer to sc-iterator that need to be destroyed
        // */
        //_SC_EXTERN void sc_iterator3_free(sc_iterator3 *it);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator3_free(IntPtr iterator3);

        ///*! Go to next iterator result
        // * @param it Pointer to iterator that we need to go next result
        // * @return Return SC_TRUE, if iterator moved to new results; otherwise return SC_FALSE.
        // * example: while(sc_iterator_next(it)) { <your code> }
        // */
        //_SC_EXTERN sc_bool sc_iterator3_next(sc_iterator3 *it);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_iterator3_next(IntPtr iterator3);

        ///*! Get iterator value
        // * @param it Pointer to iterator for getting value
        // * @param vid Value id (can't be more that 3 for sc-iterator3)
        // * @return Return sc-addr of search result value
        // */
        //_SC_EXTERN sc_addr sc_iterator3_value(sc_iterator3 *it, sc_uint vid);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_iterator3_value(IntPtr iterator3, uint valueId);

        ///*! Check if specified element type passed into
        // * iterator selection.
        // * @param el_type Compared element type
        // * @param it_type Template type from iterator parameter
        // * @return If el_type passed checking, then return SC_TRUE, else return SC_FALSE
        // */
        //_SC_EXTERN sc_bool sc_iterator_compare_type(sc_type el_type, sc_type it_type);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_iterator_compare_type(ElementType elementType, ElementType iteratorType);
    }
}
