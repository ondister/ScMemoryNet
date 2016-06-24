using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ScEngineNet;
using ScEngineNet.ExtensionsNet;
using ScEngineNet.NativeElements;
using ScEngineNet.SafeElements;

namespace ExNetPullEnty
{
    public class ExNetPullenty:IScExtensionNet
    {

        #region Члены IScExtensionNet
        private ScMemoryContext context;
        PullentiEngine pEngine;
        public string NetExtensionName
        {
            get { return "Расширение .Net Pullenti"; }
        }

        public string NetExtensionDescription
        {
            get { return "Расширение .Net для преобразования содержимого ссылок в семантические конструкции"; }
        }
        
        public ScResult Initialize()
        {
           context = new ScMemoryContext(ScAccessLevels.MaxLevel);

           


           //some code 
         pEngine = new PullentiEngine(this.context);
     
            return ScResult.SC_RESULT_OK;
        }

        public ScResult ShutDown()
        {
            //some code
            pEngine.DeleteEvents();
            //
            context.Delete();
                     
         
            return ScResult.SC_RESULT_OK;
        }

        #endregion



        #region Члены IScExtensionNet


       

        #endregion
    }
}
