using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;

namespace ScEngineNet.NetHelpers
{
    /// <summary>
    /// Класс, содержащий идентификаторы типов данных для ссылок
    /// </summary>
    public sealed class ScKeyNodes
    {
        private static volatile ScKeyNodes instance;
        private static object syncRoot = new Object();

       

      

        private ScKeyNodes()
        {
           
        }

        #region keyNodes


        /// <summary>
        /// Gets the nrel main idtf.
        /// </summary>
        /// <value>
        /// The nrel main idtf.
        /// </value>
        public Identifier NrelMainIdtf
        { get { return "nrel_main_idtf"; } }

       

        #endregion

        /// <summary>
        /// Возвращает экземпляр класса ScDataTypes
        /// </summary>
        /// <value>
        /// Экземпляр класса ScDataTypes
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
                this.CreateKeyNode(context, ElementType.NonRoleConstantNode_c, this.NrelMainIdtf);
            }
            return true;
        }
    }
}
