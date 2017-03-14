using ScEngineNet.Events;
using ScEngineNet.ScElements;
using System;
using System.Runtime.InteropServices;

namespace ScEngineNet.Native
{
    //   typedef sc_result (*fEventCallbackEx)(const sc_event *event, sc_addr arg, sc_addr other_el);
    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    internal delegate ScResult fEventCallbackEx(ref WScEvent scEvent, WScAddress arg, WScAddress other_el);

    /// Backward compatibility
    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    internal delegate ScResult fEventCallback(ref WScEvent scEvent, WScAddress arg);

    //typedef sc_result (*fDeleteCallback)(const sc_event *event);
    [UnmanagedFunctionPointer(ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
    internal delegate ScResult fDeleteCallback(ref WScEvent scEvent);

    internal static partial class NativeMethods
    {
        //_SC_EXTERN sc_event* sc_event_new(sc_memory_context const * ctx, sc_addr el, sc_event_type type, sc_pointer data, fEventCallback callback, fDeleteCallback delete_callback);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_event_new(IntPtr context, WScAddress element, ScEventType typeEvent, IntPtr dataPtr, fEventCallback callback, fDeleteCallback deleteCallBack);

        //_SC_EXTERN sc_event* sc_event_new_ex(sc_memory_context const * ctx, sc_addr el, sc_event_type type, sc_pointer data, fEventCallbackEx callback, fDeleteCallback delete_callback);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_event_new_ex(IntPtr context, WScAddress element, ScEventType typeEvent, IntPtr dataPtr, fEventCallbackEx callBack, fDeleteCallback DeleteCallBack);


        ///*! Destroys specified sc-event
        //_SC_EXTERN sc_result sc_event_destroy(sc_event *event);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScResult sc_event_destroy(IntPtr scEvent);

        ///*! Returns type of specified sc-event
        //_SC_EXTERN sc_event_type sc_event_get_type(const sc_event *event);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern ScEventType sc_event_get_type(IntPtr scEvent);

        ////! Returns data of specified sc-event
        //_SC_EXTERN sc_pointer sc_event_get_data(const sc_event *event);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern IntPtr sc_event_get_data(IntPtr scEvent);

        ////! Returns sc-addr of sc-element where event subscribed
        //_SC_EXTERN sc_addr sc_event_get_element(const sc_event *event);
        [DllImport(ScEngineNet.ScMemoryDllName, CallingConvention = ScEngineNet.DefaultCallingConvention, CharSet = ScEngineNet.DefaultCharset)]
        internal static extern WScAddress sc_event_get_element(IntPtr scEvent);
    }
}
