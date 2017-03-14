using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest
{
    [TestClass]
    public class IteratorsTests
    {
        private static ScNode node;
        private static ScLink link;
        private static ScNode nrelNode;
        private static ScArc commonArc;
        private static ScArc nrelArc;
        private static ScMemoryContext context;

        #region InitializeMemory

        [ClassInitialize]
        public static void InitializeMemory(TestContext testContext)
        {
            if (!ScMemory.IsInitialized)
            {
                ScMemory.Initialize(true, TestParams.ConfigFile, TestParams.RepoPath, TestParams.ExtensionPath,
                    TestParams.NetExtensionPath);
            }
            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            //создаем элементы
            node = context.CreateNode(ScTypes.NodeConstant);
            node.SystemIdentifier = "test_construction_node";
            link = context.CreateLink("link");
            nrelNode = context.CreateNode(ScTypes.NodeConstantNonRole);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            nrelArc = commonArc.AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, nrelNode);

            Assert.IsTrue(node.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.NodeConstant, node.ElementType);

            Assert.IsTrue(link.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.Link, link.ElementType);

            Assert.IsTrue(nrelNode.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.NodeConstantNonRole, nrelNode.ElementType);

            Assert.IsTrue(commonArc.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.ArcCommonConstant, commonArc.ElementType);

            Assert.IsTrue(nrelArc.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.ArcAccessConstantPositivePermanent, nrelArc.ElementType);
        }

        #endregion

        #region ShutDownMemory

        [ClassCleanup]
        public static void ShutDown()
        {
            node.Dispose();
            link.Dispose();
            nrelNode.Dispose();
            commonArc.Dispose();
            nrelArc.Dispose();

            context.Dispose();
            if (ScMemory.IsInitialized)
            {
                ScMemory.ShutDown(true);
            }
        }

        #endregion

        #region TestIterators

        [TestMethod]
        public void TestIterator3Faa()
        {
            var iterator3Faa = context.CreateIterator(nrelNode, nrelArc.ElementType, commonArc.ElementType);

            Assert.IsTrue(iterator3Faa.Count() != 0);
            foreach (var construction in iterator3Faa)
            {
                Assert.AreEqual(nrelNode.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[2].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator3Aaf()
        {
            var iterator3Aaf = context.CreateIterator(nrelNode.ElementType, nrelArc.ElementType, commonArc);

            Assert.IsTrue(iterator3Aaf.Count() != 0);
            foreach (var construction in iterator3Aaf)
            {
                Assert.AreEqual(nrelNode.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[2].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator3Faf()
        {
            var iterator3Faf = context.CreateIterator(nrelNode, nrelArc.ElementType, commonArc);

            Assert.IsTrue(iterator3Faf.Count() != 0);
            foreach (var construction in iterator3Faf)
            {
                Assert.AreEqual(nrelNode.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[2].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5Fafaf()
        {
            var iterator5Fafaf = context.CreateIterator(node, ScTypes.ArcCommonConstant, link,
                ScTypes.ArcAccessConstantPositivePermanent, nrelNode);

            Assert.IsTrue(iterator5Fafaf.Count() != 0);

            foreach (var construction in iterator5Fafaf)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5Faaaf()
        {
            var iterator5Faaaf = context.CreateIterator(node, commonArc.ElementType, link.ElementType,
                nrelArc.ElementType, nrelNode);

            Assert.IsTrue(iterator5Faaaf.Count() != 0);

            foreach (var construction in iterator5Faaaf)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5Aafaf()
        {
            var iterator5Aafaf = context.CreateIterator(node.ElementType, commonArc.ElementType, link,
                nrelArc.ElementType, nrelNode);

            Assert.IsTrue(iterator5Aafaf.Count() != 0);

            foreach (var construction in iterator5Aafaf)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5Fafaa()
        {
            var iterator5Fafaa = context.CreateIterator(node, commonArc.ElementType, link,
                nrelArc.ElementType, nrelNode.ElementType);

            Assert.IsTrue(iterator5Fafaa.Count() != 0);

            foreach (var construction in iterator5Fafaa)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5Faaaa()
        {
            var iterator5Faaaa = context.CreateIterator(node, commonArc.ElementType, link.ElementType,
                nrelArc.ElementType, nrelNode.ElementType);

            Assert.IsTrue(iterator5Faaaa.Count() != 0);
            if (iterator5Faaaa.Count() == 1)
            {
                foreach (var construction in iterator5Faaaa)
                {
                    Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                    Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                    Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                    Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                }
            }
        }

        [TestMethod]
        public void TestIterator5Aafaa()
        {
            var iterator5Aafaa = context.CreateIterator(node.ElementType, commonArc.ElementType, link,
                nrelArc.ElementType, nrelNode.ElementType);

            Assert.IsTrue(iterator5Aafaa.Count() != 0);

            foreach (var construction in iterator5Aafaa)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        #endregion
    }
}