using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;

namespace ScEngineNet.NetHelpers
{
    /// <summary>
    /// Класс, содержащий идентификаторы типов данных для ссылок
    /// </summary>
    public sealed class ScDataTypes
    {
        private static volatile ScDataTypes instance;
        private static object syncRoot = new Object();

        private List<Identifier> keyLinkTypes;

        /// <summary>
        /// Возвращает коллекцию идентификаторов типов ссылок
        /// </summary>
        /// <value>
        /// Коллекция идентификаторов ссылок
        /// </value>
        internal List<Identifier> KeyLinkTypes
        {
            get { return keyLinkTypes; }
        }

        private ScDataTypes()
        {
            keyLinkTypes = new List<Identifier>()
            {
                this.NumericByte,
                this.NumericDouble,
                this.NumericInt,
                this.NumericLong,
                this.TypeBinary,
                this.TypeBool,
                this.TypeString
            };
        }

        #region datatypes

        /// <summary>
        /// Возвращает идентификатор numeric_int
        /// </summary>
        /// <value>
        /// Идентификатор numeric_int
        /// </value>
        public Identifier NumericInt
        { get { return "numeric_int"; } }

        /// <summary>
        /// Возвращает идентификатор numeric_double
        /// </summary>
        /// <value>
        /// Идентификатор numeric_double
        /// </value>
        public Identifier NumericDouble
        { get { return "numeric_double"; } }

        /// <summary>
        /// Возвращает идентификатор numeric_long
        /// </summary>
        /// <value>
        /// Идентификатор numeric_long
        /// </value>
        public Identifier NumericLong
        { get { return "numeric_long"; } }

        /// <summary>
        /// Возвращает идентификатор numeric_byte
        /// </summary>
        /// <value>
        /// Идентификатор numeric_byte
        /// </value>
        public Identifier NumericByte
        { get { return "numeric_byte"; } }

        /// <summary>
        /// Возвращает идентификатор type_binary
        /// </summary>
        /// <value>
        /// Идентификатор type_binary
        /// </value>
        public Identifier TypeBinary
        { get { return "type_binary"; } }

        /// <summary>
        /// Возвращает идентификатор type_bool
        /// </summary>
        /// <value>
        /// Идентификатор type_bool
        /// </value>
        public Identifier TypeBool
        { get { return "type_bool"; } }

        /// <summary>
        /// Возвращает идентификатор type_string
        /// </summary>
        /// <value>
        /// Идентификатор type_string
        /// </value>
        public Identifier TypeString
        { get { return "type_string"; } }

        /// <summary>
        /// Возвращает идентификатор lang_en
        /// </summary>
        /// <value>
        /// Идентификатор lang_en
        /// </value>
        public Identifier LanguageEn
        { get { return "lang_en"; } }

        /// <summary>
        /// Возвращает идентификатор lang_ru
        /// </summary>
        /// <value>
        /// Идентификатор lang_ru
        /// </value>
        public Identifier LanguageRu
        { get { return "lang_ru"; } }

        #endregion

        /// <summary>
        /// Возвращает экземпляр класса ScDataTypes
        /// </summary>
        /// <value>
        /// Экземпляр класса ScDataTypes
        /// </value>
        public static ScDataTypes Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ScDataTypes();
                    }
                }

                return instance;
            }
        }

        private ScNode CreateKeyNode(ScMemoryContext context, ElementType elementType, Identifier identifier)
        {
            Console.WriteLine("Create ScMemory.net KeyNode: {0}", identifier);
            return context.CreateNode(elementType, identifier);
        }
        /// <summary>
        /// Создает ключевые узлы
        /// </summary>
        /// <returns></returns>
        internal bool CreateKeyNodes()
        {

            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.NumericInt);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.NumericDouble);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.NumericLong);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.NumericByte);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.TypeBinary);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.TypeBool);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.TypeString);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.LanguageEn);
                this.CreateKeyNode(context, ElementType.ClassConstantNode_c, this.LanguageRu);
            }
            return true;
        }
    }
}
