using System;
using System.Collections.Generic;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Основных идентификаторов узла. 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class MainIdentifiers : IDisposable
    {
        private ScNode node;
        private ScMemoryContext scContext;

        internal MainIdentifiers(ScNode node)
        {
            this.node = node;
            this.scContext = new ScMemoryContext(node.scContext);
        }

        /// <summary>
        /// Задает или получает основной идентификатор с указанным узлом класса данных.
        /// Это ключевые узлы из коллекций <see cref="DataTypes"/> или <see cref="NLanguages"/>
        /// </summary>
        /// <value>
        /// Содержимое ссылки идентификатора <see cref="ScLinkContent"/>. Его необходимо привести к нужному типу.
        /// </value>
        /// <param name="ClassNode">Это ключевые узлы из коллекций <see cref="DataTypes"/> или <see cref="NLanguages"/></param>
        public ScLinkContent this[ScNode ClassNode]
        {
            set { this.setIdentifier(ClassNode, value); }
            get { return this.getIdentifier(ClassNode); }
        }

        private List<ScLink> getLinks()
        {
            List<ScLink> links = new List<ScLink>();
            ScNode main_idtf = scContext.FindNode("nrel_main_idtf");
            ScIterator container = scContext.CreateIterator(node, ElementType.ConstantCommonArc_c, ElementType.Link_a, ElementType.PositiveConstantPermanentAccessArc_c, main_idtf);
            foreach (var construction in container)
            {
                links.Add((construction.Elements[2] as ScLink));
            }

            return links;
        }

        private ScLinkContent getIdentifier(ScNode ClassNode)
        {
            ScLinkContent identifier = "";
            List<ScLink> links = this.getLinks();

            foreach (var link in links)
            {
                ScIterator container = scContext.CreateIterator(ClassNode, ElementType.PositiveConstantPermanentAccessArc_c, link);

                var constructions = container.GetAllConstructions();
                if (constructions.Count != 0)
                {
                    ScLinkContent content = (constructions[0].Elements[2] as ScLink).LinkContent;
                    identifier = content;
                }

            }

            return identifier;
        }

        private void setIdentifier(ScNode ClassNode, ScLinkContent identifier)
        {
            if (this.getIdentifier(ClassNode) == "")
            {
                ScNode main_idtf = scContext.FindNode("nrel_main_idtf");
                ScLink idtfLink = scContext.CreateLink(identifier);
                var commonArc = scContext.CreateArc(node, idtfLink, ElementType.ConstantCommonArc_c);
                //another arcs
                var arc1 = scContext.CreateArc(main_idtf, commonArc, ElementType.PositiveConstantPermanentAccessArc_c);
                var arc2 = scContext.CreateArc(ClassNode, idtfLink, ElementType.PositiveConstantPermanentAccessArc_c);

            }
        }



        #region Члены IDisposable

        private bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                //unmanaged
                this.scContext.Dispose();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MainIdentifiers"/> class.
        /// </summary>
        ~MainIdentifiers()
        {
            Dispose(false);
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
