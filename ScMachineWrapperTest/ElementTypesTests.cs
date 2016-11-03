using System;
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
    public class ElementTypesTest
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

      
        //Unknown = 0x00,
        //Node_a = 0x1,
        //Link_a = 0x2,
        //CommonEdge_a = 0x4,
        //CommonArc_a = 0x8,
        //AccessArc_a = 0x10,
        //Constant_a = 0x20,
        //Variable_a = 0x40,
        //PositiveArc_a = 0x80,
        //NegativeArc_a = 0x100,
        //FuzzyArc_a = 0x200,
        //TemporaryArc_a = 0x400,
        //PermanentArc_a = 0x800,
        //TupleNode_a = 0x80,
        //StructureNode_a = 0x100,
        //RoleNode_a = 0x200,
        //NonRoleNode_a = 0x400,
        //NonRoleConstantNode_c = (NonRoleNode_a | Constant_a|Node_a),
        //ClassNode_a = 0x800,
        //ClassConstantNode_c = (ClassNode_a  | Constant_a),
        //AbstractNode_a = 0x1000,
        //MaterialNode_a = 0x2000,
        //ConstantNode_c = Node_a | Constant_a,
        //PositiveConstantPermanentAccessArc_c = (AccessArc_a | Constant_a | PositiveArc_a | PermanentArc_a),
        //ConstantCommonArc_c = (CommonArc_a | Constant_a),
        //AnyElementMask_c = (Node_a | Link_a | CommonEdge_a | CommonArc_a | AccessArc_a),
        //ConstantOrVariableMask_c = (Constant_a | Variable_a),
        //PermanencyMask_c = (PermanentArc_a | TemporaryArc_a),
        //NodeOrStructureMask_c = (TupleNode_a | StructureNode_a | RoleNode_a | NonRoleNode_a | ClassNode_a | AbstractNode_a | MaterialNode_a),
        //ArcMask_c = (AccessArc_a | CommonArc_a | CommonEdge_a),



        [TestMethod()]
        public void IsTypeTest()
        {
          Assert.IsTrue(ElementType.ClassConstantNode_c.IsType(ElementType.ClassNode_a));
          Assert.IsTrue(ElementType.ClassConstantNode_c.IsType(ElementType.PermanentArc_a));
        }

        [TestMethod()]
        public void HasAnyTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTypeTest()
        {
            Assert.Fail();
        }

    }
}
