using ScEngineNet;
using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;

namespace ScFullTextSearch
{
    /// <summary>
    /// Ключевые узлы набора агентов для полнотекстового поиска.
    /// </summary>
    public static class SearchKeyNodes
    {

        private static List<ScNode> keyNodes;

        private static ScNode classLinkForTokenize;
        private static ScNode classTokenizedLink;
        private static ScNode classSimpleToken;
        private static ScNode classWord;
        private static ScNode classWordLemma;
        private static ScNode classWordVariant;
        private static ScNode nrelTokenStartPosition;
        private static ScNode nrelTokenEndPosition;
        private static ScNode nrelWordLemma;
        private static ScNode nrelWordVariant;
        private static ScNode nrelToken;
        private static ScNode nrelTokenWord;

     
       

        internal static void CreateKeyNodes()
        {

           var context = new ScMemoryContext(ScAccessLevels.MinLevel);
            
                classLinkForTokenize = SearchKeyNodes.CreateKeyNode(context, ElementType.ClassNode_a, "class_link_for_tokenize");
                classTokenizedLink = SearchKeyNodes.CreateKeyNode(context, ElementType.ClassNode_a, "class_tokenized_link");
                classSimpleToken = SearchKeyNodes.CreateKeyNode(context, ElementType.ClassNode_a, "class_simple_token");
                classWord = SearchKeyNodes.CreateKeyNode(context, ElementType.ClassNode_a, "class_word");
                classWordLemma = SearchKeyNodes.CreateKeyNode(context, ElementType.ClassNode_a, "class_word_lemma");
                classWordVariant = SearchKeyNodes.CreateKeyNode(context, ElementType.ClassNode_a, "class_word_variant");
                nrelTokenStartPosition = SearchKeyNodes.CreateKeyNode(context, ElementType.NonRoleNode_a, "nrel_token_start_position");
                nrelTokenEndPosition = SearchKeyNodes.CreateKeyNode(context, ElementType.NonRoleNode_a, "nrel_token_end_position");
                nrelWordLemma = SearchKeyNodes.CreateKeyNode(context, ElementType.NonRoleNode_a, "nrel_word_lemma");
                nrelWordVariant = SearchKeyNodes.CreateKeyNode(context, ElementType.NonRoleNode_a, "nrel_word_variant");
                nrelToken = SearchKeyNodes.CreateKeyNode(context, ElementType.NonRoleNode_a, "nrel_token");
                nrelTokenWord = SearchKeyNodes.CreateKeyNode(context, ElementType.NonRoleNode_a, "nrel_token_word");

                keyNodes = new List<ScNode>() { 
                    classLinkForTokenize, 
                    classTokenizedLink,
                    classSimpleToken,
                    classWord,
                    classWordLemma,
                    classWordVariant,
                    nrelTokenStartPosition,
                    nrelTokenEndPosition,
                    nrelWordLemma,
                    nrelWordVariant,
                    nrelToken,
                    nrelTokenWord
               
            };

        }

        public static ScNode ClassWordVariant
        {
            get { return SearchKeyNodes.classWordVariant; }
            set { SearchKeyNodes.classWordVariant = value; }
        }

        public static ScNode ClassWordLemma
        {
            get { return SearchKeyNodes.classWordLemma; }
            set { SearchKeyNodes.classWordLemma = value; }
        }
        public static ScNode NrelTokenWord
        {
            get { return SearchKeyNodes.nrelTokenWord; }
            set { SearchKeyNodes.nrelTokenWord = value; }
        }
        public static ScNode ClassWord
        {
            get { return SearchKeyNodes.classWord; }
            set { SearchKeyNodes.classWord = value; }
        }
        public static ScNode NrelToken
        {
            get { return SearchKeyNodes.nrelToken; }
            set { SearchKeyNodes.nrelToken = value; }
        }

        public static ScNode NrelWordVariant
        {
            get { return SearchKeyNodes.nrelWordVariant; }
            set { SearchKeyNodes.nrelWordVariant = value; }
        }

        public static ScNode NrelWordLemma
        {
            get { return SearchKeyNodes.nrelWordLemma; }
            set { SearchKeyNodes.nrelWordLemma = value; }
        }
        public static ScNode NrelTokenEndPosition
        {
            get { return SearchKeyNodes.nrelTokenEndPosition; }
            set { SearchKeyNodes.nrelTokenEndPosition = value; }
        }
        public static ScNode NrelTokenStartPosition
        {
            get { return SearchKeyNodes.nrelTokenStartPosition; }
            set { SearchKeyNodes.nrelTokenStartPosition = value; }
        }

        public static ScNode ClassSimpleToken
        {
            get { return SearchKeyNodes.classSimpleToken; }
            set { SearchKeyNodes.classSimpleToken = value; }
        }


        public static ScNode ClassTokenizedLink
        {
            get { return SearchKeyNodes.classTokenizedLink; }
            set { SearchKeyNodes.classTokenizedLink = value; }
        }

        public static ScNode ClassLinkForTokenize
        {
            get { return SearchKeyNodes.classLinkForTokenize; }
            set { SearchKeyNodes.classLinkForTokenize = value; }
        }
     
     

    

        /// <summary>
        /// Возвращает коллекцию ключевых узлов
        /// </summary>
        /// <value>
        /// Коллекция ключевых узлов
        /// </value>
        public static List<ScNode> KeyNodes
        {
            get { return SearchKeyNodes.keyNodes; }
        }


       



        private static ScNode CreateKeyNode(ScMemoryContext context, ElementType elementType, Identifier identifier)
        {
            Console.WriteLine("Create SearchKeyNode KeyNode: {0}", identifier);
            return context.CreateNode(elementType, identifier);
        }
    }
}
