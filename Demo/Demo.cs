using System;
using System.Diagnostics;
using System.Drawing;
using ScEngineNet;
using ScEngineNet.LinkContent;
using ScEngineNet.ScElements;

namespace Demo
{
    public class Demo
    {
        private Stopwatch watch;
        /// <summary>
        ///     Повазывает возможность сохранения ссылки с содержимым типа изображение
        /// </summary>
        public void Start()
        {
            watch= new Stopwatch();
            const string configFile = @"d:\develop\ostis\ostis-master\config\sc-web.ini";
            const string repoPath = @"d:\develop\ostis\ostis-master\kb.bin";
            const string extensionPath = @"d:\develop\ostis\ostis-master\sc-machine\bin\extensions";
            const string netExtensionPath = @"d:\develop\ostis\ostis-master\sc-machine\bin\netextensions";
            ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath);
            Console.WriteLine(ScMemory.IsInitialized);

            var context = new ScMemoryContext(ScAccessLevels.MinLevel);
            var node = context.CreateNode(ScTypes.NodeConstant);
            var link = context.CreateLink("link");
            link.InputArcAdded += Link_InputArcAdded;
          
            node.ElementRemoved += Node_ElementRemoved;

            watch.Start();
            node.AddOutputArc(link, ScTypes.ArcCommonVariable);
            node.DeleteFromMemory();

            Console.ReadKey();
            node.Dispose();
            link.Dispose();
            context.Dispose();
            ScMemory.ShutDown(false);
        }

        private void Node_ElementRemoved(object sender, ScEngineNet.Events.ScEventArgs e)
        {
            Console.WriteLine("node delete for: {0} ms", watch.ElapsedMilliseconds);
        }

        private void Link_InputArcAdded(object sender, ScEngineNet.Events.ScEventArgs e)
        {
            watch.Stop();
            Console.WriteLine("Input arc added for: {0} ms", watch.ElapsedMilliseconds);
        }
    }
}