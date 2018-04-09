using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using NLog;
using Sapfir.Clients.Log;
using Sapfir.Models.ScNetExtension.ElasticIndexerModel;

namespace Sapfir.Servers.SemanticServer.ElasticIndexer_netextension
{
   internal abstract class LinkIndexer
    {
        protected readonly Logger ConsoleLog =
            LoggerFactory.CreateConsoleLogger("Sapfir.Servers.SemanticServer.ElasticIndexer.LinkIndexer");

        protected readonly Logger WcfLog =
            LoggerFactory.CreateWcfLogger("Sapfir.Servers.SemanticServer.ElasticIndexer.LinkIndexer");

        protected ElasticClient ElasticClient;

        public abstract void Subscribe();

        public abstract void UnSubscribe();

        protected abstract void ReindexLinks();

        protected void IndexLink(IndexedLink link, string indexName)
        {
            try
            {
                //находим ссылку с таким же адресом
                var request = new SearchRequest
                {
                    From = 0,
                    Size = 1,
                    Query = new TermQuery { Field = "Offset", Value = link.Offset } &&
                            new TermQuery { Field = "Segment", Value = link.Segment }
                };
                var response = ElasticClient.Search<IndexedLink>(request);

                //если она есть, то удаляем ее из индекса
                if (response.Documents.Any())
                {
                    ElasticClient.Delete<IndexedLink>(link, idx => idx.Index(indexName));
                }

                var indexResult =  ElasticClient.Index(link, idx => idx.Index(indexName));
                if (!indexResult.IsValid)
                {
                    ConsoleLog.Error(
                        $"Cервер ElasticSearch возвратил неверный результат при индексации в индекс {indexName} ссылки {link.Content}.");
                    WcfLog.Error(
                        $"Cервер ElasticSearch возвратил неверный результат при индексации в индекс {indexName} ссылки {link.Content}.");
                }
              
            }
            catch (ElasticsearchClientException elasticException)
            {
                ConsoleLog.Error(elasticException,
                    $"Проблема с сервером ElasticSearch при индексации в индекс {indexName} ссылки {link.Content}.");
                WcfLog.Error(elasticException,
                    $"Проблема с сервером ElasticSearch при индексации в индекс {indexName} ссылки {link.Content}.");
            }
        }

        protected void DeleteLink(IndexedLink link, string indexName)
        {
            try
            {
                //находим ссылку с таким же адресом
                var request = new SearchRequest
                {
                    From = 0,
                    Size = 1,
                    Query = new TermQuery { Field = "Offset", Value = link.Offset } &&
                            new TermQuery { Field = "Segment", Value = link.Segment }
                };
                var response = ElasticClient.Search<IndexedLink>(request);

                //если она есть, то удаляем ее из индекса
                if (response.Documents.Any())
                {
                    var deleteResponse = ElasticClient.Delete<IndexedLink>(link, idx => idx.Index(indexName));
                    if (!deleteResponse.IsValid)
                    {
                        var message =
                            $"Cервер ElasticSearch возвратил неверный результат при удалении из индекса {indexName} ссылки {link.Content}.";
                        ConsoleLog.Error(message);
                        WcfLog.Error(message);
                    }
                  
                }

            }
            catch (ElasticsearchClientException elasticException)
            {
                ConsoleLog.Error(elasticException,
                    $"Проблема с сервером ElasticSearch при индексации в индекс {indexName} ссылки {link.Content}.");
                WcfLog.Error(elasticException,
                    $"Проблема с сервером ElasticSearch при индексации в индекс {indexName} ссылки {link.Content}.");
            }
        }
    }
}
