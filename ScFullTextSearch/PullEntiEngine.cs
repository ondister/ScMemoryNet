using EP.Text;
using ScEngineNet;
using ScEngineNet.LinkContent;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScFullTextSearch
{
    public class PullEntiEngine
    {

        public PullEntiEngine()
        {


        }

        public void AddLink(ScLink Link)
        {

            this.ProcessLink(Link);

            //Заменяем класс ссылки после токенизации
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                var classLinkForTokenize = context.FindNode(SearchKeyNodes.Instance.ClassLinkForTokenize);
                var classTokenizedLink = context.FindNode(SearchKeyNodes.Instance.ClassTokenizedLink);

                if (context.ArcIsExist(classLinkForTokenize, Link, ScTypes.ArcAccessConstantPositivePermanent) == true)
                {
                    var linkIterator = context.CreateIterator(classLinkForTokenize, ScTypes.ArcAccessConstantPositivePermanent, Link);
                    linkIterator.ElementAt(0)[1].DeleteFromMemory();//убираем дугу от класса ClassLinkForTokenize

                    //и добавляем дугу от класса   classTokenizedLink, то есть ссылка токенизирована
                    classTokenizedLink.AddOutputArc(Link, ScTypes.ArcAccessConstantPositivePermanent);
                }

                classLinkForTokenize.Dispose();
                classTokenizedLink.Dispose();

            }
        }

        private void ProcessLink(ScLink Link)
        {
            String content = ((ScString)Link.LinkContent).Value;

            var tokensList = Morphology.Process(content);
            ScElement element = Link;

            int tokensCount = tokensList.Count;

            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                for (int tokenIndex = 0; tokenIndex < tokensCount; tokenIndex++)
                { 
                        Console.WriteLine("Токен {0} обрабатывается. Его лемма {1}, его термин {2}", tokensList[tokenIndex].Source, tokensList[tokenIndex].Lemma, tokensList[tokenIndex].Term);
                        element = this.CreateTokenConstruction(context, element, tokensList[tokenIndex]);
                }
            }

        }

        private ScElement CreateTokenConstruction(ScMemoryContext context, ScElement element, MorphToken Token)
        {

            //создаем конструкцию токена. см. файл token_construction в папке docs

            var classSimpleToken = context.FindNode(SearchKeyNodes.Instance.ClassSimpleToken);
            var nrelToken = context.FindNode(SearchKeyNodes.Instance.NrelToken);
            var nrelTokenStartPosition = context.FindNode(SearchKeyNodes.Instance.NrelTokenStartPosition);
            var nrelTokenEndPosition = context.FindNode(SearchKeyNodes.Instance.NrelTokenEndPosition);

            //создаем узел токена
            ScNode tokenNode = this.CreateClassNodeInstance(context, classSimpleToken);

            //добавляем общую дугу принадлежности токена к элементу и неролевое отношение 
            element.AddOutputArc(tokenNode, ScTypes.ArcCommonConstant)
                   .AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, nrelToken);

            //добавляем основной идентификатор
            tokenNode.MainIdentifiers[ScDataTypes.Instance.LanguageRu] = new ScString(Token.Term);

            //добавляем начальную и конечную позицию токена
            ScLink linkStartPosition = context.CreateLink(new ScInt32(Token.BeginChar));
            tokenNode.AddOutputArc(linkStartPosition, ScTypes.ArcCommonConstant)
                     .AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, nrelTokenStartPosition);
            ScLink linkEndPosition = context.CreateLink(new ScInt32(Token.EndChar));
            tokenNode.AddOutputArc(linkEndPosition, ScTypes.ArcCommonConstant)
                     .AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, nrelTokenEndPosition);

            //привязываемся к существующей словарной форме или создаем новую
         
            ScNode wordConcept = null;
            wordConcept = this.FindWordByLemma(context, Token.Lemma);
            if ((object)wordConcept==null)//Если не удалось найти  словарное слово по лемме то создаем его
            {
                wordConcept = this.CreateWordConcept(context, Token);
                Console.WriteLine("Создаем новое словарное слово {0}.", Token.Lemma);
            }
               
            //прикрепляем словарное слово к токену
            if ((object)wordConcept != null)
            {
                tokenNode.AddOutputArc(wordConcept, ScTypes.ArcCommonConstant)
                         .AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, context.FindNode(SearchKeyNodes.Instance.NrelTokenWord));
                Console.WriteLine("Слово {0} уже есть.", Token.Lemma);
            }
            else
            {
                Console.WriteLine("Связь слова и токена не установлена. Так как не удалось найти слово по лемме {0}.",Token.Lemma);
            }


            classSimpleToken.Dispose();
            nrelToken.Dispose();
            nrelTokenEndPosition.Dispose();
            nrelTokenStartPosition.Dispose();

            return tokenNode;
        }

        public ScNode CreateWordConcept(ScMemoryContext context, MorphToken Token)
        {

            ScNode nodeWord = this.CreateClassNodeInstance(context, context.FindNode(SearchKeyNodes.Instance.ClassWord));

            //добавляем лемму
            ScNode nodeLemma = this.CreateClassNodeInstance(context, context.FindNode(SearchKeyNodes.Instance.ClassWordLemma));
            nodeLemma.MainIdentifiers[ScDataTypes.Instance.LanguageRu] = new ScString(Token.Lemma);
            //присоединяем ее к словарному слову
            nodeWord.AddOutputArc(nodeLemma, ScTypes.ArcCommonConstant)
                    .AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, context.FindNode(SearchKeyNodes.Instance.NrelWordLemma));

            return nodeWord;
        }

        public ScNode CreateClassNodeInstance(ScMemoryContext context, ScNode classNode)
        {
            ScNode node = context.CreateNode(ScTypes.NodeConstant);
            node.SystemIdentifier = context.CreateUniqueIdentifier(ScNode.InstancePreffix + classNode.SystemIdentifier, node);
            classNode.AddOutputArc(node, ScTypes.ArcAccessConstantPositivePermanent);
            return node;
        }

        /// <summary>
        /// Ищет узел-чловарное слово по ссылке на лемму
        /// </summary>
        /// <param name="lemmaLink">The lemma link.</param>
        /// <returns></returns>
        public ScNode FindWordByLemma(ScMemoryContext context, String lemmaString)
        {
            ScNode node = null;

            List<ScLink> lemmaLinks = context.FindLinks(lemmaString);

            foreach(var link in lemmaLinks)
            {
                ScNode main_idtf = context.FindNode(ScKeyNodes.Instance.NrelMainIdtf);
                ScIterator lemmaIterator = context.CreateIterator(ScTypes.NodeConstant, ScTypes.ArcCommon, link, ScTypes.ArcAccessConstantPositivePermanent, main_idtf);

                foreach (var construction in lemmaIterator)
                {
                    ScIterator wordIterator = context.CreateIterator(ScTypes.NodeConstant, ScTypes.ArcCommon, construction[0], ScTypes.ArcAccessConstantPositivePermanent, context.FindNode(SearchKeyNodes.Instance.NrelWordLemma));
                    if (wordIterator.Count() != 0)
                    {
                        node = (ScNode)wordIterator.ElementAt(0)[0];
                        break;
                    }
                }
                if ((object)node != null) { break; }
            }

            

            return node;
        }


    }
}
