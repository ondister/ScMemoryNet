using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScEngineNet.Events;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest
{
    /// <summary>
    /// Необходимо переписать все тесты на события
    /// </summary>
    [TestClass]
    
    public class EventsTests
    {
        private static ScNode node;
        private static ScLink link;
        private static ScNode nrelNode;
        private static ScArc commonArc;
        private static ScMemoryContext context;

        #region InitializeMemory

       
        public  void InitializeMemory()
        {
            if (!ScMemory.IsInitialized)
            {
                ScMemory.Initialize(true, TestParams.ConfigFile, TestParams.RepoPath, TestParams.ExtensionPath,
                    TestParams.NetExtensionPath);
            }
            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            //создаем элементы
            node = context.CreateNode(ScTypes.NodeConstant);
            node.SystemIdentifier = "test_construction_node";
            link = context.CreateLink("link");
            nrelNode = context.CreateNode(ScTypes.NodeConstantNonRole);


            Assert.IsTrue(node.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.NodeConstant, node.ElementType);

            Assert.IsTrue(link.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.Link, link.ElementType);

            Assert.IsTrue(nrelNode.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.NodeConstantNonRole, nrelNode.ElementType);
        }

        #endregion

        #region ShutDownMemory

    
        public  void ShutDown()
        {
            node.Dispose();
            link.Dispose();
            nrelNode.Dispose();

            context.Dispose();
            if (ScMemory.IsInitialized)
            {
                ScMemory.ShutDown(true);
            }
        }

        #endregion

        #region EventTests

        [TestMethod]
        public void RunEventTests()
        {
            InitializeMemory();

          //  TestAddInputArcEvent();
            TestAddOutputArcEvent();
            //TestInputArcRemovedEvent();
            //TestOutputArcRemovedEvent();
            //TestChangeLinkContentEvent();



            //тесты на неправильное использование
        //    TestAddDoubleEvent();
           // TestRemoveVoidEvent();
            //тест на удаление элемента
        //    TestElementRemovedEvent();

            ShutDown();
        }

        public void TestAddOutputArcEvent()
        {
            object obj = null;
            ScArc arc = null;
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventAddOutputArc;

            var autoResetEvent = new AutoResetEvent(false);
            node.OutputArcAdded += delegate(object o, ScEventArgs e)
            {
                obj = o;
                //arc = e.Arc;
                //eventType = e.EventType;
                //autoResetEvent.Set();
            };
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            autoResetEvent.WaitOne();

            Assert.AreEqual(node, (ScNode) obj);
            Assert.AreEqual(commonArc, arc);
            Assert.AreEqual(expectedEventType, eventType);

            commonArc.DeleteFromMemory();
            commonArc.Dispose();
        }


        public void TestAddInputArcEvent()
        {
            object obj = null;
            ScArc arc = null;
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventAddInputArc;

            var autoResetEvent = new AutoResetEvent(false);
            link.InputArcAdded += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            autoResetEvent.WaitOne();
            Assert.AreEqual(link, (ScLink) obj);
            Assert.AreEqual(commonArc, arc);
            Assert.AreEqual(expectedEventType, eventType);

            commonArc.DeleteFromMemory();
            commonArc.Dispose();
        }


        public void TestElementRemovedEvent()
        {
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventRemoveElement;

            var autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);

            commonArc.ElementRemoved += delegate(object o, ScEventArgs e)
            {
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();

            Assert.AreEqual(expectedEventType, eventType);
            commonArc.Dispose();
        }


        public void TestOutputArcRemovedEvent()
        {
            object obj = null;
            ScArc arc = null;
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventRemoveOutputArc;

            var autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            node.OutputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();
            Assert.AreEqual(node, (ScNode) obj);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);
            commonArc.Dispose();
        }


        public void TestInputArcRemovedEvent()
        {
            object obj = null;
            ScArc arc = null;
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventRemoveInputArc;

            var autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            link.InputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();

            Assert.AreEqual(link, (ScLink) obj);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);

            commonArc.Dispose();
        }

 
        public void TestAddDoubleEvent()
        {
            object obj = null;
            ScArc arc = null;
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventRemoveInputArc;

            var autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            link.InputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                eventType = e.EventType;
                autoResetEvent.Set();
            };

            link.InputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                eventType = e.EventType;
                autoResetEvent.Set();
            };


            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();
            Assert.AreEqual(link, (ScLink) obj);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);



            commonArc.Dispose();
        }


        public void TestRemoveVoidEvent()
        {
            var link1 = context.CreateLink("testlink");
            commonArc = node.AddOutputArc(link1, ScTypes.ArcCommonConstant);
            if (link1 != null)
            {
                link1.InputArcRemoved -= delegate { };
                link1.DeleteFromMemory();
                link1.Dispose();
            }
        }


        public void TestChangeLinkContentEvent()
        {
            object obj = null;
            var eventType = ScEventType.ScEventUnknown;
            const ScEventType expectedEventType = ScEventType.ScEventContentChanged;

            var autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            link.ContentChanged += delegate(object o, ScEventArgs e)
            {
                obj = o;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            link.LinkContent = "new link content";
            autoResetEvent.WaitOne();
            Assert.AreEqual(link, (ScLink) obj);
            Assert.AreEqual(expectedEventType, eventType);
        }

        #endregion
    }
}