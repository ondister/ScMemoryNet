using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest
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
                ConfigFile = TestParams.ConfigFile,
                RepoPath = TestParams.RepoPath,
                ExtensionsPath = TestParams.ExtensionPath
            };

            #region sc_memory_initialize 

            if (!NativeMethods.sc_memory_is_initialized())
            {
                NativeMethods.sc_memory_initialize(scParams);
            }

            var scMemoryContext = NativeMethods.sc_memory_context_new(0);
            //sc_memory_is_initialized
            var isInitialized = NativeMethods.sc_memory_is_initialized();
            Assert.IsTrue(isInitialized);

            #endregion

            #region sc_helper_set_system_identifier

            var identifier = new ScString("sc_helper_test_idtf");
            var nodeAddr = NativeMethods.sc_memory_node_new(scMemoryContext, ElementTypes.ClassConstantNodeC);
            var resultSetIdtf = NativeMethods.sc_helper_set_system_identifier(scMemoryContext, nodeAddr,
                identifier.Bytes, (uint) identifier.Value.Length);
            Assert.AreEqual(ScResult.ScResultOk, resultSetIdtf);

            #endregion

            #region sc_helper_resolve_system_identifier

            WScAddress resolvedAddr;
            var isResolve = NativeMethods.sc_helper_resolve_system_identifier(scMemoryContext, identifier.Bytes,
                out resolvedAddr);
            Assert.IsTrue(isResolve);
            Assert.AreEqual(nodeAddr.Offset, resolvedAddr.Offset);

            #endregion

            #region sc_helper_get_system_identifier_link

            WScAddress linkAddress;
            var resultGetIdtfLink = NativeMethods.sc_helper_get_system_identifier_link(scMemoryContext, resolvedAddr,
                out linkAddress);
            Assert.AreEqual(ScResult.ScResultOk, resultGetIdtfLink);
            Assert.AreNotEqual(0, linkAddress.Offset);

            #endregion

            #region проверяем идентичность содержимого ссылок

            IntPtr stream;
            var resultgetlinkContent = NativeMethods.sc_memory_get_link_content(scMemoryContext, linkAddress, out stream);
            Assert.AreEqual(ScResult.ScResultOk, resultgetlinkContent);
            Assert.AreEqual(identifier, new ScString(stream));

            #endregion

            #region sc_helper_find_element_by_system_identifier

            WScAddress findedAddress;
            var resultFindElement = NativeMethods.sc_helper_find_element_by_system_identifier(scMemoryContext,
                identifier.Bytes, (uint) identifier.Bytes.Length, out findedAddress);
            Assert.AreEqual(ScResult.ScResultOk, resultFindElement);
            Assert.AreEqual(nodeAddr.Offset, findedAddress.Offset);

            #endregion

            #region sc_helper_get_keynode

            WScAddress keyNodeAddress;
            var resultGetKeyNode = NativeMethods.sc_helper_get_keynode(scMemoryContext,
                KeyNode.ScKeynodeNrelSystemIdentifier, out keyNodeAddress);
            Assert.AreEqual(ScResult.ScResultOk, resultGetKeyNode);
            Assert.AreNotEqual(0, keyNodeAddress.Offset);

            #endregion

            #region sc_helper_check_arc

            var beginNodeAddr = NativeMethods.sc_memory_node_new(scMemoryContext, ElementTypes.ClassConstantNodeC);
            var endNodeAddr = NativeMethods.sc_memory_node_new(scMemoryContext, ElementTypes.NodeA);
            NativeMethods.sc_memory_arc_new(scMemoryContext,
                ElementTypes.PositiveConstantPermanentAccessArcC, beginNodeAddr, endNodeAddr);
            var isExist = NativeMethods.sc_helper_check_arc(scMemoryContext, beginNodeAddr, endNodeAddr,
                ElementTypes.PositiveConstantPermanentAccessArcC);
            Assert.IsTrue(isExist);

            #endregion

            #region sc_helper_check_version_equal

            const byte major = 0;
            const byte minor = 3;
            const byte patch = 0;
            var isRight = NativeMethods.sc_helper_check_version_equal(major, minor, patch);
            Assert.IsTrue(isRight, "Тест может не проходить, если версия библиотеки не верна.");

            #endregion

            #region sc_memory_shutdown

            NativeMethods.sc_memory_context_free(scMemoryContext);
            var isShutDown = NativeMethods.sc_memory_shutdown(false);
            Assert.IsTrue(isShutDown);

            #endregion
        }
    }
}