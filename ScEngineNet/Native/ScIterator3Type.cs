namespace ScEngineNet.Native
{
    /// <summary>
    /// Типы ScIterator3 из нативной библиотеки.
    /// </summary>
    internal enum ScIterator3Type
    {
        sc_iterator3_unknown = -1,
        sc_iterator3_f_a_a = 0, // output arcs
        sc_iterator3_a_a_f=1,   // input arcs
        sc_iterator3_f_a_f =2   // arcs between two elements
    }
}
