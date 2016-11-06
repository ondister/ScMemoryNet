using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScMemoryNet;
using ScEngineNet;
using ScEngineNet.ScElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet.NetHelpers;

namespace ScEngineNetTest
{

    [TestClass]
    public class IteratorsTests
    {
        static ScNode node;
        static ScLink link;
        static ScNode nrelNode;
        static ScArc commonArc;
        static ScArc nrelArc;
        const string configFile = @"d:\OSTIS\sc-machine-master\bin\sc-memory.ini";
        const string repoPath = @"d:\OSTIS\sc-machine-master\bin\repo";
        const string extensionPath = @"d:\OSTIS\sc-machine-master\bin\extensions";
        const string netExtensionPath = "";
        static ScMemoryContext context;


        #region InitializeMemory

        [ClassInitialize]
        public static void InitializeMemory(TestContext testContext)
        {
            if (!ScMemory.IsInitialized) { ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath); }
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
            if (ScMemory.IsInitialized) { ScMemory.ShutDown(true); }
        }
        #endregion

        #region TestIterators

        [TestMethod]
        public void TestIterator3FAA()
        {
            var iterator3FAA = context.CreateIterator(nrelNode, nrelArc.ElementType, commonArc.ElementType);

            Assert.IsTrue(iterator3FAA.Count() != 0);
            foreach (var construction in iterator3FAA)
            {
                Assert.AreEqual(nrelNode.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[2].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator3AAF()
        {
            var iterator3AAF = context.CreateIterator(nrelNode.ElementType, nrelArc.ElementType, commonArc);

            Assert.IsTrue(iterator3AAF.Count() != 0);
            foreach (var construction in iterator3AAF)
            {
                Assert.AreEqual(nrelNode.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[2].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator3FAF()
        {
            var iterator3FAF = context.CreateIterator(nrelNode, nrelArc.ElementType, commonArc);

            Assert.IsTrue(iterator3FAF.Count() != 0);
            foreach (var construction in iterator3FAF)
            {
                Assert.AreEqual(nrelNode.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[2].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5FAFAF()
        {
            var iterator5FAFAF = context.CreateIterator(node, ScTypes.ArcCommonConstant, link,
                                                        ScTypes.ArcAccessConstantPositivePermanent, nrelNode);

            Assert.IsTrue(iterator5FAFAF.Count() != 0);

            foreach (var construction in iterator5FAFAF)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);

            }
        }

        [TestMethod]
        public void TestIterator5FAAAF()
        {
            var iterator5FAAAF = context.CreateIterator(node, commonArc.ElementType, link.ElementType,
                                                        nrelArc.ElementType, nrelNode);

            Assert.IsTrue(iterator5FAAAF.Count() != 0);

            foreach (var construction in iterator5FAAAF)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5AAFAF()
        {
            var iterator5AAFAF = context.CreateIterator(node.ElementType, commonArc.ElementType, link,
                                                        nrelArc.ElementType, nrelNode);

            Assert.IsTrue(iterator5AAFAF.Count() != 0);

            foreach (var construction in iterator5AAFAF)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5FAFAA()
        {
            var iterator5FAFAA = context.CreateIterator(node, commonArc.ElementType, link,
                                                        nrelArc.ElementType, nrelNode.ElementType);

            Assert.IsTrue(iterator5FAFAA.Count() != 0);

            foreach (var construction in iterator5FAFAA)
            {
                Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                Assert.AreEqual(nrelNode.ScAddress, construction[4].ScAddress);
            }
        }

        [TestMethod]
        public void TestIterator5FAAAA()
        {
            var iterator5FAAAA = context.CreateIterator(node, commonArc.ElementType, link.ElementType,
                                                        nrelArc.ElementType, nrelNode.ElementType);

            Assert.IsTrue(iterator5FAAAA.Count() != 0);
            if (iterator5FAAAA.Count() == 1)
            {
                foreach (var construction in iterator5FAAAA)
                {
                    Assert.AreEqual(node.ScAddress, construction[0].ScAddress);
                    Assert.AreEqual(commonArc.ScAddress, construction[1].ScAddress);
                    Assert.AreEqual(link.ScAddress, construction[2].ScAddress);
                    Assert.AreEqual(nrelArc.ScAddress, construction[3].ScAddress);
                }
            }
        }

        [TestMethod]
        public void TestIterator5AAFAA()
        {
            var iterator5AAFAA = context.CreateIterator(node.ElementType, commonArc.ElementType, link,
                                                        nrelArc.ElementType, nrelNode.ElementType);
            
                Assert.IsTrue(iterator5AAFAA.Count() != 0);

                foreach (var construction in iterator5AAFAA)
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
