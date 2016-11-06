using System;

namespace ScEngineNet.Native
{
    /// <summary>
    /// Тип элемента.
    /// Суффикс a указывает на атомарность типа элемента, суффикс c указывает, что это тип составлен из нескольких
    /// </summary>
    ///<remarks>
    ///Для экономии памяти разработчик библиотеки scmemory продублировал значения перечислений для узлов и дуг.
    ///Предполагается, что вы не будете ожидать от узла типа элемента "временная дуга".
    ///Тем не менее не стоит забывать  о дублировании.
    /// TupleNode_a = 0x80	        PositiveArc_a = 0x80
    /// StructureNode_a = 0x100	    NegativeArc_a = 0x100
    /// RoleNode_a = 0x200	        FuzzyArc_a = 0x200
    /// NonRoleNode_a = 0x400	    TemporaryArc_a = 0x400
    /// ClassNode_a = 0x800	        PermanentArc_a = 0x800
    ///</remarks>


    [Flags]
    internal enum ElementTypes : ushort
    {
        /// <summary>
        /// Неизвестный или не указан.
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// SC-ссылка общего вида.
        /// </summary>
        Link_a = 0x2,

        /// <summary>
        /// Константный тип SC-элемента.
        /// </summary>
        Constant_a = 0x20,

        /// <summary>
        /// Переменный тип SC-элемента.
        /// </summary>
        Variable_a = 0x40,

        #region Nodes
        /// <summary>
        /// SC-узел общего вида.
        /// </summary>
        Node_a = 0x1,

        /// <summary>
        /// SC-узел, обозначающий небинарную связку.
        /// </summary>
        TupleNode_a = 0x80,

        /// <summary>
        /// SC-узел, обозначающий структуру.
        /// </summary>
        StructureNode_a = 0x100,

        /// <summary>
        /// SC-узел, обозначающий ролевое отношение.
        /// </summary>
        RoleNode_a = 0x200,

        /// <summary>
        /// SC-узел, обозначающий неролевое отношение.
        /// </summary>
        NonRoleNode_a = 0x400,

        /// <summary>
        /// SC-узел, не являющейся отношением.
        /// </summary>
        ClassNode_a = 0x800,

        /// <summary>
        /// SC-узел, обозначающий абстрактный объект, не являющийся множеством.
        /// </summary>
        AbstractNode_a = 0x1000,

        /// <summary>
        /// SC-узел, обозначающий материальный объект.
        /// </summary>
        MaterialNode_a = 0x2000,

        /// <summary>
        /// Константный SC-узел, обозначающий неролевое отношение.
        /// </summary>
        NonRoleConstantNode_c = (NonRoleNode_a | Constant_a | Node_a),

        /// <summary>
        /// The class constant node_c
        /// </summary>
        ClassConstantNode_c = (ClassNode_a | Constant_a | Node_a),

        /// <summary>
        /// Константный SC-узел.
        /// </summary>
        ConstantNode_c = Node_a | Constant_a,
        #endregion

        #region Arcs
        /// <summary>
        /// SC-ребро общего вида.
        /// </summary>
        CommonEdge_a = 0x4,

        /// <summary>
        /// SC-дуга общего вида.
        /// </summary>
        CommonArc_a = 0x8,

        /// <summary>
        /// SC-дуга принадлежности.
        /// </summary>
        AccessArc_a = 0x10,

        /// <summary>
        /// Позитивная SC-дуга.
        /// </summary>
        PositiveArc_a = 0x80,

        /// <summary>
        /// Негативная SC-дуга.
        /// </summary>
        NegativeArc_a = 0x100,

        /// <summary>
        /// Нечеткая SC-дуга.
        /// </summary>
        FuzzyArc_a = 0x200,

        /// <summary>
        /// Нестационарная SC-дуга.
        /// </summary>
        TemporaryArc_a = 0x400,

        /// <summary>
        /// Стационарная SC-дуга.
        /// </summary>
        PermanentArc_a = 0x800,

        /// <summary>
        /// Позитивная константная стационарная SC-дуга принадлежности.
        /// </summary>
        PositiveConstantPermanentAccessArc_c = (AccessArc_a | Constant_a | PositiveArc_a | PermanentArc_a),

        /// <summary>
        /// Позитивная константная стационарная SC-дуга общего вида.
        /// </summary>
        ConstantCommonArc_c = (CommonArc_a | Constant_a),
        #endregion

        #region Masks
        /// <summary>
        /// Маска, означающая все SC-элементы.
        /// </summary>
        AnyElementMask_c = (Node_a | Link_a | CommonEdge_a | CommonArc_a | AccessArc_a),

        /// <summary>
        /// Маска константности/переменности.
        /// </summary>
        ConstantOrVariableMask_c = (Constant_a | Variable_a),

        /// <summary>
        /// Маска позитивности/негативности/нечеткости.
        /// </summary>
        PositivityMask_c = (PositiveArc_a | NegativeArc_a | FuzzyArc_a),

        /// <summary>
        /// Маска стационарности/нестационарности,
        /// </summary>
        PermanencyMask_c = (PermanentArc_a | TemporaryArc_a),

        /// <summary>
        /// Маска типов узлов.
        /// </summary>
        NodeOrStructureMask_c = (TupleNode_a | StructureNode_a | RoleNode_a | NonRoleNode_a | ClassNode_a | AbstractNode_a | MaterialNode_a),

        /// <summary>
        /// Маска типов SC-коннекторов.
        /// </summary>
        ArcMask_c = (AccessArc_a | CommonArc_a | CommonEdge_a),
        #endregion
    }
}
