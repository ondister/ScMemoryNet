using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ScEngineNet;
using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;

namespace ScEngineNetTest
{
    [TestClass]
    public class ScHelperFunctionsTest
    {
        [TestMethod]
        public void ScHelperFunctionsTestMethod()
        {
            var scParams = new WScMemoryParams
            {
                Clear = true,
                ConfigFile = @"sc-memory.ini",
                RepoPath = @"repo",
                ExtensionsPath = @"extensions"
            };
            //sc_memory_initialize 
            IntPtr scMemoryContext = NativeMethods.sc_memory_initialize(scParams);

            //sc_memory_is_initialized
            bool isInitialized = NativeMethods.sc_memory_is_initialized();
            Assert.IsTrue(isInitialized);
                        
            //sc_helper_set_system_identifier
            var identifier=new ScLinkContent("sc_helper_test_idtf");
            var nodeAddr = NativeMethods.sc_memory_node_new(scMemoryContext, ElementType.ClassNode_a);
            var resultSetIdtf = NativeMethods.sc_helper_set_system_identifier(scMemoryContext, nodeAddr, identifier.Content, (uint)identifier.Content.Length);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultSetIdtf);

            //sc_helper_resolve_system_identifier
            WScAddress resolvedAddr;
            bool isResolve = NativeMethods.sc_helper_resolve_system_identifier(  scMemoryContext, identifier.Content, out resolvedAddr);
            Assert.IsTrue(isResolve);
            Assert.AreEqual(nodeAddr.Offset, resolvedAddr.Offset);

            //sc_helper_get_system_identifier_link
            WScAddress linkAddress;
            ScResult resultGetIdtfLink = NativeMethods.sc_helper_get_system_identifier_link(  scMemoryContext, resolvedAddr, out linkAddress);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultGetIdtfLink);
            Assert.AreNotEqual(0, linkAddress.Offset);

            //проверяем идентичность содержимого ссылок
            IntPtr stream;
            ScResult resultgetlinkContent = NativeMethods.sc_memory_get_link_content(  scMemoryContext, linkAddress, out stream);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultgetlinkContent);
            Assert.AreEqual(identifier, new ScLinkContent(stream));

            //sc_helper_find_element_by_system_identifier
            WScAddress findedAddress;
            ScResult resultFindElement = NativeMethods.sc_helper_find_element_by_system_identifier(  scMemoryContext, identifier.Content, (uint)identifier.Content.Length, out findedAddress);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultFindElement);
            Assert.AreEqual(nodeAddr.Offset, findedAddress.Offset);

            //sc_helper_get_keynode
            WScAddress keyNodeAddress;
            ScResult resultGetKeyNode = NativeMethods.sc_helper_get_keynode(  scMemoryContext, KeyNode.SC_KEYNODE_NREL_SYSTEM_IDENTIFIER, out keyNodeAddress);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultGetKeyNode);
            Assert.AreNotEqual(0, keyNodeAddress.Offset);
          
            //sc_helper_check_arc
            WScAddress beginNodeAddr = NativeMethods.sc_memory_node_new(  scMemoryContext, ElementType.ClassNode_a);
            WScAddress EndNodeAddr = NativeMethods.sc_memory_node_new(  scMemoryContext, ElementType.Node_a);
            WScAddress arcAddr = NativeMethods.sc_memory_arc_new(  scMemoryContext, ElementType.PositiveConstantPermanentAccessArc_c, beginNodeAddr, EndNodeAddr);
            bool isExist = NativeMethods.sc_helper_check_arc(  scMemoryContext, beginNodeAddr, EndNodeAddr, ElementType.PositiveConstantPermanentAccessArc_c);
            Assert.IsTrue(isExist);

            //sc_helper_check_version_equal
            byte major = 0;
            byte minor = 2;
            byte patch = 0;
            bool isRight = NativeMethods.sc_helper_check_version_equal(major, minor, patch);
            Assert.IsTrue(isRight, "Тест может и не проходить, если версия библиотеки не верна");

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
