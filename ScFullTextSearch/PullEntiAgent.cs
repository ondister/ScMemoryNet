using EP.Text;
using ScEngineNet;
using ScEngineNet.Events;
using ScEngineNet.ExtensionsNet;
using ScEngineNet.ScElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScFullTextSearch
{
    public  class PullEntiAgent : IScExtensionNet
    {
        private PullEntiEngine pullEntyEngine;
        private ScMemoryContext context;
        private ScNode classLinkForTokenize;
        private FTSearchEngine ftSearchEngine;
        private ScNode classQuerryString;

        public string NetExtensionName
        {
            get { return "PullEnti Agent "; }
        }

        public string NetExtensionDescription
        {
            get { return "Агент разбор содержимого ссылки на токены"; }
        }

        public ScResult Initialize()
        {
            //инициализируем морфологический движок
            Morphology.Initialize();

            //создаем ключевые узлы
            SearchKeyNodes.Instance.CreateKeyNodes();

            //создаем контекст, который уничтожим только после выгрузки расширения
            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            //подписываемся на событие добавление к узлу class_link_for_tokenize дуги и ссылки
            this.classLinkForTokenize = context.FindNode(SearchKeyNodes.Instance.ClassLinkForTokenize);

            if (this.classLinkForTokenize.ScAddress != ScAddress.Invalid)
            {
                this.classLinkForTokenize.OutputArcAdded += ForTokenizeString_OutputArcAdded;
            }
            else
            {
                Console.WriteLine("KeyNode {0} not found", SearchKeyNodes.Instance.ClassLinkForTokenize);
            }

            //инициируем движок токенизатора
            pullEntyEngine = new PullEntiEngine();

            //подписываемся на событие добавление к узлу class_querry_string нового экземпляра
            this.classQuerryString = context.FindNode(SearchKeyNodes.Instance.ClassQuerryString);

            if (this.classQuerryString.ScAddress != ScAddress.Invalid)
            {
                this.classQuerryString.OutputArcAdded += classQuerryString_OutputArcAdded;
            }
            else
            {
                Console.WriteLine("KeyNode {0} not found", SearchKeyNodes.Instance.ClassQuerryString);
            }
            ftSearchEngine = new FTSearchEngine();

            return ScResult.ScResultOk;
        }

        void classQuerryString_OutputArcAdded(object sender, ScEventArgs e)
        {
            var addedElement = e.Arc.EndElement;
            if (addedElement.ElementType == ScTypes.NodeConstant)
            {
                Console.WriteLine("Querry added");
                ftSearchEngine.AddQuerry((ScNode)addedElement);
            }
        }

        void ForTokenizeString_OutputArcAdded(object sender, ScEventArgs e)
        {
            var addedElement= e.Arc.EndElement;
            if (addedElement.ElementType == ScTypes.Link)
            {
                Console.WriteLine("Link for tokenize added");
                pullEntyEngine.AddLink((ScLink)addedElement);
            }
        }

        public ScResult ShutDown()
        {
            //отписываемся от события добавление к узлу for_tokenize_string дуги и ссылки
            this.classLinkForTokenize.OutputArcAdded -= ForTokenizeString_OutputArcAdded;
            this.classQuerryString.OutputArcAdded -= classQuerryString_OutputArcAdded;

            this.classQuerryString.Dispose();
            this.classLinkForTokenize.Dispose();
            //уничтожаем контекст
            context.Dispose();

            return ScResult.ScResultOk;
        }
    }
}
