using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;

namespace ScEngineNet.NetHelpers
{
    /// <summary>
    /// Ключевые узлы типов данных SC ссылки.
    /// Обертка для .net всегда указывает тип данных ссылки при добавлении контента.
    /// </summary>
    public static class DataTypes
    {
        private static ScNode numeric_int;
        private static ScNode numeric_double;
        private static ScNode numeric_long;
        private static ScNode numeric_byte;
        private static ScNode type_binary;
        private static ScNode type_bool;
        private static ScNode type_string;

        private static List<ScNode> keyNodes;

        /// <summary>
        /// Возвращает коллекцию ключевых узлов
        /// </summary>
        /// <value>
        /// Коллекция ключевых узлов
        /// </value>
        public static List<ScNode> KeyNodes
        {
            get { return DataTypes.keyNodes; }
        }


        /// <summary>
        /// Возвращает ключевой узел: класс строка
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Type_string
        {
            get { return DataTypes.type_string; }
            set { DataTypes.type_string = value; }
        }


        /// <summary>
        /// Возвращает ключевой узел: логическое значение
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Type_bool
        {
            get { return DataTypes.type_bool; }
            set { DataTypes.type_bool = value; }
        }


        /// <summary>
        /// Возвращает ключевой узел: класс byte
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Numeric_byte
        {
            get { return DataTypes.numeric_byte; }
            set { DataTypes.numeric_byte = value; }
        }


        /// <summary>
        /// Возвращает ключевой узел: класс binary (массив байт)
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Binary
        {
            get { return DataTypes.type_binary; }
        }


        /// <summary>
        /// Возвращает ключевой узел: класс long
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Numeric_long
        {
            get { return DataTypes.numeric_long; }
        }


        /// <summary>
        /// Возвращает ключевой узел: класс double
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Numeric_double
        {
            get { return DataTypes.numeric_double; }
        }


        /// <summary>
        /// Возвращает ключевой узел: класс int
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Numeric_int
        {
            get { return DataTypes.numeric_int; }
        }



        internal static void CreateKeyNodes()
        {

            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                numeric_int = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "numeric_int");
                numeric_double = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "numeric_double");
                numeric_long = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "numeric_long");
                numeric_byte = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "numeric_byte");
                type_binary = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "type_binary");
                type_bool = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "type_bool");
                type_string = DataTypes.CreateKeyNode(context, ElementType.ClassNode_a, "type_string");
                keyNodes = new List<ScNode>() { numeric_int, numeric_double, numeric_long, numeric_byte, type_binary, type_bool, type_string };
            }
          
        }



        private static ScNode CreateKeyNode(ScMemoryContext context, ElementType elementType, Identifier identifier)
        {
            Console.WriteLine("Create DataTypes KeyNode: {0}", identifier);
            return context.CreateNode(elementType, identifier);
        }
    }
}
