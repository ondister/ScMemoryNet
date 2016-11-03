﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScEngineNet.SafeElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScMemoryNet;
using ScEngineNet.NetHelpers;

namespace ScEngineNetTest
{
    [TestClass()]
    public class ScElementTests
    {

        static ScNode node;
        static ScNode node1;
        static ScNode node2;
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
            node2 = context.FindNode("test_construction_node");
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
            node2.Dispose();
            context.Dispose();
            ScMemory.ShutDown(true);
        }
        #endregion

        
        [TestMethod()]
        public void DeleteFromMemoryTest()
        {
            var node3 = context.CreateNode(ElementType.ClassConstantNode_c);
            Assert.IsTrue(node3.IsValid);

           var result= node3.DeleteFromMemory();
           var result1=node3.DeleteFromMemory();
           Assert.AreEqual(ScResult.SC_RESULT_OK, result);
           Assert.AreEqual(ScResult.SC_RESULT_ERROR, result1);

            Assert.IsFalse(node3.IsValid);
            Assert.IsNotNull(node3);

            node3.Dispose();
            
        }

        [TestMethod()]
        public void ChangeSubTypeTest()
        {
            ElementType subType = ElementType.Constant_a;
            var node3= context.CreateNode(ElementType.Node_a);
            Assert.AreEqual(ElementType.Node_a, node3.ElementType);

            node3.ChangeSubType(subType);
            Assert.AreEqual(ElementType.ConstantNode_c, node3.ElementType);
        }

        [TestMethod()]
        public void AddInputArcTest()
        {
          var arc=  node1.AddInputArc(ElementType.PositiveConstantPermanentAccessArc_c, node);
          
          Assert.AreEqual(node, arc.BeginElement);
          Assert.AreEqual(ElementType.PositiveConstantPermanentAccessArc_c, arc.ElementType);
          Assert.AreEqual(node, arc.BeginElement);
          Assert.AreEqual(node1, arc.EndElement);
          arc.DeleteFromMemory();
          arc.Dispose();

        }

        [TestMethod()]
        public void AddOutputArcTest()
        {
            var arc = node.AddOutputArc(node1,ElementType.PositiveConstantPermanentAccessArc_c);

            Assert.AreEqual(node, arc.BeginElement);
            Assert.AreEqual(ElementType.PositiveConstantPermanentAccessArc_c, arc.ElementType);
            Assert.AreEqual(node, arc.BeginElement);
            Assert.AreEqual(node1, arc.EndElement);
            arc.DeleteFromMemory();
            arc.Dispose();
        }

        [TestMethod()]
        public void GetElementByNrelClassTest()
        {
            string idtf = "main_node_idtf";
            node.MainIdentifiers[ScDataTypes.Instance.LanguageRu] = idtf;

            var main_idtf_link = (ScLink)node.GetElementByNrelClass(context.FindNode(ScKeyNodes.Instance.NrelMainIdtf), context.FindNode(ScDataTypes.Instance.LanguageRu), ElementType.Link_a);
            Assert.IsNotNull(main_idtf_link);
           
            var text = ((ScString)main_idtf_link.LinkContent).Value;

            Assert.AreEqual(idtf,text);
        
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsNotNull(node);
            Assert.IsFalse(node == node1);
            Assert.IsTrue(node != node1);
            ScElement element = null;
            Assert.IsTrue(element == null);

            object obj = (object)node;
            Assert.AreEqual(node, (ScNode)obj);
      
            Assert.AreEqual(node, node2);

        }


        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(Convert.ToInt32(node.ScAddress.Offset.ToString() + node.ScAddress.Segment.ToString()), node.GetHashCode());
        }

        [TestMethod()]
        public void DisposeTest()
        {
            var node3 = context.CreateNode(ElementType.ClassConstantNode_c);
            Assert.IsTrue(node3.IsValid);
            Assert.IsFalse(node3.Disposed);

            node3.Dispose();
            Assert.IsTrue(node3.Disposed);
        }
    }
}