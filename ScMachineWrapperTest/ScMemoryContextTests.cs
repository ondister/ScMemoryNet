﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScEngineNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet.SafeElements;
using ScMemoryNet;
using ScEngineNet.NetHelpers;
namespace ScEngineNet.Tests
{
    [TestClass()]
    public class ScMemoryContextTests
    {
        static ScNode node;
        static ScNode node1;
        static ScLink link;
        const string configFile = @"d:\OSTIS\sc-machine-master\bin\sc-memory.ini";
        const string repoPath = @"d:\OSTIS\sc-machine-master\bin\repo";
        const string extensionPath = @"d:\OSTIS\sc-machine-master\bin\extensions";
        const string netExtensionPath = "";
        static ScMemoryContext context;


        #region InitializeMemory

        [ClassInitialize]
        public static void InitializeMemory(TestContext testContext)
        {
            ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);
            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            //создаем элементы
            node = context.CreateNode(ElementType.ConstantNode_c);
            node.SystemIdentifier = "test_construction_node";
            node1 = context.CreateNode(ElementType.ConstantNode_c);
            link = context.CreateLink("link");

            Assert.IsTrue(node.ScAddress.IsValid);
            Assert.AreEqual(ElementType.ConstantNode_c, node.ElementType);

            Assert.IsTrue(link.ScAddress.IsValid);
            Assert.AreEqual(ElementType.Link_a, link.ElementType);

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
            ScMemory.ShutDown(true);
        }
        #endregion



        [TestMethod()]
        public void IsMemoryInitializedTest()
        {
            Assert.IsTrue(ScMemoryContext.IsMemoryInitialized());
        }

      
        [TestMethod()]
        public void GetStatisticsTest()
        {
            ScStat stat = context.GetStatistics();

            Assert.IsTrue(stat.NodeCount != 0);
            Assert.IsTrue(stat.ArcCount != 0);
            Assert.IsTrue(stat.LinkCount != 0);
        }

        [TestMethod()]
        public void SaveStateTest()
        {
            var result = context.SaveState();

            Assert.AreEqual(ScResult.SC_RESULT_OK, result);
        }

        [TestMethod()]
        public void CreateUniqueIdentifierTest()
        {
            Assert.IsTrue(context.CreateUniqueIdentifier(node).ToString().Length != 0);

            var node4 = context.CreateNode(ElementType.Node_a);
            var node5 = context.CreateNode(ElementType.Node_a);
            node4.DeleteFromMemory();
            node5.DeleteFromMemory();
            Assert.AreEqual(context.CreateUniqueIdentifier(node4), context.CreateUniqueIdentifier(node5));
            //пока в базе такого идентификатора нет, то на основе одного и того же узла будут одинаковые идентификаторы
            Assert.AreEqual(context.CreateUniqueIdentifier(node), context.CreateUniqueIdentifier(node));

            node.SystemIdentifier = context.CreateUniqueIdentifier(node);
            //как только такой идентификатор уже в базе, на основе этого узла будет создан другой
            Assert.AreNotEqual(node.SystemIdentifier, context.CreateUniqueIdentifier(node));
        
        }

        [TestMethod()]
        public void CreateUniqueIdentifierTest1()
        {
            Assert.IsTrue(context.CreateUniqueIdentifier(ScNode.InstancePreffix,node).ToString().Length != 0);
        }

        [TestMethod()]
        public void IsElementExistTest()
        {
            Assert.IsTrue(context.IsElementExist(node.ScAddress));
            var node4 = context.CreateNode(ElementType.Node_a);
            node4.DeleteFromMemory();
            Assert.IsFalse(context.IsElementExist(node4.ScAddress));
            node4.Dispose();
        }

