using System;
using System.Collections.Generic;
using System.Linq;
using ScEngineNet.LinkContent;

namespace ScEngineNet.ScElements
{
    /// <summary>
    ///     Основные идентификаторы узла.
    /// </summary>
    public class MainIdentifiers
    {
        private readonly ScNode node;
        private readonly ScMemoryContext scContext;

        internal MainIdentifiers(ScNode node)
        {
            this.node = node;
            scContext = node.ScContext;
        }

        /// <summary>
        ///     Задает или получает основной идентификатор с указанным узлом класса данных.
        ///     Это ключевые узлы из коллекций <see cref="NetHelpers.ScDataTypes" />
        /// </summary>
        /// <value>
        ///     Содержимое ссылки идентификатора <see cref="ScLinkContent" />. Его необходимо привести к нужному типу.
        /// </value>
        /// <param name="classNodeIdentifier">Это ключевые узлы из коллекций <see cref="NetHelpers.ScDataTypes" /> </param>
        public ScLinkContent this[Identifier classNodeIdentifier]
        {
            set { SetIdentifier(classNodeIdentifier, value); }
            get { return GetIdentifier(classNodeIdentifier); }
        }

        private IEnumerable<ScLink> GetLinks()
        {
            var mainIdtf = scContext.FindNode("nrel_main_idtf");
            var container = scContext.CreateIterator(node, ScTypes.ArcCommonConstant, ScTypes.Link,
                ScTypes.ArcAccessConstantPositivePermanent, mainIdtf);

            return container.Select(construction => (construction[2] as ScLink)).ToList();
        }

        private ScLinkContent GetIdentifier(Identifier classNodeIdentifier)
        {
            ScLinkContent identifier = "";
            var links = GetLinks();

            foreach (var link in links)
            {
                var container = scContext.CreateIterator(scContext.FindNode(classNodeIdentifier),
                    ScTypes.ArcAccessConstantPositivePermanent, link);
                if (container.Any())
                {
                    var scLink = container.ElementAt(0)[2] as ScLink;
                    if (scLink != null)
                    {
                        var content = scLink.LinkContent;
                        identifier = content;
                    }
                }
            }

            return identifier;
        }

        private void SetIdentifier(Identifier classNodeIdentifier, ScLinkContent identifier)
        {
            var mainIdtf = scContext.FindNode("nrel_main_idtf");

            if (GetIdentifier(classNodeIdentifier) != "")
            {
                //delete identifier
                var links = GetLinks();

                foreach (var link in links)
                {
                    var container = scContext.CreateIterator(scContext.FindNode(classNodeIdentifier),
                        ScTypes.ArcAccessConstantPositivePermanent, link);
                    if (container.Any())
                    {
                        var baseIterator = scContext.CreateIterator(node, ScTypes.ArcCommonConstant, link,
                            ScTypes.ArcAccessConstantPositivePermanent, mainIdtf);
                        foreach (var construction in baseIterator)
                        {
                            construction[1].DeleteFromMemory();
                            construction[3].DeleteFromMemory();
                            Console.WriteLine("Idtf Deleted");
                        }
                    }
                }
            }

            //add identifier
                var idtfLink = scContext.CreateLink(identifier);
                var commonArc = scContext.CreateArc(node, idtfLink, ScTypes.ArcCommonConstant);
                //another arcs
                scContext.CreateArc(mainIdtf, commonArc, ScTypes.ArcAccessConstantPositivePermanent);
                scContext.CreateArc(scContext.FindNode(classNodeIdentifier), idtfLink,
                    ScTypes.ArcAccessConstantPositivePermanent);
        }
    }
}