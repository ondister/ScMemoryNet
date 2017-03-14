using System;
using ScEngineNet.ScElements;

namespace ScEngineNet.NetHelpers
{
    /// <summary>
    ///     Класс, содержащий идентификаторы типов данных для ссылок
    /// </summary>
    public sealed class ScKeyNodes
    {
        private static volatile ScKeyNodes instance;
        private static readonly object syncRoot = new Object();

        private ScKeyNodes()
        {
        }

        #region keyNodes

        /// <summary>
        ///     Gets the nrel main idtf.
        /// </summary>
        /// <value>
        ///     The nrel main idtf.
        /// </value>
        public Identifier NrelMainIdtf
        {
            get { return "nrel_main_idtf"; }
        }

        #endregion

        /// <summary>
        ///     Возвращает экземпляр класса ScDataTypes
        /// </summary>
        /// <value>
        ///     Экземпляр класса ScDataTypes
        /// </value>
        public static ScKeyNodes Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ScKeyNodes();
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
                CreateKeyNode(context, ScTypes.NodeConstantNonRole, NrelMainIdtf);
            }
            return true;
        }
    }
}