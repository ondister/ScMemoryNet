using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Sapfir.Models.ScNetExtension.ElasticIndexerModel;
using ScEngineNet.Events;
using ScEngineNet.NetHelpers;
using ScEngineNet.ScElements;

namespace Sapfir.Servers.SemanticServer.ElasticIndexer_netextension
{
    /// <summary>
    ///     Класс индексирует ссылки типов string, langRu, langEn
    /// </summary>
    internal class EnTextIndexer : LinkIndexer
    {
        private ScMemoryContext context;
        private ScNode linkClass;


        public override void Subscribe()
        {
            #region Подключаемся  к эластику

            var uriToElasticNode = new UriBuilder("http", ElasticSearchConfig.ServerAddress,
                ElasticSearchConfig.ServerPort).Uri;
            var elasticSettings = new ConnectionSettings(uriToElasticNode);
            elasticSettings.ThrowExceptions();

            ElasticClient = new ElasticClient(elasticSettings);

            #endregion

            context = new SapfirContext(ScAccessLevels.MinLevel);
            if (context != null)
            {
                linkClass = context.FindNode(ScDataTypes.Instance.LanguageEn);

                try
                {
                    if (linkClass != null)
                    {
                        linkClass.OutputArcAdded += ClassRu_OutputArcAdded;
                        linkClass.OutputArcRemoved += ClassRu_OutputArcRemoved;

                        ConsoleLog.Debug($"{linkClass.SystemIdentifier} подписан на добавление и удаление дуги");


                        if (ElasticClient.IndexExists(linkClass.SystemIdentifier.ToString()).Exists)
                        {
                            //удаляем индекс
                            ElasticClient.DeleteIndex(linkClass.SystemIdentifier.ToString());
                            ConsoleLog.Debug($"Удален индекс {linkClass.SystemIdentifier}");
                        }

                        //создаем индекс
                        ElasticClient.CreateIndex(linkClass.SystemIdentifier.ToString(),
                            c => c.Mappings(m => m.Map<IndexedLink>(
                                mm => mm.Properties(
                                        p => p.Text(t => t.Name(n => n.Content)
                                            .Analyzer("russian")
                                        ))
                                    .Properties(p => p.Number(t => t.Name(n => n.Offset)))
                                    .Properties(p => p.Number(t => t.Name(n => n.Segment))))));

                        ConsoleLog.Debug($"Создан индекс {linkClass.SystemIdentifier}");
                    }


                    //запускаем реиндексацию всех текстовых ссылок
                  ReindexLinks();
                }
                catch (ElasticsearchClientException elasticException)
                {
                    ConsoleLog.Fatal(elasticException,
                        $"Проблема с сервером ElasticSearch. Убедитесь, что сервер доступен по адресу {uriToElasticNode}. Возможно индексация временно не работает");
                    WcfLog.Fatal(elasticException,
                        $"Проблема с сервером ElasticSearch. Убедитесь, что сервер доступен по адресу {uriToElasticNode}. Возможно индексация временно не работает");
                }
            }
        }

        public override void UnSubscribe()
        {
            if (context != null)
            {
                if (linkClass != null)
                {
                    linkClass.OutputArcAdded -= ClassRu_OutputArcAdded;
                    linkClass.OutputArcRemoved -= ClassRu_OutputArcRemoved;
                    ConsoleLog.Debug($"{linkClass.SystemIdentifier} отписан на добавление и удаление дуги");
                }

                context.Dispose();
            }
            else
            {
                ConsoleLog.Error("При попытке уничтожения контекса он не найден");
                WcfLog.Error("При попытке уничтожения контекса он не найден");
            }
        }

        protected override void ReindexLinks()
        {
            
                ConsoleLog.Info($"Начинаем реиндексацию всех ссылок класса {linkClass.SystemIdentifier}...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

                var ruLinksIterator =
                    context.CreateIterator(linkClass, ScTypes.ArcAccessConstantPositivePermanent, ScTypes.Link);

                var count = ruLinksIterator.Count();
                var links = ruLinksIterator.Select(construction => construction[2])
                    .OfType<ScLink>()
                    .Select(link => new IndexedLink
                    {
                        Content = link.LinkContent.ToString(),
                        Offset = link.ScAddress.Offset,
                        Segment = link.ScAddress.Segment
                    })
                    .ToList();

        


            var waitHandle = new CountdownEvent(1);
                var bulkAll = ElasticClient.BulkAll(links, b => b
                    .Index(linkClass.SystemIdentifier.ToString())
                    .BackOffRetries(2)
                    .BackOffTime("30s")
                    .RefreshOnCompleted(true)
                    .MaxDegreeOfParallelism(4)
                    .Size(count));

                bulkAll.Subscribe(new BulkAllObserver(
                    b => { Console.Write("."); },
                    c =>
                    {
                        throw new ElasticsearchClientException(
                            $"Ошибка при реиндексации ссылок класса {linkClass.SystemIdentifier.ToString()} ");
                    },
                    () => waitHandle.Signal()));
                waitHandle.Wait();
          
            stopWatch.Stop();
                ConsoleLog.Info(
                    $"Реиндексация {count} ссылок класса {linkClass} завершена за {stopWatch.Elapsed.TotalMinutes} мин.");

           
        }


        #region Обработка событий

        #region Удаление дуги

        private void ClassRu_OutputArcRemoved(object sender, ScEventArgs e)
        {
            var scNode = e.Element as ScNode;
            if (scNode != null)
            {
                ConsoleLog.Debug($"Удалена дуга от {scNode.SystemIdentifier}");

                var link = e.OtherElement as ScLink;

                if (link != null)
                {
                    var indexedLink = new IndexedLink
                    {
                        Content = link.LinkContent.ToString(),
                        Offset = link.ScAddress.Offset,
                        Segment = link.ScAddress.Segment
                    };

                    DeleteLink(indexedLink, scNode.SystemIdentifier.ToString());
                }
                else
                {
                    ConsoleLog.Warn(
                        $"Элемент на конце дуги подписанного класса {scNode.SystemIdentifier} не является ссылкой");
                }
            }
            else
            {
                ConsoleLog.Warn($"Элемент {sender} не является узлом");
            }
        }

        #endregion

        #region Добавление дуги

        private void ClassRu_OutputArcAdded(object sender, ScEventArgs e)
        {
            var scNode = e.Element as ScNode;
            if (scNode != null)
            {
                ConsoleLog.Debug($"Добавлена дуга к {scNode.SystemIdentifier}");

                var link = e.OtherElement as ScLink;

                if (link != null)
                {
                    var indexedLink = new IndexedLink
                    {
                        Content = link.LinkContent.ToString(),
                        Offset = link.ScAddress.Offset,
                        Segment = link.ScAddress.Segment
                    };

                    IndexLink(indexedLink, scNode.SystemIdentifier.ToString());
                    ;
                }
                else
                {
                    ConsoleLog.Warn(
                        $"Элемент на конце дуги подписанного класса {scNode.SystemIdentifier} не является ссылкой");
                }
            }
            else
            {
                ConsoleLog.Warn($"Элемент {sender} не является узлом");
            }
        }

        #endregion

        #endregion
    }
}