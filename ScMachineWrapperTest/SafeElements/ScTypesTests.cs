using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest.SafeElements
{
    [TestClass]
    public class ScTypesTests
    {
        [TestMethod]
        public void IsArcTest()
        {
            Assert.IsFalse(ScTypes.NodeConstant.IsArc);

            Assert.IsTrue(ScTypes.ArcAccess.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessConstantFuzzyPermanent.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessConstantFuzzyTemporary.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessConstantNegativePermanent.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessConstantNegativeTemporary.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessConstantPositivePermanent.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessConstantPositiveTemporary.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessVariableFuzzyPermanent.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessVariableFuzzyTemporary.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessVariableNegativePermanent.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessVariableNegativeTemporary.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessVariablePositivePermanent.IsArc);
            Assert.IsTrue(ScTypes.ArcAccessVariablePositiveTemporary.IsArc);
            Assert.IsTrue(ScTypes.ArcCommon.IsArc);
            Assert.IsTrue(ScTypes.ArcCommonConstant.IsArc);
            Assert.IsTrue(ScTypes.ArcCommonVariable.IsArc);
            Assert.IsTrue(ScTypes.EdgeCommon.IsArc);
            Assert.IsTrue(ScTypes.EdgeCommonConstant.IsArc);
            Assert.IsTrue(ScTypes.EdgeCommonVariable.IsArc);
        }

        [TestMethod]
        public void IsLinkTest()
        {
            Assert.IsFalse(ScTypes.NodeConstant.IsLink);

            Assert.IsTrue(ScTypes.Link.IsLink);
        }

        [TestMethod]
        public void IsNodeTest()
        {
            Assert.IsFalse(ScTypes.Link.IsNode);
            Assert.IsFalse(ScTypes.ArcCommon.IsNode);

            Assert.IsTrue(ScTypes.Node.IsNode);
            Assert.IsTrue(ScTypes.NodeConstant.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantAbstract.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantClass.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantMaterial.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantNonRole.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantRole.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantStructure.IsNode);
            Assert.IsTrue(ScTypes.NodeConstantTuple.IsNode);
            Assert.IsTrue(ScTypes.NodeVariable.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableAbstract.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableClass.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableMaterial.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableNonRole.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableRole.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableStructure.IsNode);
            Assert.IsTrue(ScTypes.NodeVariableTuple.IsNode);
        }

        [TestMethod]
        public void IsConstantTest()
        {
            Assert.IsFalse(ScTypes.ArcCommonVariable.IsConstant);
            Assert.IsFalse(ScTypes.EdgeCommonVariable.IsConstant);
            Assert.IsFalse(ScTypes.NodeVariable.IsConstant);

            Assert.IsTrue(ScTypes.ArcAccessConstantFuzzyPermanent.IsConstant);
            Assert.IsTrue(ScTypes.ArcAccessConstantFuzzyTemporary.IsConstant);
            Assert.IsTrue(ScTypes.ArcAccessConstantNegativePermanent.IsConstant);
            Assert.IsTrue(ScTypes.ArcAccessConstantNegativeTemporary.IsConstant);
            Assert.IsTrue(ScTypes.ArcAccessConstantPositivePermanent.IsConstant);
            Assert.IsTrue(ScTypes.ArcAccessConstantPositiveTemporary.IsConstant);
            Assert.IsTrue(ScTypes.ArcCommonConstant.IsConstant);
            Assert.IsTrue(ScTypes.EdgeCommonConstant.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstant.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantAbstract.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantClass.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantMaterial.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantNonRole.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantRole.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantStructure.IsConstant);
            Assert.IsTrue(ScTypes.NodeConstantTuple.IsConstant);
        }

        [TestMethod]
        public void IsVariableTest()
        {
            Assert.IsFalse(ScTypes.ArcCommonConstant.IsVariable);
            Assert.IsFalse(ScTypes.EdgeCommonConstant.IsVariable);
            Assert.IsFalse(ScTypes.NodeConstant.IsVariable);

            Assert.IsTrue(ScTypes.ArcAccessVariableFuzzyPermanent.IsVariable);
            Assert.IsTrue(ScTypes.ArcAccessVariableFuzzyTemporary.IsVariable);
            Assert.IsTrue(ScTypes.ArcAccessVariableNegativePermanent.IsVariable);
            Assert.IsTrue(ScTypes.ArcAccessVariableNegativeTemporary.IsVariable);
            Assert.IsTrue(ScTypes.ArcAccessVariablePositivePermanent.IsVariable);
            Assert.IsTrue(ScTypes.ArcAccessVariablePositiveTemporary.IsVariable);
            Assert.IsTrue(ScTypes.ArcCommonVariable.IsVariable);
            Assert.IsTrue(ScTypes.EdgeCommonVariable.IsVariable);

            Assert.IsTrue(ScTypes.NodeVariable.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableAbstract.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableClass.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableMaterial.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableNonRole.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableRole.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableStructure.IsVariable);
            Assert.IsTrue(ScTypes.NodeVariableTuple.IsVariable);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsTrue(ScTypes.NodeVariableRole != ScTypes.NodeConstantNonRole);
            Assert.AreEqual(ScTypes.NodeVariableRole, ScTypes.NodeVariableRole);
            var type1 = ScTypes.ArcAccess;
            var type2 = ScTypes.ArcAccess;
            var type3 = ScTypes.NodeConstant;
            ScTypes type4 = null;

            Assert.AreEqual(type1, type2);
            Assert.AreNotEqual(type1, type3);

            Assert.IsNull(type4);
            Assert.AreNotEqual(null, type3);
        }
    }
}