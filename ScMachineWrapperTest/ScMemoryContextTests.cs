using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScEngineNet.LinkContent;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest
{
    [TestClass]
    public class ScMemoryContextTests
    {
        private static ScNode node;
        private static ScNode node1;
        private static ScLink link;
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
            node1 = context.CreateNode(ScTypes.NodeConstant);
            link = context.CreateLink("link");

            Assert.IsTrue(node.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.NodeConstant, node.ElementType);

            Assert.IsTrue(link.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.Link, link.ElementType);
        }

        #endregion

        #region ShutDownMemory

        [ClassCleanup]
        public static void ShutDown()
        {
            node.Dispose();
            link.Dispose();
            node1.Dispose();
            context.Dispose();
            if (ScMemory.IsInitialized)
            {
                ScMemory.ShutDown(true);
            }
        }

        #endregion

        [TestMethod]
        public void IsMemoryInitializedTest()
        {
            Assert.IsTrue(ScMemoryContext.IsMemoryInitialized());
        }

        [TestMethod]
        public void GetStatisticsTest()
        {
            var stat = context.GetStatistics();

            Assert.IsTrue(stat.NodeCount != 0);
            Assert.IsTrue(stat.ArcCount != 0);
            Assert.IsTrue(stat.LinkCount != 0);
        }

        [TestMethod]
        public void SaveStateTest()
        {
            var result = context.SaveState();

            Assert.AreEqual(ScResult.ScResultOk, result);
        }

        [TestMethod]
        public void CreateUniqueIdentifierTest()
        {
            Assert.IsTrue(context.CreateUniqueIdentifier(node).ToString().Length != 0);

            var node4 = context.CreateNode(ScTypes.Node);
            var node5 = context.CreateNode(ScTypes.Node);
            node4.DeleteFromMemory();
            node5.DeleteFromMemory();
            Assert.AreEqual(context.CreateUniqueIdentifier(node4), context.CreateUniqueIdentifier(node5));
            //пока в базе такого идентификатора нет, то на основе одного и того же узла будут одинаковые идентификаторы
            Assert.AreEqual(context.CreateUniqueIdentifier(node), context.CreateUniqueIdentifier(node));

            node.SystemIdentifier = context.CreateUniqueIdentifier(node);
            //как только такой идентификатор уже в базе, на основе этого узла будет создан другой
            Assert.AreNotEqual(node.SystemIdentifier, context.CreateUniqueIdentifier(node));
        }

        [TestMethod]
        public void CreateUniqueIdentifierTest1()
        {
            Assert.IsTrue(context.CreateUniqueIdentifier(ScNode.InstancePreffix, node).ToString().Length != 0);
        }

        [TestMethod]
        public void IsElementExistTest()
        {
            Assert.IsTrue(context.IsElementExist(node.ScAddress));
            var node4 = context.CreateNode(ScTypes.Node);
            node4.DeleteFromMemory();
            Assert.IsFalse(context.IsElementExist(node4.ScAddress));
            node4.Dispose();
        }

        [TestMethod]
        public void CreateArcTest()
        {
            var arc = context.CreateArc(node, node1, ScTypes.ArcAccessConstantPositivePermanent);
            var arc1 = context.CreateArc(node, node1, ScTypes.ArcAccessConstantPositivePermanent);
            //метод при дублировании такой же дуги должен просто возвращать ссылку на дугу, но не создавать новую
            Assert.AreEqual(arc, arc1);
            Assert.AreEqual(node, arc.BeginElement);
            Assert.AreEqual(node1, arc.EndElement);
            Assert.AreEqual(ScTypes.ArcAccessConstantPositivePermanent, arc.ElementType);

            //проверяем на null
            var arcNull = context.CreateArc(null, null, ScTypes.ArcAccessConstantPositivePermanent);
            Assert.IsNull(arcNull);

            arc.DeleteFromMemory();

            arc.Dispose();
            arc1.Dispose();
        }

        [TestMethod]
        public void FindArcTest()
        {
            var arc = context.CreateArc(node, node1, ScTypes.ArcAccessConstantPositivePermanent);
            var findedArc = context.FindArc(arc.ScAddress);

            Assert.AreEqual(arc, findedArc);

            arc.DeleteFromMemory();
            findedArc.Dispose();
            arc.Dispose();
        }

        [TestMethod]
        public void ArcIsExistTest()
        {
            var arc = context.CreateArc(node, node1, ScTypes.ArcAccessConstantPositivePermanent);
            var arcIsExist = context.ArcIsExist(node, node1, ScTypes.ArcAccessConstantPositivePermanent);

            Assert.IsTrue(arcIsExist);

            var arcIsExistNull = context.ArcIsExist(null, null, ScTypes.ArcAccessConstantPositivePermanent);
            Assert.IsFalse(arcIsExistNull);

            context.ArcIsExist(node, node1, ScTypes.ArcAccessVariableFuzzyPermanent);
            Assert.IsFalse(arcIsExistNull);

            arc.DeleteFromMemory();
            arc.Dispose();
        }

        [TestMethod]
        public void CreateNodeTest()
        {
            var node5 = context.CreateNode(ScTypes.Node);
            Assert.IsNotNull(node5);
            Assert.AreEqual(ScTypes.Node, node5.ElementType);
            node5.DeleteFromMemory();
            node5.Dispose();
        }

        [TestMethod]
        public void CreateNodeTestIdentifiers()
        {
            Identifier sysId = "test_node";
            Identifier ruId = "тестовая нода";
            Identifier enId = "testing node";
            var node5 = context.CreateNode(ScTypes.Node, sysId, ruId, enId);
            Assert.IsNotNull(node5);
            Assert.AreEqual(ScTypes.Node, node5.ElementType);

            Assert.AreEqual(sysId, node5.SystemIdentifier);
            Assert.AreEqual(ruId, ((ScString) node5.MainIdentifiers[ScDataTypes.Instance.LanguageRu]).Value);
            Assert.AreEqual(enId, ((ScString) node5.MainIdentifiers[ScDataTypes.Instance.LanguageEn]).Value);

            node5.DeleteFromMemory();
            node5.Dispose();
        }

        [TestMethod]
        public void FindNodeTest()
        {
            var findedNode = context.FindNode(node.ScAddress);
            Assert.AreEqual(node, findedNode);

            var findedNodeById = context.FindNode(node.SystemIdentifier);
            Assert.AreEqual(node, findedNodeById);

            //у node1 нет системного идентификатора
            var findedNodeByInvalidId = context.FindNode(node1.SystemIdentifier);
            Assert.AreNotEqual(node1, findedNodeByInvalidId);
            Assert.IsNull(findedNodeByInvalidId);
        }

        [TestMethod]
        public void CreateLinkTest()
        {
            ScString content = "Test_content_1234Тестовый контент";
            var testLink = context.CreateLink(content);
            var expectedClassNode = testLink.LinkContent.ClassNodeIdentifier;
            Assert.IsNotNull(testLink);
            Assert.AreEqual(ScTypes.Link, testLink.ElementType);
            var classnode = testLink.LinkContent.ClassNodeIdentifier;
            Assert.AreEqual(expectedClassNode, classnode);
            Assert.IsTrue(content == ((ScString) testLink.LinkContent));

            //проверка создания ссылки с дублирующим контентом.

            var testLink1 = context.CreateLink(content);

            var links = context.FindLinks(content);
            Assert.AreEqual(1, links.Count);

            //проверка создания пустой ссылки
            var linkVoid = context.CreateLink();
            Assert.IsNotNull(linkVoid);


            testLink.DeleteFromMemory();
            testLink.Dispose();
            testLink1.DeleteFromMemory();
            testLink1.Dispose();
        }

        [TestMethod]
        public void FindLinkTest()
        {
            ScString content = "Test_content_1234Тестовый контент";

            var testLink = context.CreateLink(content);
            var findedTestLink = context.FindLink(testLink.ScAddress);
            Assert.AreEqual(testLink, findedTestLink);
            Assert.AreEqual(content, ((ScString) findedTestLink.LinkContent));

            testLink.DeleteFromMemory();
            testLink.Dispose();

            findedTestLink.DeleteFromMemory();
            findedTestLink.Dispose();
        }

        [TestMethod]
        public void FindLinksTest()
        {
            ScString content = "FindLinksTestContent Контент тесового метода 12345";
            var testLink = context.CreateLink(content);
            var links = context.FindLinks(content);

            Assert.AreEqual(1, links.Count);

            foreach (var scLink in links)
            {
                Assert.AreEqual(content, ((ScString) scLink.LinkContent));
            }

            testLink.DeleteFromMemory();
            testLink.Dispose();
        }

        [TestMethod]
        public void DisposeTest()
        {
            var testContext = new ScMemoryContext(ScAccessLevels.MedLevel);
            Assert.IsFalse(testContext.Disposed);

            testContext.Dispose();

            Assert.IsTrue(testContext.Disposed);
        }

        [TestMethod]
        public void MultyThreadCreationTest()
        {
            var thread1 = new Thread(delegate()
            {
                using (var memoryContext = new ScMemoryContext(ScAccessLevels.MinLevel))
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        var scLink = memoryContext.CreateLink();
                        Assert.IsNotNull(scLink);

                        scLink.Dispose();
                    }
                }
            });

            var thread2 = new Thread(delegate()
            {
                using (var memoryContext = new ScMemoryContext(ScAccessLevels.MinLevel))
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        var scNode = memoryContext.CreateNode(ScTypes.Node);
                        Assert.IsNotNull(scNode);

                        var scLink = memoryContext.CreateLink();
                        Assert.IsNotNull(scLink);

                        var arc = memoryContext.CreateArc(scNode, scLink, ScTypes.ArcAccessConstantPositivePermanent);
                        Assert.IsNotNull(arc);

                        arc.Dispose();

                        scNode.Dispose();

                        scLink.Dispose();
                    }
                }
            });

            var thread3 = new Thread(delegate()
            {
                using (var memoryContext = new ScMemoryContext(ScAccessLevels.MinLevel))
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        var scNode = memoryContext.CreateNode(ScTypes.Node);
                        Assert.IsNotNull(scNode);

                        scNode.Dispose();
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread3.Start();
        }
    }
}