using ScEngineNet.Native;
using System;

namespace ScEngineNet.ScElements
{
    public class ScTypes : IEquatable<ScTypes>
    {
        private ElementTypes elementType;

        internal ElementTypes ElementType
        {
            get { return elementType; }
        }

        internal ScTypes(ElementTypes elementType)
        {
            this.elementType = elementType;
        }

        #region StaticProperties
        public static ScTypes UnKnown
        { get { return new ScTypes(ElementTypes.Unknown); } }
        public static ScTypes EdgeCommon
        { get { return new ScTypes(ElementTypes.CommonEdge_a); } }

        public static ScTypes ArcCommon
        { get { return new ScTypes(ElementTypes.CommonArc_a); } }

        public static ScTypes EdgeCommonConstant
        { get { return new ScTypes(ElementTypes.CommonEdge_a | ElementTypes.Constant_a); } }

        public static ScTypes ArcCommonConstant
        { get { return new ScTypes(ElementTypes.CommonArc_a | ElementTypes.Constant_a); } }

        public static ScTypes ArcAccess
        { get { return new ScTypes(ElementTypes.AccessArc_a); } }

        public static ScTypes ArcAccessConstantPositivePermanent
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Constant_a | ElementTypes.PositiveArc_a | ElementTypes.PermanentArc_a); } }

        public static ScTypes ArcAccessConstantNegativePermanent
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Constant_a | ElementTypes.NegativeArc_a | ElementTypes.PermanentArc_a); } }

        public static ScTypes ArcAccessConstantFuzzyPermanent
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Constant_a | ElementTypes.FuzzyArc_a | ElementTypes.PermanentArc_a); } }

        public static ScTypes ArcAccessConstantPositiveTemporary
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Constant_a | ElementTypes.PositiveArc_a | ElementTypes.TemporaryArc_a); } }

        public static ScTypes ArcAccessConstantNegativeTemporary
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Constant_a | ElementTypes.NegativeArc_a | ElementTypes.TemporaryArc_a); } }

        public static ScTypes ArcAccessConstantFuzzyTemporary
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Constant_a | ElementTypes.FuzzyArc_a | ElementTypes.TemporaryArc_a); } }


        public static ScTypes EdgeCommonVariable
        { get { return new ScTypes(ElementTypes.CommonEdge_a | ElementTypes.Variable_a); } }

        public static ScTypes ArcCommonVariable
        { get { return new ScTypes(ElementTypes.CommonArc_a | ElementTypes.Variable_a); } }

        public static ScTypes ArcAccessVariablePositivePermanent
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Variable_a | ElementTypes.PositiveArc_a | ElementTypes.PermanentArc_a); } }

        public static ScTypes ArcAccessVariableNegativePermanent
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Variable_a | ElementTypes.NegativeArc_a | ElementTypes.PermanentArc_a); } }

        public static ScTypes ArcAccessVariableFuzzyPermanent
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Variable_a | ElementTypes.FuzzyArc_a | ElementTypes.PermanentArc_a); } }

        public static ScTypes ArcAccessVariablePositiveTemporary
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Variable_a | ElementTypes.PositiveArc_a | ElementTypes.TemporaryArc_a); } }

        public static ScTypes ArcAccessVariableNegativeTemporary
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Variable_a | ElementTypes.NegativeArc_a | ElementTypes.TemporaryArc_a); } }

        public static ScTypes ArcAccessVariableFuzzyTemporary
        { get { return new ScTypes(ElementTypes.AccessArc_a | ElementTypes.Variable_a | ElementTypes.FuzzyArc_a | ElementTypes.TemporaryArc_a); } }


        public static ScTypes Node
        { get { return new ScTypes(ElementTypes.Node_a); } }

        public static ScTypes Link
        { get { return new ScTypes(ElementTypes.Link_a); } }

        public static ScTypes NodeConstant
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a); } }
        public static ScTypes NodeVariable
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a); } }

        public static ScTypes NodeConstantStructure
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.StructureNode_a); } }

        public static ScTypes NodeConstantTuple
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.TupleNode_a); } }

        public static ScTypes NodeConstantRole
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.RoleNode_a); } }

        public static ScTypes NodeConstantNonRole
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.NonRoleNode_a); } }

        public static ScTypes NodeConstantClass
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.ClassNode_a); } }

        public static ScTypes NodeConstantAbstract
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.AbstractNode_a); } }

        public static ScTypes NodeConstantMaterial
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Constant_a | ElementTypes.MaterialNode_a); } }


        public static ScTypes NodeVariableStructure
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.StructureNode_a); } }

        public static ScTypes NodeVariableTuple
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.TupleNode_a); } }

        public static ScTypes NodeVariableRole
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.RoleNode_a); } }

        public static ScTypes NodeVariableNonRole
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.NonRoleNode_a); } }

        public static ScTypes NodeVariableClass
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.ClassNode_a); } }

        public static ScTypes NodeVariableAbstract
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.AbstractNode_a); } }

        public static ScTypes NodeVariableMaterial
        { get { return new ScTypes(ElementTypes.Node_a | ElementTypes.Variable_a | ElementTypes.MaterialNode_a); } }


        #endregion

        #region Properties

        public bool IsArc
        {
            get { return (this.elementType & ElementTypes.ArcMask_c) != 0; }
        }

        public bool IsLink
        {
            get { return (this.elementType & ElementTypes.Link_a) != 0; }
        }

        public bool IsNode
        {
            get { return (this.elementType & ElementTypes.Node_a) != 0; }
        }

        public bool IsConstant
        {
            get { return (this.elementType & ElementTypes.Constant_a) != 0; }
        }

        public bool IsVariable
        {
            get { return (this.elementType & ElementTypes.Variable_a) != 0; }
        }

        #endregion


        #region Equals
        public override bool Equals(object obj)
        {

            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return this.Equals(obj as ScTypes);
        }

        public bool Equals(ScTypes other)
        {
            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            if (this.ElementType.Equals(other.ElementType))
            { return true; }
            else
            { return false; }
        }

        public static bool operator ==(ScTypes scType1, ScTypes scType2)
        {

            if (ReferenceEquals(scType1, null) || ReferenceEquals(scType2, null))
            {
                return ReferenceEquals(scType1, scType2);
            }
            return scType1.Equals(scType2);


        }

        public static bool operator !=(ScTypes scType1, ScTypes scType2)
        {
            if (ReferenceEquals(scType1, null) || ReferenceEquals(scType2, null))
            {
                return !ReferenceEquals(scType1, scType2);
            }
            return !scType1.Equals(scType2);
        }
        #endregion

        public override string ToString()
        {
            return elementType.ToString();
        }
    }
}
