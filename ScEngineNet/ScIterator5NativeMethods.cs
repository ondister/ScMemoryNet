using System;
using System.Runtime.InteropServices;

using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;

namespace ScEngineNet
{
    internal static partial class NativeMethods
    {
        //  _SC_EXTERN sc_iterator5* sc_iterator5_new(const sc_memory_context *ctx, sc_iterator5_type type,
        //                               sc_iterator_param p1, sc_iterator_param p2, sc_iterator_param p3, sc_iterator_param p4, sc_iterator_param p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_new(IntPtr context, ScIterator5Type IteratorType, ScIteratorParam p1, ScIteratorParam p2, ScIteratorParam p3, ScIteratorParam p4, ScIteratorParam p5);

        ///*! Create new sc-iterator5
        // * @param type Iterator type (search template)
        // * @param p1 First element type
        // * @param p2 Second element type
        // * @param p3 sc-addr of third element in construction
        // * @param p4 4th element type
        // * @param p5 sc-addr of 5th element in construction
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator5* sc_iterator5_a_a_f_a_f_new(const sc_memory_context *ctx, sc_type p1, sc_type p2, sc_addr p3, sc_type p4, sc_addr p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_a_a_f_a_f_new(IntPtr context, ElementType p1, ElementType p2,  WScAddress p3, ElementType p4,  WScAddress p5);

        ///*! Create new sc-iterator5
        // * @param type Iterator type (search template)
        // * @param p1 sc-addr of first element in construction
        // * @param p2 Second element type
        // * @param p3 Third element type
        // * @param p4 4-th element type
        // * @param p5 sc-addr of 5th element in construction
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator5* sc_iterator5_f_a_a_a_f_new(const sc_memory_context *ctx, sc_addr p1, sc_type p2, sc_type p3, sc_type p4, sc_addr p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_f_a_a_a_f_new(IntPtr context,  WScAddress p1, ElementType p2, ElementType p3, ElementType p4,   WScAddress p5);

        ///*! Create new sc-iterator5
        // * @param type Iterator type (search template)
        // * @param p1 sc-addr of first element in construction
        // * @param p2 Second element type
        // * @param p3 sc-addr of third element in construction
        // * @param p4 4-th element type
        // * @param p5 sc-addr of 5th element in construction
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator5* sc_iterator5_f_a_f_a_f_new(const sc_memory_context *ctx, sc_addr p1, sc_type p2, sc_addr p3, sc_type p4, sc_addr p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_f_a_f_a_f_new(IntPtr context,  WScAddress p1, ElementType p2,  WScAddress p3, ElementType p4,  WScAddress p5);

        ///*! Create new sc-iterator5
        // * @param type Iterator type (search template)
        // * @param p1 sc-addr of first element in construction
        // * @param p2 Second element type
        // * @param p3 sc-addr of first element in construction
        // * @param p4 4-th element type
        // * @param p5 5-th element type
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator5* sc_iterator5_f_a_f_a_a_new(const sc_memory_context *ctx, sc_addr p1, sc_type p2, sc_addr p3, sc_type p4, sc_type p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_f_a_f_a_a_new(IntPtr context,  WScAddress p1, ElementType p2,  WScAddress p3, ElementType p4, ElementType p5);

        ///*! Create new sc-iterator5
        // * @param type Iterator type (search template)
        // * @param p1 sc-addr of first element in construction
        // * @param p2 Second element type
        // * @param p3 Third element type
        // * @param p4 4-th element type
        // * @param p5 5-th element type
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator5* sc_iterator5_f_a_a_a_a_new(const sc_memory_context *ctx, sc_addr p1, sc_type p2, sc_type p3, sc_type p4, sc_type p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_f_a_a_a_a_new(IntPtr context,  WScAddress p1, ElementType p2, ElementType p3, ElementType p4, ElementType p5);

        ///*! Create new sc-iterator5
        // * @param type Iterator type (search template)
        // * @param p1 First element type
        // * @param p2 Second element type
        // * @param p3 sc-addr of third element in construction
        // * @param p4 4-th element type
        // * @param p5 5-th element type
        // * @return Pointer to created iterator. If parameters invalid for specified iterator type, or type is not a sc-iterator-3, then return 0
        // */
        //_SC_EXTERN sc_iterator5* sc_iterator5_a_a_f_a_a_new(const sc_memory_context *ctx, sc_type p1, sc_type p2, sc_addr p3, sc_type p4, sc_type p5);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_a_a_f_a_a_new(IntPtr context, ElementType p1, ElementType p2,   WScAddress p3, ElementType p4, ElementType p5);

        ///*! Go to next iterator result
        // * @param it Pointer to iterator that we need to go next result
        // * @return Return SC_TRUE, if iterator moved to new results; otherwise return SC_FALSE.
        // * example: while(sc_iterator_next(it)) { <your code> }
        // */
        //_SC_EXTERN sc_bool sc_iterator5_next(sc_iterator5 *it);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern bool sc_iterator5_next(IntPtr iterator5);

        ///*! Get iterator value
        // * @param it Pointer to iterator for getting value
        // * @param vid Value id (can't be more that 5 for sc-iterator5)
        // * @return Return sc-addr of search result value
        // */
        //_SC_EXTERN sc_addr sc_iterator5_value(sc_iterator5 *it, sc_uint vid);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_iterator5_value(IntPtr iterator5, uint valueId);

        ///*! Destroy iterator and free allocated memory
        // * @param it Pointer to sc-iterator that need to be destroyed
        // */
        //_SC_EXTERN void sc_iterator5_free(sc_iterator5 *it);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_iterator5_free(IntPtr iterator5);
    }
}
