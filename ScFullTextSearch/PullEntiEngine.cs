using EP.Text;
using ScEngineNet;
using ScEngineNet.NetHelpers;
using ScEngineNet.SafeElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScFullTextSearch
{
    public class PullEntiEngine
    {
        private List<ScLink> linkPull;

        public PullEntiEngine()
        {
            linkPull = new List<ScLink>();

        }

        public void AddLink(ScLink Link)
        {
            linkPull.Add(Link);
            this.ProcessLink(Link);
            //Заменяем класс ссылки после токенизации
            linkPull.Remove(Link);
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                if (context.ArcIsExist(SearchKeyNodes.ClassLinkForTokenize, Link, ElementType.PositiveConstantPermanentAccessArc_c) == true)
                {
                    var linkIterator = context.CreateIterator(SearchKeyNodes.ClassLinkForTokenize, ElementType.PositiveConstantPermanentAccessArc_c, Link);
                    linkIterator.ElementAt(0)[1].DeleteFromMemory();//убераем дугу от классас ClassForTokenizeString
                    //и довляем дугу от класса  ClassTokenizedString, то есть ссылка токенизирована
                    SearchKeyNodes.ClassTokenizedLink.AddOutputArc(Link, ElementType.PositiveConstantPermanentAccessArc_c);
                }
            }
        }

        private void ProcessLink(ScLink Link)
        {
            String content = ((ScString)Link.LinkContent).Value;

            var tokensList = Morphology.Process(content);
            ScElement element = Link;
            int tokenIndex = 0;
            int tokensCount = tokensList.Count;
            foreach (var token in tokensList)
            {
                tokenIndex++;
                if (tokenIndex != tokensCount)//У последнего токена последователей нету
                {
                    Console.WriteLine("Токен {0} обрабатывается. Его лемма {1}, его термин {2}",token.Source,token.Lemma,token.Term);
                    element = this.CreateTokenConstruction(element, token);
                    
                }

            }

        }

        private ScElement CreateTokenConstruction(ScElement element, MorphToken Token)
        {
            ScNode tokenNode = null;
            //создаем конструкцию токена. см. файл token_construction в папке docs
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                //создаем узел токена
                tokenNode = this.CreateClassNodeInstance(SearchKeyNodes.ClassSimpleToken);

                //добавляем общую дугу принадлежности и неролевое отношение
                element.AddOutputArc(tokenNode, ElementType.ConstantCommonArc_c).AddInputArc(SearchKeyNodes.NrelToken, ElementType.PositiveConstantPermanentAccessArc_c);

                //добавляем основной идентификатор
                tokenNode.MainIdentifiers[ScDataTypes.Instance.TypeString] = new ScString(Token.Term);

                //добавляем начальную и конечную позицию токена
                ScLink linkStartPosition = context.CreateLink(new ScInt32(Token.BeginChar));
                tokenNode.AddOutputArc(linkStartPosition, ElementType.ConstantCommonArc_c).AddInputArc(SearchKeyNodes.NrelTokenStartPosition, ElementType.PositiveConstantPermanentAccessArc_c);
                ScLink linkEndPosition = context.CreateLink(new ScInt32(Token.EndChar));
                tokenNode.AddOutputArc(linkEndPosition, ElementType.ConstantCommonArc_c).AddInputArc(SearchKeyNodes.NrelTokenEndPosition, ElementType.PositiveConstantPermanentAccessArc_c);

                //привязываемся к существующей словарной форме или создаем новую
                this.CreateWordConcept(tokenNode, Token);
            }
            return tokenNode;
        }

        private void CreateWordConcept(ScElement SimpleToken, MorphToken Token)
        {

            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                ScNode nodeWord = null;
                List<ScLink> links = context.FindLinks(Token.Lemma);
                //пробуем найти лемму
                if (links.Count == 0)//если не находим создаем новое слово
                {
                    nodeWord = this.CreateClassNodeInstance(SearchKeyNodes.ClassWord);
                    
                    //добавляем лемму
                    ScNode nodeLemma = this.CreateClassNodeInstance(SearchKeyNodes.ClassWordLemma);
                   nodeLemma.MainIdentifiers[ScDataTypes.Instance.TypeString] = new ScString(Token.Lemma);
                   
                    nodeWord.AddOutputArc(nodeLemma, ElementType.ConstantCommonArc_c).AddInputArc(SearchKeyNodes.NrelWordLemma, ElementType.PositiveConstantPermanentAccessArc_c);
                }
                else
                {
                    if (links.Count > 1) { Console.WriteLine("В базе более 1 ссылки с леммой. Нужно посмотреть код добавления леммы."); }
#warning Здесь очень опасный кусок кода. Нужно подумать и исправить
                    nodeWord = this.FindWordByLemma(links[0]);
                }
                // привязываем токен к слову
                if ((object)nodeWord != null)
                {
                    SimpleToken.AddOutputArc(nodeWord, ElementType.ConstantCommonArc_c).AddInputArc(SearchKeyNodes.NrelTokenWord, ElementType.PositiveConstantPermanentAccessArc_c);
                }
                else
                {
                    Console.WriteLine("Связь слова и токена не установлена. Так как нне удалось найти слово по лемме.");
                }

            }
        }

        private ScNode CreateClassNodeInstance(ScNode Node)
        {
            ScNode node = null;
            //создаем конструкцию токена. см. файл token_construction в папке docs
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                node = context.CreateNode(ElementType.ConstantNode_c);
                node.SystemIdentifier = context.CreateUniqueIdentifier(ScNode.InstancePreffix + Node.SystemIdentifier, node);
                Node.AddOutputArc(node, ElementType.PositiveConstantPermanentAccessArc_c);
            }
            return node;
        }

        private ScNode FindWordByLemma(ScLink Lemma)
        {
            ScNode node = null;
            using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
            {
                ScNode main_idtf = context.FindNode("nrel_main_idtf");
                ScIterator lemmaIterator = context.CreateIterator(ElementType.ConstantNode_c, ElementType.ConstantCommonArc_c, Lemma, ElementType.PositiveConstantPermanentAccessArc_c, main_idtf);
                              
                foreach (var construction in lemmaIterator)
                {
                    Console.WriteLine(((ScString)Lemma.LinkContent).Value);
                    Console.WriteLine(((ScNode)construction[0]).SystemIdentifier);

                    ScIterator wordIterator = context.CreateIterator(ElementType.ConstantNode_c, ElementType.ConstantCommonArc_c, construction[0], ElementType.PositiveConstantPermanentAccessArc_c, SearchKeyNodes.NrelWordLemma);
                    if (wordIterator.Count() != 0)
                    {
                        node = (ScNode)wordIterator.ElementAt(0)[0];
                        break;
                    }
                }

            }
            return node;
        }


    }
}
