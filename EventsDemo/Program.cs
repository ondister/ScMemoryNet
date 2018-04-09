using System;
using System.Diagnostics;
using ScEngineNet;
using ScEngineNet.Events;
using ScEngineNet.ScElements;

namespace EventsDemo
{
    internal class Program
    {
        private static ScMemoryContext context;

        private static Stopwatch stopWatch;


        private static void Main(string[] args)
        {
            stopWatch = new Stopwatch();

            if (!ScMemory.IsInitialized)
                ScMemory.Initialize(true, TestParams.ConfigFile, TestParams.RepoPath, TestParams.ExtensionPath,
                    TestParams.NetExtensionPath);

            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            var node = context.CreateNode(ScTypes.Node);
            var node2 = context.CreateNode(ScTypes.Node);

            node.OutputArcAdded += Node_OutputArcAdded;
            node2.InputArcAdded += Node2_InputArcAdded;
            node.OutputArcRemoved += Node_OutputArcRemoved;

            stopWatch.Start();

         var arc=   node.AddOutputArc(node2, ScTypes.ArcAccess);
         arc.DeleteFromMemory();

            Console.ReadKey();
        }

        private static void Node_OutputArcRemoved(object sender, ScEventArgs e)
        {
            stopWatch.Stop();
            Console.WriteLine($" Прошло от инициации события {stopWatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"Дуга {e.Arc.ElementType} с адресом {e.Arc} удалена окончательно");

            Console.WriteLine("Закрываем контекст");
            context.Dispose();
            Console.ReadKey();
        }

        private static void Node2_InputArcAdded(object sender, ScEventArgs e)
        {
            stopWatch.Stop();
            Console.WriteLine($" Прошло от инициации события {stopWatch.ElapsedMilliseconds} мс");
            Console.WriteLine(
                $"К узлу {e.Element.ScAddress} от узла {e.OtherElement.ScAddress} создана дуга {e.Arc.ElementType} с адресом {e.Arc}");
            stopWatch.Start();
        }

        private static void Node_OutputArcAdded(object sender, ScEventArgs e)
        {
            stopWatch.Stop();
            Console.WriteLine($" Прошло от инициации события {stopWatch.ElapsedMilliseconds} мс");
            Console.WriteLine(
                $"От узла {e.Element.ScAddress} к узлу {e.OtherElement.ScAddress} создана дуга {e.Arc.ElementType} с адресом {e.Arc}");
            stopWatch.Start();
        }
    }
}