using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ScEngineNet;
using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;

namespace ScEngineNetTest
{
    [TestClass]
    public class ScIterator3FunctionsTest
    {
        static IntPtr scMemoryContext;

        [TestMethod]
        public void ScIterator3FunctionsTestMethod()
        {
            var scParams = new WScMemoryParams
            {
                Clear = true,
                ConfigFile = @"sc-memory.ini",
                RepoPath = @"repo",
                ExtensionsPath = @"extensions"
            };

            //sc_memory_initialize 
            scMemoryContext = NativeMethods.sc_memory_initialize(scParams);

            //создаем конструкцию
            WScAddress addrNode = NativeMethods.sc_memory_node_new(  scMemoryContext, ElementType.ConstantNode_c);
            WScAddress addrLink = NativeMethods.sc_memory_link_new(  scMemoryContext);
            WScAddress addrCommArc = NativeMethods.sc_memory_arc_new(  scMemoryContext, ElementType.PositiveConstantPermanentAccessArc_c, addrNode, addrLink);

            //sc_iterator3_f_a_a_new
            //sc_iterator3_a_a_f_new
            //sc_iterator3_f_a_f_new
            //sc_iterator3_new
            //sc_iterator3_free
            //sc_iterator3_next
            //sc_iterator3_value
            //sc_iterator_compare_type

            //IntPtr iter5 = ScIterator5Functions.sc_iterator5_f_a_a_a_a_new(scMemoryContext, addrNode, ElementType.PositiveConstantPermanentAccessArc_c, ElementType.Link_a, ElementType.CommonArc_a, ElementType.NonRoleNode_a);

            //while (ScIterator5Functions.sc_iterator5_next(iter5) == true)
            //{
            //    Console.WriteLine(ScIterator5Functions.sc_iterator5_value(iter5, 0).Offset + "/"
            //        + ScIterator5Functions.sc_iterator5_value(iter5, 1).Offset + "/"
            //        + ScIterator5Functions.sc_iterator5_value(iter5, 2).Offset + "/"
            //        + ScIterator5Functions.sc_iterator5_value(iter5, 3).Offset + "/"
            //        + ScIterator5Functions.sc_iterator5_value(iter5, 4).Offset);
            //}

            //sc_memory_shutdown
            bool isShutDown = NativeMethods.sc_memory_shutdown(false);
            if (isShutDown)
            {
                scMemoryContext = IntPtr.Zero;
            }
            Assert.IsTrue(isShutDown);
        }
    }
}
