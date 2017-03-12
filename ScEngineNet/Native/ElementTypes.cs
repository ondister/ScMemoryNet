using System;

namespace ScEngineNet.Native
{
    /// <summary>
    ///     Тип элемента.
    ///     Суффикс a указывает на атомарность типа элемента, суффикс c указывает, что это тип составлен из нескольких
    /// </summary>
    /// <remarks>
    ///     Для экономии памяти разработчик библиотеки scmemory продублировал значения перечислений для узлов и дуг.
    ///     Предполагается, что вы не будете ожидать от узла типа элемента "временная дуга".
    ///     Тем не менее не стоит забывать  о дублировании.
    ///     TupleNode_a = 0x80	        PositiveArc_a = 0x80
    ///     StructureNode_a = 0x100	    NegativeArc_a = 0x100
    ///     RoleNode_a = 0x200	        FuzzyArc_a = 0x200
    ///     NonRoleNode_a = 0x400	    TemporaryArc_a = 0x400
    ///     ClassNode_a = 0x800	        PermanentArc_a = 0x800
    /// </remarks>
    [Flags]
    internal enum ElementTypes : ushort
    {
        /// <summary>
        ///     Неизвестный или не указан.
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        ///     SC-ссылка общего вида.
        /// </summary>
        LinkA = 0x2,

        /// <summary>
        ///     Константный тип SC-элемента.
        /// </summary>
        ConstantA = 0x20,

        /// <summary>
        ///     Переменный тип SC-элемента.
        /// </summary>
        VariableA = 0x40,

        #region Nodes

        /// <summary>
        ///     SC-узел общего вида.
        /// </summary>
        NodeA = 0x1,

        /// <summary>
        ///     SC-узел, обозначающий небинарную связку.
        /// </summary>
        TupleNodeA = 0x80,

        /// <summary>
        ///     SC-узел, обозначающий структуру.
        /// </summary>
        StructureNodeA = 0x100,

        /// <summary>
        ///     SC-узел, обозначающий ролевое отношение.
        /// </summary>
        RoleNodeA = 0x200,

        /// <summary>
        ///     SC-узел, обозначающий неролевое отношение.
        /// </summary>
        NonRoleNodeA = 0x400,

        /// <summary>
        ///     SC-узел, не являющейся отношением.
        /// </summary>
        ClassNodeA = 0x800,

        /// <summary>
        ///     SC-узел, обозначающий абстрактный объект, не являющийся множеством.
        /// </summary>
        AbstractNodeA = 0x1000,

        /// <summary>
        ///     SC-узел, обозначающий материальный объект.
        /// </summary>
        MaterialNodeA = 0x2000,

        /// <summary>
        ///     Константный SC-узел, обозначающий неролевое отношение.
        /// </summary>
        NonRoleConstantNodeC = (NonRoleNodeA | ConstantA | NodeA),

        /// <summary>
        ///     The class constant node_c
        /// </summary>
        ClassConstantNodeC = (ClassNodeA | ConstantA | NodeA),

        /// <summary>
        ///     Константный SC-узел.
        /// </summary>
        ConstantNodeC = NodeA | ConstantA,

        #endregion

        #region Arcs

        /// <summary>
        ///     SC-ребро общего вида.
        /// </summary>
        CommonEdgeA = 0x4,

        /// <summary>
        ///     SC-дуга общего вида.
        /// </summary>
        CommonArcA = 0x8,

        /// <summary>
        ///     SC-дуга принадлежности.
        /// </summary>
        AccessArcA = 0x10,

        /// <summary>
        ///     Позитивная SC-дуга.
        /// </summary>
        PositiveArcA = 0x80,

        /// <summary>
        ///     Негативная SC-дуга.
        /// </summary>
        NegativeArcA = 0x100,

        /// <summary>
        ///     Нечеткая SC-дуга.
        /// </summary>
        FuzzyArcA = 0x200,

        /// <summary>
        ///     Нестационарная SC-дуга.
        /// </summary>
        TemporaryArcA = 0x400,

        /// <summary>
        ///     Стационарная SC-дуга.
        /// </summary>
        PermanentArcA = 0x800,

        /// <summary>
        ///     Позитивная константная стационарная SC-дуга принадлежности.
        /// </summary>
        PositiveConstantPermanentAccessArcC = (AccessArcA | ConstantA | PositiveArcA | PermanentArcA),

        /// <summary>
        ///     Позитивная константная стационарная SC-дуга общего вида.
        /// </summary>
        ConstantCommonArcC = (CommonArcA | ConstantA),

        #endregion

        #region Masks

        /// <summary>
        ///     Маска, означающая все SC-элементы.
        /// </summary>
        AnyElementMaskC = (NodeA | LinkA | CommonEdgeA | CommonArcA | AccessArcA),

        /// <summary>
        ///     Маска константности/переменности.
        /// </summary>
        ConstantOrVariableMaskC = (ConstantA | VariableA),

        /// <summary>
        ///     Маска позитивности/негативности/нечеткости.
        /// </summary>
        PositivityMaskC = (PositiveArcA | NegativeArcA | FuzzyArcA),

        /// <summary>
        ///     Маска стационарности/нестационарности,
        /// </summary>
        PermanencyMaskC = (PermanentArcA | TemporaryArcA),

        /// <summary>
        ///     Маска типов узлов.
        /// </summary>
        NodeOrStructureMaskC =
            (TupleNodeA | StructureNodeA | RoleNodeA | NonRoleNodeA | ClassNodeA | AbstractNodeA | MaterialNodeA),

        /// <summary>
        ///     Маска типов SC-коннекторов.
        /// </summary>
        ArcMaskC = (AccessArcA | CommonArcA | CommonEdgeA)

        #endregion
    }
}