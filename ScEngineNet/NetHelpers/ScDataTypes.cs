using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScEngineNet.NetHelpers
{
    public sealed class ScDataTypes
    {
        private static volatile ScDataTypes instance;
        private static object syncRoot = new Object();

        private List<Identifier> keyLinkTypes;

        public List<Identifier> KeyLinkTypes
        {
            get { return keyLinkTypes; }
        }
        private ScDataTypes()
        {
            keyLinkTypes = new List<Identifier>(){
                this.NumericByte,
                this.NumericDouble,
                this.NumericInt,
                this.NumericLong,
                this.TypeBinary,
                this.TypeBool,
                this.TypeString};
        }

        #region datatypes


        public Identifier NumericInt
        { get { return "numeric_int"; } }

        public Identifier NumericDouble
        { get { return "numeric_double"; } }

        public Identifier NumericLong
        { get { return "numeric_long"; } }

        public Identifier NumericByte
        { get { return "numeric_byte"; } }

        public Identifier TypeBinary
        { get { return "type_binary"; } }

        public Identifier TypeBool
        { get { return "type_bool"; } }

        public Identifier TypeString
        { get { return "type_string"; } }

        public Identifier LanguageEn
        { get { return "lang_en"; } }

        public Identifier LanguageRu
        { get { return "lang_ru"; } }

        #endregion
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
        internal bool CreateKeyNodes()
        {

            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.NumericInt);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.NumericDouble);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.NumericLong);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.NumericByte);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.TypeBinary);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.TypeBool);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.TypeString);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.LanguageEn);
                this.CreateKeyNode(context, ElementType.ClassNode_a, this.LanguageRu);
            }
            return true;
        }
    }
}
