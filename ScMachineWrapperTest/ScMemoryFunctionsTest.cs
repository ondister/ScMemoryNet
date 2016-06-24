using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;
namespace ScEngineNetTest
{



    [TestClass]
    public class ScMemoryFunctionsTest
    {

        [TestMethod]
        public void ScMemoryFunctionsTestMethod()
        {
            IntPtr scMemoryContext;
            WScMemoryParams scParams = new WScMemoryParams();

            scParams.Clear = true;
            scParams.ConfigFile = @"sc-memory.ini";
            scParams.RepoPath = @"repo";
            scParams.ExtensionsPath = @"extensions";
            //sc_memory_initialize 
            scMemoryContext = NativeMethods.sc_memory_initialize(scParams);

            //sc_memory_is_initialized
            bool isInitialized = NativeMethods.sc_memory_is_initialized();
            Assert.IsTrue(isInitialized);

            // sc_memory_node_new
            WScAddress nodeAddr = NativeMethods.sc_memory_node_new( scMemoryContext, ElementType.Node_a);
            Assert.AreNotEqual(0, nodeAddr.Offset);

            //sc_memory_is_element
            bool isExist = NativeMethods.sc_memory_is_element( scMemoryContext, nodeAddr);
            Assert.IsTrue(isExist);


            //sc_memory_change_element_subtype
            ElementType newType = ElementType.Constant_a;
            ScResult resultChangeType = NativeMethods.sc_memory_change_element_subtype( scMemoryContext, nodeAddr, newType);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultChangeType);

            //sc_memory_get_element_type
            ElementType gettingType = ElementType.Unknown;
            ScResult resultGetElementType = NativeMethods.sc_memory_get_element_type( scMemoryContext, nodeAddr, out gettingType);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultGetElementType);
            Assert.AreEqual(ElementType.ConstantNode_c, gettingType);


            //sc_memory_link_new
            WScAddress linkAddr = NativeMethods.sc_memory_link_new( scMemoryContext);
            Assert.AreNotEqual(0, linkAddr.Offset);

            //sc_memory_set_link_content
            ScLinkContent linkContent = new ScLinkContent("Test Content 1234567890 Тест Контент");
            ScResult resultSetLinkContent = NativeMethods.sc_memory_set_link_content( scMemoryContext, linkAddr, linkContent.ScStream);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultSetLinkContent);

            //sc_memory_get_link_content
            IntPtr streamPtr = IntPtr.Zero;
            NativeMethods.sc_memory_get_link_content( scMemoryContext, linkAddr, out streamPtr);
            ScLinkContent gettingContent = new ScLinkContent(streamPtr);
            Assert.AreEqual(linkContent, gettingContent);

            //sc_memory_find_links_with_content
            uint resultCount = 0;
            IntPtr addressesPtr = IntPtr.Zero;
            ScResult resultFindLinks = NativeMethods.sc_memory_find_links_with_content( scMemoryContext, linkContent.ScStream, out addressesPtr, out resultCount);
            Assert.AreNotEqual(addressesPtr, IntPtr.Zero);
            Assert.AreNotEqual(0, resultCount);
            Array adressesArray = NativeMethods.PtrToArray(typeof(WScAddress), addressesPtr, resultCount);
            Assert.AreNotEqual(0, adressesArray.Length);

            //sc_memory_free_buff
            NativeMethods.sc_memory_free_buff(addressesPtr);


            //sc_memory_arc_new
            WScAddress arcAddr = NativeMethods.sc_memory_arc_new( scMemoryContext, ElementType.PositiveConstantPermanentAccessArc_c, nodeAddr, linkAddr);
            Assert.AreNotEqual(0, arcAddr.Offset);

            // sc_memory_get_arc_begin
            WScAddress gettingArcBeginAddr;
            ScResult resultGetArcBegin = NativeMethods.sc_memory_get_arc_begin( scMemoryContext, arcAddr, out gettingArcBeginAddr);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultGetArcBegin);
            Assert.AreEqual(nodeAddr.Offset, gettingArcBeginAddr.Offset);

            //sc_memory_get_arc_end
            WScAddress gettingArcEndAddr;
            ScResult resultGetArcEnd = NativeMethods.sc_memory_get_arc_end( scMemoryContext, arcAddr, out gettingArcEndAddr);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultGetArcEnd);
            Assert.AreEqual(linkAddr.Offset, gettingArcEndAddr.Offset);

            //sc_memory_get_element_access_levels
            byte gettingAccessLevel = 0; ;
            ScResult resultgetAccessLevel = NativeMethods.sc_memory_get_element_access_levels( scMemoryContext, nodeAddr, out gettingAccessLevel);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultgetAccessLevel);
            Assert.AreNotEqual(0, gettingAccessLevel);

            //sc_memory_set_element_access_levels
            byte settingAccessLevel = 128; ;
            ScResult resultsetAccessLevel = NativeMethods.sc_memory_set_element_access_levels( scMemoryContext, nodeAddr, settingAccessLevel, out gettingAccessLevel);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultsetAccessLevel);
            Assert.AreEqual(settingAccessLevel, gettingAccessLevel);

            //sc_memory_stat
            ScStat statistics;
            ScResult resultGetStatistics = NativeMethods.sc_memory_stat( scMemoryContext, out statistics);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultGetStatistics);
            Assert.AreNotEqual(0, statistics.ArcCount);
            Assert.AreNotEqual(0, statistics.EmptyCount);
            Assert.AreNotEqual(0, statistics.LinkCount);
            Assert.AreNotEqual(0, statistics.NodeCount);
            Assert.AreNotEqual(0, statistics.SegmentsCount);

            //sc_memory_save
            ScResult resultSaveState = NativeMethods.sc_memory_save( scMemoryContext);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultSaveState);


            //sc_memory_element_free
            ScResult resultFree = NativeMethods.sc_memory_element_free( scMemoryContext, nodeAddr);
            Assert.AreEqual(ScResult.SC_RESULT_OK, resultFree);

#warning Непонятно, как и зачем работает функция очистки. Здесь она не работает. У разработчика памяти она просто очищает поля структуры.
            //sc_memory_params_clear
            NativeMethods.sc_memory_params_clear(scParams);


            //sc_memory_shutdown
            bool isShutDown = NativeMethods.sc_memory_shutdown(false);
            if (isShutDown == true)
            {
                scMemoryContext = IntPtr.Zero;
            };
            Assert.IsTrue(isShutDown);
        }
















    }
}
