using ScEngineNet;
using ScEngineNet.ExtensionsNet;
using ScEngineNet.SafeElements;

namespace ExNetPullEnty
{
    public class ExNetPullenty : IScExtensionNet
    {
        #region Члены IScExtensionNet

        private ScMemoryContext context;
        private PullentiEngine pullentiEngine;

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
           pullentiEngine = new PullentiEngine(this.context);
           return ScResult.SC_RESULT_OK;
        }

        public ScResult ShutDown()
        {
            pullentiEngine.DeleteEvents();
            context.Delete();
            return ScResult.SC_RESULT_OK;
        }

        #endregion
    }
}
