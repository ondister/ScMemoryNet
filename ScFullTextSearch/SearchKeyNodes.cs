using System;

using ScEngineNet.ScElements;

namespace ScFullTextSearch
{
    /// <summary>
    /// Класс, содержащий идентификаторы типов данных для ссылок
    /// </summary>
    public sealed class SearchKeyNodes
    {
        private static volatile SearchKeyNodes instance;
        private static object syncRoot = new Object();

        private SearchKeyNodes()
        {
        }

        #region keynodes

        public Identifier ClassLinkForTokenize
        { get { return "class_link_for_tokenize"; } }

        public Identifier ClassTokenizedLink
        { get { return "class_tokenized_link"; } }

        public Identifier ClassSimpleToken
        { get { return "class_simple_token"; } }

        public Identifier ClassWord
        { get { return "class_word"; } }

        public Identifier ClassWordLemma
        { get { return "class_word_lemma"; } }

        public Identifier ClassWordVariant
        { get { return "class_word_variant"; } }

        public Identifier NrelTokenStartPosition
        { get { return "nrel_token_start_position"; } }

        public Identifier NrelTokenEndPosition
        { get { return "nrel_token_end_position"; } }

        public Identifier NrelWordLemma
        { get { return "nrel_word_lemma"; } }

        public Identifier NrelWordVariant
        { get { return "nrel_word_variant"; } }

        public Identifier NrelToken
        { get { return "nrel_token"; } }

        public Identifier NrelTokenWord
        { get { return "nrel_token_word"; } }

        public Identifier ClassQuerryString
        { get { return "class_querry_string"; } }

        public Identifier ClassQuerryResponse
        { get { return "class_querry_response"; } }

        public Identifier NrelQuerryResponse
        { get { return "nrel_querry_response"; } }

        #endregion

        /// <summary>
        /// Возвращает экземпляр класса ScDataTypes
        /// </summary>
        /// <value>
        /// Экземпляр класса ScDataTypes
        /// </value>
        public static SearchKeyNodes Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new SearchKeyNodes());
                }
            }
        }

        private ScNode CreateKeyNode(ScMemoryContext context, ScTypes  elementType, Identifier identifier)
        {
            Console.WriteLine("Create ScFulltextSearch KeyNode: {0}", identifier);
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
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassLinkForTokenize);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassTokenizedLink);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassSimpleToken);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassWord);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassWordLemma);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassWordVariant);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassQuerryString);
                this.CreateKeyNode(context, ScTypes.NodeConstantClass, this.ClassQuerryResponse);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelToken);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelTokenEndPosition);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelTokenStartPosition);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelTokenWord);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelWordLemma);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelWordVariant);
                this.CreateKeyNode(context, ScTypes.NodeConstantNonRole, this.NrelQuerryResponse);
            }
            return true;
        }
    }
}
