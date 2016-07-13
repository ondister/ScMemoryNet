using ScEngineNet.SafeElements;
using System;

namespace ScEngineNet.NetHelpers
{
    /// <summary>
    /// Ключевые узлы, означающие натуральные языки
    /// </summary>
    public static class NLanguages
    {
        private static ScNode lang_ru;
        private static ScNode lang_en;


        /// <summary>
        /// Возвращает ключевой узел: английский язык
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Lang_en
        {
            get { return NLanguages.lang_en; }
        }


        /// <summary>
        /// Возвращает ключевой узел: русский язык
        /// </summary>
        /// <value>
        /// <see cref="ScNode"/>
        /// </value>
        public static ScNode Lang_ru
        {
            get { return NLanguages.lang_ru; }
        }


        internal static void CreateKeyNodes()
        {
            ScMemoryContext context = new ScMemoryContext(ScAccessLevels.MinLevel);
            lang_ru = NLanguages.CreateKeyNode(context, ElementType.ClassNode_a, "lang_ru");
            lang_en = NLanguages.CreateKeyNode(context, ElementType.ClassNode_a, "lang_en");
            context.Delete();
        }

        private static ScNode CreateKeyNode(ScMemoryContext context, ElementType elementType, Identifier identifier)
        {
            Console.WriteLine("Create NLanguage KeyNode: {0}", identifier);
            return context.CreateNode(elementType, identifier);
        }
    }
}
