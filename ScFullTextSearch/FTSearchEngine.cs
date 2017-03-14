using EP.Text;
using ScEngineNet;
using ScEngineNet.LinkContent;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScFullTextSearch
{
  public  class FTSearchEngine
    {
      private PullEntiEngine engine;
      private ScNode querryNode;
      public FTSearchEngine()
      {
          engine = new PullEntiEngine();
      }
      public void AddQuerry(ScNode querryNode)
      {
          this.querryNode = querryNode;
          this.ProcessContent(((ScString)this.querryNode.MainIdentifiers[ScDataTypes.Instance.LanguageRu]).Value);
         
      }

      private void ProcessContent(String content)
      {

          var tokensList = Morphology.Process(content);

          int tokensCount = tokensList.Count;

          using (var context = new ScMemoryContext(ScAccessLevels.MinLevel))
          {
              //создаем узел ответа
              var responseNode = engine.CreateClassNodeInstance(context, context.FindNode(SearchKeyNodes.Instance.ClassQuerryResponse));

              for (int tokenIndex = 0; tokenIndex < tokensCount; tokenIndex++)
              {         
                      //ищем словарное слово по лемме
                      var wordNode = engine.FindWordByLemma(context, tokensList[tokenIndex].Lemma);
                      //добавляем к узлу ответа найденное слово
                      if ((object)wordNode != null)
                      {
                          responseNode.AddOutputArc(wordNode, ScTypes.ArcAccessConstantPositivePermanent);
                          Console.WriteLine("Слово {0} добавлено в ответ", tokensList[tokenIndex].Lemma);
                      }
                      else
                      {
                          Console.WriteLine("Слово по лемме не найдено для ответа {0}. Создадим его", tokensList[tokenIndex].Lemma);
                          engine.CreateWordConcept(context, tokensList[tokenIndex]);
                      }
              }
              //добавляем ответ к запросу
              if ((object)querryNode != null)
              {
                  querryNode.AddOutputArc(responseNode, ScTypes.ArcCommonConstant)
                            .AddInputArc(ScTypes.ArcAccessConstantPositivePermanent, context.FindNode(SearchKeyNodes.Instance.NrelQuerryResponse));
              }

          }

      }

     

     

    }
}
