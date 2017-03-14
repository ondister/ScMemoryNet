using System;
using System.Collections.Generic;
using ScEngineNet.ScElements;

namespace ScEngineNet.NetHelpers
{
    /// <summary>
    ///     Класс, содержащий идентификаторы типов данных для ссылок
    /// </summary>
    public sealed class ScDataTypes
    {
        private static volatile ScDataTypes instance;
        private static readonly object syncRoot = new Object();

        private ScDataTypes()
        {
            KeyLinkTypes = new List<Identifier>
            {
                NumericByte,
                NumericDouble,
                NumericInt,
                NumericLong,
                TypeBinary,
                TypeBool,
                TypeString,
                Date,
                DateTime,
                Time,
                Bitmap
            };
        }

        /// <summary>
        ///     Возвращает коллекцию идентификаторов типов ссылок
        /// </summary>
        /// <value>
        ///     Коллекция идентификаторов ссылок
        /// </value>
        internal List<Identifier> KeyLinkTypes { get; private set; }

        /// <summary>
        ///     Возвращает экземпляр класса ScDataTypes
        /// </summary>
        /// <value>
        ///     Экземпляр класса ScDataTypes
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

        private static void CreateKeyNode(ScMemoryContext context, ScTypes elementType, Identifier identifier)
        {
            Console.WriteLine("Create ScMemory.net KeyNode: {0}", identifier);
            context.CreateNode(elementType, identifier);
        }

        /// <summary>
        ///     Создает ключевые узлы
        /// </summary>
        /// <returns></returns>
        internal bool CreateKeyNodes()
        {
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                CreateKeyNode(context, ScTypes.NodeConstantClass, NumericInt);
                CreateKeyNode(context, ScTypes.NodeConstantClass, NumericDouble);
                CreateKeyNode(context, ScTypes.NodeConstantClass, NumericLong);
                CreateKeyNode(context, ScTypes.NodeConstantClass, NumericByte);
                CreateKeyNode(context, ScTypes.NodeConstantClass, TypeBinary);
                CreateKeyNode(context, ScTypes.NodeConstantClass, TypeBool);
                CreateKeyNode(context, ScTypes.NodeConstantClass, TypeString);
                CreateKeyNode(context, ScTypes.NodeConstantClass, LanguageEn);
                CreateKeyNode(context, ScTypes.NodeConstantClass, LanguageRu);
                CreateKeyNode(context, ScTypes.NodeConstantClass, Bitmap);
                CreateKeyNode(context, ScTypes.NodeConstantClass, Date);
                CreateKeyNode(context, ScTypes.NodeConstantClass, Time);
                CreateKeyNode(context, ScTypes.NodeConstantClass, DateTime);
            }
            return true;
        }

        #region datatypes

        /// <summary>
        ///     Возвращает идентификатор numeric_int
        /// </summary>
        /// <value>
        ///     Идентификатор numeric_int
        /// </value>
        public Identifier NumericInt
        {
            get { return "numeric_int"; }
        }

        /// <summary>
        ///     Возвращает идентификатор numeric_double
        /// </summary>
        /// <value>
        ///     Идентификатор numeric_double
        /// </value>
        public Identifier NumericDouble
        {
            get { return "numeric_double"; }
        }

        /// <summary>
        ///     Возвращает идентификатор numeric_long
        /// </summary>
        /// <value>
        ///     Идентификатор numeric_long
        /// </value>
        public Identifier NumericLong
        {
            get { return "numeric_long"; }
        }

        /// <summary>
        ///     Возвращает идентификатор numeric_byte
        /// </summary>
        /// <value>
        ///     Идентификатор numeric_byte
        /// </value>
        public Identifier NumericByte
        {
            get { return "numeric_byte"; }
        }

        /// <summary>
        ///     Возвращает идентификатор type_binary
        /// </summary>
        /// <value>
        ///     Идентификатор type_binary
        /// </value>
        public Identifier TypeBinary
        {
            get { return "type_binary"; }
        }

        /// <summary>
        ///     Возвращает идентификатор type_bool
        /// </summary>
        /// <value>
        ///     Идентификатор type_bool
        /// </value>
        public Identifier TypeBool
        {
            get { return "type_bool"; }
        }

        /// <summary>
        ///     Возвращает идентификатор type_string
        /// </summary>
        /// <value>
        ///     Идентификатор type_string
        /// </value>
        public Identifier TypeString
        {
            get { return "type_string"; }
        }

        /// <summary>
        ///     Возвращает идентификатор lang_en
        /// </summary>
        /// <value>
        ///     Идентификатор lang_en
        /// </value>
        public Identifier LanguageEn
        {
            get { return "lang_en"; }
        }

        /// <summary>
        ///     Возвращает идентификатор lang_ru
        /// </summary>
        /// <value>
        ///     Идентификатор lang_ru
        /// </value>
        public Identifier LanguageRu
        {
            get { return "lang_ru"; }
        }

        public Identifier Date
        {
            get { return "type_date"; }
        }

        public Identifier Time
        {
            get { return "type_time"; }
        }

        public Identifier DateTime
        {
            get { return "type_date_time"; }
        }

        public Identifier Bitmap
        {
            get { return "type_bitmap"; }
        }

        #endregion
    }
}