using EP.Text;
using ScEngineNet.ExtensionsNet;
using ScEngineNet.SafeElements;
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
            SearchKeyNodes.CreateKeyNodes();
            
            //подписываемся на событие добавление к узлу for_tokenize_string дуги и ссылки
            SearchKeyNodes.ClassLinkForTokenize.OutputArcAdded += ForTokenizeString_OutputArcAdded;

            //инициируем движок 
            pullEntyEngine = new PullEntiEngine();
           

            return ScResult.SC_RESULT_OK;
        }

        void ForTokenizeString_OutputArcAdded(object sender, ScEventArgs e)
        {
            var addedElement= e.Arc.EndElement;
            if (addedElement.ElementType == ElementType.Link_a)
            {
                pullEntyEngine.AddLink((ScLink)addedElement);
            }
        }

        public ScResult ShutDown()
        {
            //отписываемся от события добавление к узлу for_tokenize_string дуги и ссылки
            SearchKeyNodes.ClassLinkForTokenize.OutputArcAdded -= ForTokenizeString_OutputArcAdded; 

            return ScResult.SC_RESULT_OK;
        }
    }
}
