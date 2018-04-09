using Sapfir.Models.ScNetExtension;

namespace Sapfir.Servers.SemanticServer.ElasticIndexer_netextension
{
    public class ExtensionContainer : IScExtensionNet
    {
        private EnTextIndexer textIndexerEn;

        private RuTextIndexer textIndexerRu;
        public string NetExtensionName => "ElasticSearchIndexer";

        public string NetExtensionDescription =>
            "Расширение для индексации ссылок в систему полнотекстового поиска ElasticSearch";

        public bool Initialize()
        {
            ElasticSearchConfig.Read();

            //подписываем индексаторы
            textIndexerRu = new RuTextIndexer();
            textIndexerRu.Subscribe();

            textIndexerEn = new EnTextIndexer();
            textIndexerEn.Subscribe();

            return true;
        }

        public bool ShutDown()
        {
           textIndexerRu.UnSubscribe();
           textIndexerEn.UnSubscribe();
            return true;
        }
    }
}