        [TestMethod()]
        public void CreateArcTest()
        {
            var arc = context.CreateArc(node, node1, ElementType.PositiveConstantPermanentAccessArc_c);
            var arc1 = context.CreateArc(node, node1, ElementType.PositiveConstantPermanentAccessArc_c);
            //метод при дублировании такой же дуги должен просто возвращать ссылку на дугу, но не создавать новую
            Assert.AreEqual(arc, arc1);
            Assert.AreEqual(node, arc.BeginElement);
            Assert.AreEqual(node1, arc.EndElement);
            Assert.AreEqual(ElementType.PositiveConstantPermanentAccessArc_c, arc.ElementType);

            //проверяем на null
            var arcNull = context.CreateArc(null, null, ElementType.PositiveConstantPermanentAccessArc_c);
            Assert.IsNull(arcNull);

            arc.DeleteFromMemory();

            arc.Dispose();
            arc1.Dispose();

        }

        [TestMethod()]
        public void FindArcTest()
        {
            var arc = context.CreateArc(node, node1, ElementType.PositiveConstantPermanentAccessArc_c);
            var findedArc= context.FindArc(arc.ScAddress);
           
            Assert.AreEqual(arc, findedArc);

            arc.DeleteFromMemory();
            findedArc.Dispose();
            arc.Dispose();
        }

        [TestMethod()]
        public void ArcIsExistTest()
        {
            var arc = context.CreateArc(node, node1, ElementType.PositiveConstantPermanentAccessArc_c);
            bool arcIsExist = context.ArcIsExist(node, node1, ElementType.PositiveConstantPermanentAccessArc_c);

            Assert.IsTrue(arcIsExist);

            bool arcIsExistNull = context.ArcIsExist(null, null, ElementType.PositiveConstantPermanentAccessArc_c);
            Assert.IsFalse(arcIsExistNull);

            bool unexistensArc = context.ArcIsExist(node, node1, ElementType.PermanentArc_a);
            Assert.IsFalse(arcIsExistNull);

            arc.DeleteFromMemory();
            arc.Dispose();
        }

        [TestMethod()]
        public void CreateNodeTest()
        {
            var node5 = context.CreateNode(ElementType.ClassNode_a);
            Assert.IsNotNull(node5);
            Assert.AreEqual(ElementType.ClassNode_a, node5.ElementType);
            node5.DeleteFromMemory();
            node5.Dispose();
        }

        [TestMethod()]
        public void CreateNodeTestIdentifiers()
        {
            Identifier sysId="test_node";
            Identifier ruId="тестовая нода";
            Identifier enId="testing node";
            var node5 = context.CreateNode(ElementType.Node_a, sysId, ruId, enId);
            Assert.IsNotNull(node5);
            Assert.AreEqual(ElementType.Node_a, node5.ElementType);

            Assert.AreEqual(sysId, node5.SystemIdentifier);
            Assert.AreEqual(ruId, ((ScString)node5.MainIdentifiers[ScDataTypes.Instance.LanguageRu]).Value);
            Assert.AreEqual(enId, ((ScString)node5.MainIdentifiers[ScDataTypes.Instance.LanguageEn]).Value);

            node5.DeleteFromMemory();
            node5.Dispose();
        }


        [TestMethod()]
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

     

        [TestMethod()]
        public void CreateLinkTest()
        {
            ScString content = "Test_content_1234Тестовый контент";
            var testLink = context.CreateLink(content);
            var expectedClassNode = testLink.LinkContent.ClassNodeIdentifier;
            Assert.IsNotNull(testLink);
            Assert.AreEqual(ElementType.Link_a, testLink.ElementType);
         var classnode=   testLink.LinkContent.ClassNodeIdentifier;
         Assert.AreEqual(expectedClassNode, classnode);
         Assert.AreEqual(content.Value, ((ScString)testLink.LinkContent).Value);
        }

       

        [TestMethod()]
        public void FindLinkTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindLinksTest()
        {
            Assert.Fail();
        }

      

        [TestMethod()]
        public void DisposeTest()
        {
            Assert.Fail();
        }
    }
}
