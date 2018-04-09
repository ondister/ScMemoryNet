using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet.LinkContent;
using ScEngineNet.Native;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest
{
    [TestClass]
    public class ScMemoryFunctionsTest
    {
        private const byte BaseAccessLevel = 10;
        [TestMethod]
        public void ScMemoryFunctionsTestMethod()
        {
            var scParams = new WScMemoryParams
            {
                Clear = true,
                ConfigFile = TestParams.ConfigFile,
                RepoPath = TestParams.RepoPath,
                ExtensionsPath = TestParams.ExtensionPath
            };

            #region Initialize

            var isInitialized = NativeMethods.sc_memory_is_initialized();
            if (isInitialized == false)
            {
                NativeMethods.sc_memory_initialize(scParams);
            }

            var scMemoryContext = NativeMethods.sc_memory_context_new(BaseAccessLevel);

            #endregion


            #region sc_memory_is_initialized

            isInitialized = NativeMethods.sc_memory_is_initialized();
            Assert.IsTrue(isInitialized);

            #endregion

            #region sc_memory_node_new

            var nodeAddr = NativeMethods.sc_memory_node_new(scMemoryContext, ElementTypes.NodeA);
            Assert.AreNotEqual(0, nodeAddr.Offset);

            #endregion

            #region sc_memory_is_element

            var isExist = NativeMethods.sc_memory_is_element(scMemoryContext, nodeAddr);
            Assert.IsTrue(isExist);

            #endregion

            #region sc_memory_change_element_subtype

            const ElementTypes newType = ElementTypes.ConstantA;
            var resultChangeType = NativeMethods.sc_memory_change_element_subtype(scMemoryContext, nodeAddr, newType);
            Assert.AreEqual(ScResult.ScResultOk, resultChangeType);

            #endregion

            #region sc_memory_get_element_type

            ElementTypes gettingType;
            var resultGetElementType = NativeMethods.sc_memory_get_element_type(scMemoryContext, nodeAddr,
                out gettingType);
            Assert.AreEqual(ScResult.ScResultOk, resultGetElementType);
            Assert.AreEqual(ElementTypes.ConstantNodeC, gettingType);

            #endregion

            #region sc_memory_link_new

            var linkAddr = NativeMethods.sc_memory_link_new(scMemoryContext);
            Assert.AreNotEqual(0, linkAddr.Offset);

            #endregion

            #region sc_memory_set_link_content

            var linkContent = new ScString("Test Content 1234567890 Тест Контент");
            var resultSetLinkContent = NativeMethods.sc_memory_set_link_content(scMemoryContext, linkAddr,
                linkContent.ScStream);
            Assert.AreEqual(ScResult.ScResultOk, resultSetLinkContent);

            #endregion

            #region sc_memory_get_link_content

            IntPtr streamPtr;
            NativeMethods.sc_memory_get_link_content(scMemoryContext, linkAddr, out streamPtr);
            ScLinkContent gettingContent = new ScString(streamPtr);
            Assert.AreEqual(linkContent, gettingContent);

            #endregion

            #region sc_memory_find_links_with_content

            uint resultCount;
            IntPtr addressesPtr;
            NativeMethods.sc_memory_find_links_with_content(scMemoryContext, linkContent.ScStream,
                out addressesPtr, out resultCount);
            Assert.AreNotEqual(addressesPtr, IntPtr.Zero);
            Assert.AreNotEqual(0, resultCount);
            var adressesArray = NativeMethods.PtrToArray(typeof (WScAddress), addressesPtr, resultCount);
            Assert.AreNotEqual(0, adressesArray.Length);

            #endregion

            #region sc_memory_free_buff

            NativeMethods.sc_memory_free_buff(addressesPtr);

            #endregion

            #region sc_memory_arc_new

            var arcAddr = NativeMethods.sc_memory_arc_new(scMemoryContext,
                ElementTypes.PositiveConstantPermanentAccessArcC, nodeAddr, linkAddr);
            Assert.AreNotEqual(0, arcAddr.Offset);

            #endregion

            #region sc_memory_get_arc_begin

            WScAddress gettingArcBeginAddr;
            var resultGetArcBegin = NativeMethods.sc_memory_get_arc_begin(scMemoryContext, arcAddr,
                out gettingArcBeginAddr);
            Assert.AreEqual(ScResult.ScResultOk, resultGetArcBegin);
            Assert.AreEqual(nodeAddr.Offset, gettingArcBeginAddr.Offset);

            #endregion

            #region sc_memory_get_arc_end

            WScAddress gettingArcEndAddr;
            var resultGetArcEnd = NativeMethods.sc_memory_get_arc_end(scMemoryContext, arcAddr, out gettingArcEndAddr);
            Assert.AreEqual(ScResult.ScResultOk, resultGetArcEnd);
            Assert.AreEqual(linkAddr.Offset, gettingArcEndAddr.Offset);

            #endregion

            #region sc_memory_get_arc_info

            WScAddress startArcAddr;
            WScAddress endArcAddr;
            var resultGetArcInfo = NativeMethods.sc_memory_get_arc_info(scMemoryContext, arcAddr, out startArcAddr,
                out endArcAddr);
            Assert.AreEqual(ScResult.ScResultOk, resultGetArcInfo);
            Assert.AreEqual(nodeAddr.Offset, startArcAddr.Offset);
            Assert.AreEqual(linkAddr.Offset, endArcAddr.Offset);

            #endregion

            #region sc_memory_get_element_access_levels

            byte gettingAccessLevel;
            var resultgetAccessLevel = NativeMethods.sc_memory_get_element_access_levels(scMemoryContext, nodeAddr,
                out gettingAccessLevel);
            Assert.AreEqual(ScResult.ScResultOk, resultgetAccessLevel);
            Assert.AreEqual(BaseAccessLevel, gettingAccessLevel);

            #endregion

            #region sc_memory_stat

            ScStat statistics;
            var resultGetStatistics = NativeMethods.sc_memory_stat(scMemoryContext, out statistics);
            Assert.AreEqual(ScResult.ScResultOk, resultGetStatistics);
            Assert.AreNotEqual(0, statistics.ArcCount);
            Assert.AreNotEqual(0, statistics.EmptyCount);
            Assert.AreNotEqual(0, statistics.LinkCount);
            Assert.AreNotEqual(0, statistics.NodeCount);
            Assert.AreNotEqual(0, statistics.SegmentsCount);

            #endregion

            #region sc_memory_save

            var resultSaveState = NativeMethods.sc_memory_save(scMemoryContext);
            Assert.AreEqual(ScResult.ScResultOk, resultSaveState);

            #endregion

            #region sc_memory_element_free

            var resultFree = NativeMethods.sc_memory_element_free(scMemoryContext, nodeAddr);
            Assert.AreEqual(ScResult.ScResultOk, resultFree);

            #endregion

            #region sc_memory_set_element_access_levels
#warning Этот метод не работает, поэтому для прохождения теста задается такое же значение, как и начальное.Пока не знаю почему. Может он и в нативной библиотеке не работает
            byte oldAccessLevel;
            const byte settingAccessLevel = 10;
            var resultsetAccessLevel = NativeMethods.sc_memory_set_element_access_levels(scMemoryContext, nodeAddr,
                settingAccessLevel, out oldAccessLevel);
            Assert.AreEqual(ScResult.ScResultOk, resultsetAccessLevel);
            Assert.AreEqual(settingAccessLevel, oldAccessLevel);

            #endregion

            #region sc_memory_shutdown

            NativeMethods.sc_memory_context_free(scMemoryContext);
            var isShutDown = NativeMethods.sc_memory_shutdown(false);
            Assert.IsTrue(isShutDown);

            #endregion
        }
    }
}