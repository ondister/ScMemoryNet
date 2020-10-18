using System;
using ScEngineNet.Native;

namespace ScEngineNet.ScElements
{
    public class ScTypes : IEquatable<ScTypes>
    {
        internal ScTypes(ElementTypes elementType)
        {
            ElementType = elementType;
        }

        internal ElementTypes ElementType { get; private set; }

     
        public override int GetHashCode()
        {
            return (int) ElementType;
        }

        public override string ToString()
        {
            return ElementType.ToString();
        }

        #region StaticProperties

        public static ScTypes UnKnown
        {
            get { return new ScTypes(ElementTypes.Unknown); }
        }

        public static ScTypes EdgeCommon
        {
            get { return new ScTypes(ElementTypes.CommonEdgeA); }
        }

        public static ScTypes ArcCommon
        {
            get { return new ScTypes(ElementTypes.CommonArcA); }
        }

        public static ScTypes EdgeCommonConstant
        {
            get { return new ScTypes(ElementTypes.CommonEdgeA | ElementTypes.ConstantA); }
        }

        public static ScTypes ArcCommonConstant
        {
            get { return new ScTypes(ElementTypes.CommonArcA | ElementTypes.ConstantA); }
        }

        public static ScTypes ArcAccess
        {
            get { return new ScTypes(ElementTypes.AccessArcA); }
        }

        public static ScTypes ArcAccessConstantPositivePermanent
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.ConstantA | ElementTypes.PositiveArcA |
                                ElementTypes.PermanentArcA);
            }
        }

        public static ScTypes ArcAccessConstantNegativePermanent
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.ConstantA | ElementTypes.NegativeArcA |
                                ElementTypes.PermanentArcA);
            }
        }

        public static ScTypes ArcAccessConstantFuzzyPermanent
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.ConstantA | ElementTypes.FuzzyArcA |
                                ElementTypes.PermanentArcA);
            }
        }

        public static ScTypes ArcAccessConstantPositiveTemporary
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.ConstantA | ElementTypes.PositiveArcA |
                                ElementTypes.TemporaryArcA);
            }
        }

        public static ScTypes ArcAccessConstantNegativeTemporary
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.ConstantA | ElementTypes.NegativeArcA |
                                ElementTypes.TemporaryArcA);
            }
        }

        public static ScTypes ArcAccessConstantFuzzyTemporary
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.ConstantA | ElementTypes.FuzzyArcA |
                                ElementTypes.TemporaryArcA);
            }
        }


        public static ScTypes EdgeCommonVariable
        {
            get { return new ScTypes(ElementTypes.CommonEdgeA | ElementTypes.VariableA); }
        }

        public static ScTypes ArcCommonVariable
        {
            get { return new ScTypes(ElementTypes.CommonArcA | ElementTypes.VariableA); }
        }

        public static ScTypes ArcAccessVariablePositivePermanent
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.VariableA | ElementTypes.PositiveArcA |
                                ElementTypes.PermanentArcA);
            }
        }

        public static ScTypes ArcAccessVariableNegativePermanent
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.VariableA | ElementTypes.NegativeArcA |
                                ElementTypes.PermanentArcA);
            }
        }

        public static ScTypes ArcAccessVariableFuzzyPermanent
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.VariableA | ElementTypes.FuzzyArcA |
                                ElementTypes.PermanentArcA);
            }
        }

        public static ScTypes ArcAccessVariablePositiveTemporary
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.VariableA | ElementTypes.PositiveArcA |
                                ElementTypes.TemporaryArcA);
            }
        }

        public static ScTypes ArcAccessVariableNegativeTemporary
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.VariableA | ElementTypes.NegativeArcA |
                                ElementTypes.TemporaryArcA);
            }
        }

        public static ScTypes ArcAccessVariableFuzzyTemporary
        {
            get
            {
                return
                    new ScTypes(ElementTypes.AccessArcA | ElementTypes.VariableA | ElementTypes.FuzzyArcA |
                                ElementTypes.TemporaryArcA);
            }
        }


        public static ScTypes Node
        {
            get { return new ScTypes(ElementTypes.NodeA); }
        }

        public static ScTypes Link
        {
            get { return new ScTypes(ElementTypes.LinkA); }
        }

        public static ScTypes NodeConstant
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA); }
        }

        public static ScTypes NodeVariable
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA); }
        }

        public static ScTypes NodeConstantStructure
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.StructureNodeA); }
        }

        public static ScTypes NodeConstantTuple
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.TupleNodeA); }
        }

        public static ScTypes NodeConstantRole
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.RoleNodeA); }
        }

        public static ScTypes NodeConstantNonRole
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.NonRoleNodeA); }
        }

        public static ScTypes NodeConstantClass
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.ClassNodeA); }
        }

        public static ScTypes NodeConstantAbstract
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.AbstractNodeA); }
        }

        public static ScTypes NodeConstantMaterial
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.ConstantA | ElementTypes.MaterialNodeA); }
        }


        public static ScTypes NodeVariableStructure
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.StructureNodeA); }
        }

        public static ScTypes NodeVariableTuple
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.TupleNodeA); }
        }

        public static ScTypes NodeVariableRole
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.RoleNodeA); }
        }

        public static ScTypes NodeVariableNonRole
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.NonRoleNodeA); }
        }

        public static ScTypes NodeVariableClass
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.ClassNodeA); }
        }

        public static ScTypes NodeVariableAbstract
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.AbstractNodeA); }
        }

        public static ScTypes NodeVariableMaterial
        {
            get { return new ScTypes(ElementTypes.NodeA | ElementTypes.VariableA | ElementTypes.MaterialNodeA); }
        }

        #endregion

        #region Properties

        public bool IsArc
        {
            get { return (ElementType & ElementTypes.ArcMaskC) != 0; }
        }

        public bool IsLink
        {
            get { return (ElementType & ElementTypes.LinkA) != 0; }
        }

        public bool IsNode
        {
            get { return (ElementType & ElementTypes.NodeA) != 0; }
        }

        public bool IsConstant
        {
            get { return (ElementType & ElementTypes.ConstantA) != 0; }
        }

        public bool IsVariable
        {
            get { return (ElementType & ElementTypes.VariableA) != 0; }
        }

     
        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Equals(obj as ScTypes);
        }

        public bool Equals(ScTypes other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (ElementType.Equals(other.ElementType))
            {
                return true;
            }
            return false;
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
    }
}