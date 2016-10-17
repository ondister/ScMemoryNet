using System;
using System.Collections.Generic;
using System.Linq;

namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Основных идентификаторов узла. 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class MainIdentifiers
    {
        private ScNode node;
        private ScMemoryContext scContext;

        internal MainIdentifiers(ScNode node)
        {
            this.node = node;
            this.scContext = node.scContext;
        }

        /// <summary>
        /// Задает или получает основной идентификатор с указанным узлом класса данных.
        /// Это ключевые узлы из коллекций <see cref="DataTypes"/> или <see cref="NLanguages"/>
        /// </summary>
        /// <value>
        /// Содержимое ссылки идентификатора <see cref="ScLinkContent"/>. Его необходимо привести к нужному типу.
        /// </value>
        /// <param name="ClassNodeIdentifier">Это ключевые узлы из коллекций <see cref="DataTypes"/> или <see cref="NLanguages"/></param>
        public ScLinkContent this[Identifier ClassNodeIdentifier]
        {
            set { this.setIdentifier(ClassNodeIdentifier, value); }
            get { return this.getIdentifier(ClassNodeIdentifier); }
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

        private ScLinkContent getIdentifier(Identifier ClassNodeIdentifier)
        {
            ScLinkContent identifier = "";
            List<ScLink> links = this.getLinks();

            foreach (var link in links)
            {
                var container = scContext.CreateIterator(scContext.FindNode(ClassNodeIdentifier), ElementType.PositiveConstantPermanentAccessArc_c, link);
                if ( container.Count() != 0)
                {    
                    ScLinkContent content = (container.ElementAt(0).Elements[2] as ScLink).LinkContent;
                    identifier = content;
                }

            }

            return identifier;
        }

        private void setIdentifier(Identifier ClassNodeIdentifier, ScLinkContent identifier)
        {
            if (this.getIdentifier(ClassNodeIdentifier) == "")
            {
                ScNode main_idtf = scContext.FindNode("nrel_main_idtf");
                ScLink idtfLink = scContext.CreateLink(identifier);
                var commonArc = scContext.CreateArc(node, idtfLink, ElementType.ConstantCommonArc_c);
                //another arcs
                var arc1 = scContext.CreateArc(main_idtf, commonArc, ElementType.PositiveConstantPermanentAccessArc_c);
                var arc2 = scContext.CreateArc(scContext.FindNode(ClassNodeIdentifier), idtfLink, ElementType.PositiveConstantPermanentAccessArc_c);
            }
        }
    }
}
