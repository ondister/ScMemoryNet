namespace ScMemoryNet.Models.ScNetExtension.ElasticIndexerModel
{
    /// <summary>
    /// Класс обертка для индексации ссылки в ElasticSearch
    /// </summary>
   public class IndexedLink
    {
        public int Offset { get; set; }
        public int Segment { get; set; }

        public string Content { get; set; }
    }
}
