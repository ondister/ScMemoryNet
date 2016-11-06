using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScEngineNet.Events;
using ScEngineNet.ScElements;
using ScMemoryNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScEngineNetTest
{
    [TestClass]
    public class EventsTests
    {
        static ScNode node;
        static ScLink link;
        static ScNode nrelNode;
        static ScArc commonArc;
        const string configFile = @"d:\OSTIS\sc-machine-master\bin\sc-memory.ini";
        const string repoPath = @"d:\OSTIS\sc-machine-master\bin\repo";
        const string extensionPath = @"d:\OSTIS\sc-machine-master\bin\extensions";
        const string netExtensionPath = "";
        static ScMemoryContext context;


        #region InitializeMemory

        [ClassInitialize]
        public static void InitializeMemory(TestContext testContext)
        {
            if (!ScMemory.IsInitialized) { ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath); }
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
        [ClassCleanup]
        public static void ShutDown()
        {
            node.Dispose();
            link.Dispose();
            nrelNode.Dispose();

            context.Dispose();
            if (ScMemory.IsInitialized) { ScMemory.ShutDown(true); }
        }
        #endregion

        #region EventTests
        [TestMethod]
        public void TestAddOutputArcEvent()
        {
            object obj = null;
            ScArc arc = null;
            ScElement scElement = null;
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_ADD_OUTPUT_ARC;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            node.OutputArcAdded += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            autoResetEvent.WaitOne();

           Assert.AreEqual(node, (ScNode)obj);
            Assert.AreEqual(node, (ScNode)scElement);
            Assert.AreEqual(commonArc, arc);
            Assert.AreEqual(expectedEventType, eventType);

            commonArc.DeleteFromMemory();
            commonArc.Dispose();
        }

        [TestMethod]
        public void TestAddInputArcEvent()
        {
            object obj = null;
            ScArc arc = null;
            ScElement scElement = null;
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_ADD_INPUT_ARC;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            link.InputArcAdded += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            autoResetEvent.WaitOne();

            Assert.AreEqual(link, (ScLink)obj);
            Assert.AreEqual(link, (ScLink)scElement);
            Assert.AreEqual(commonArc, arc);
            Assert.AreEqual(expectedEventType, eventType);

            commonArc.DeleteFromMemory();
            commonArc.Dispose();
        }

        [TestMethod]
        public void TestElementRemovedEvent()
        {
           
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_REMOVE_ELEMENT;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);

            commonArc.ElementRemoved += delegate(object o, ScEventArgs e)
            {
                Assert.IsFalse(((ScElement)o).IsValid);
                Assert.IsNull(e.Arc);
                Assert.IsNull(e.Element);
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();

            Assert.AreEqual(expectedEventType, eventType);
            commonArc.Dispose();
        }

        [TestMethod]
        public void TestOutputArcRemovedEvent()
        {
            object obj = null;
            ScArc arc = null;
            ScElement scElement = null;
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_REMOVE_OUTPUT_ARC;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            node.OutputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();

            Assert.AreEqual(node, (ScNode)obj);
            Assert.AreEqual(node, (ScNode)scElement);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);
            commonArc.Dispose();
            
        }

        [TestMethod]
        public void TestInputArcRemovedEvent()
        {
            object obj = null;
            ScArc arc = null;
            ScElement scElement = null;
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_REMOVE_INPUT_ARC;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            link.InputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();

            Assert.AreEqual(link, (ScLink)obj);
            Assert.AreEqual(link, (ScLink)scElement);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);

            commonArc.Dispose();
        }

        [TestMethod]
        public void TestAddDoubleEvent()
        {
            object obj = null;
            ScArc arc = null;
            ScElement scElement = null;
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_REMOVE_INPUT_ARC;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            link.InputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };

            link.InputArcRemoved += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };


            commonArc.DeleteFromMemory();
            autoResetEvent.WaitOne();

            Assert.AreEqual(link, (ScLink)obj);
            Assert.AreEqual(link, (ScLink)scElement);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);

            link.InputArcRemoved -= delegate(object o, ScEventArgs e)
            {
               
            };
            link.InputArcRemoved -= delegate(object o, ScEventArgs e)
            {

            };
            
            
            commonArc.Dispose();
        }

        [TestMethod]
        public void TestRemoveVoidEvent()
        {

            var link1 = context.CreateLink("testlink");
            commonArc = node.AddOutputArc(link1, ScTypes.ArcCommonConstant);
            link1.InputArcRemoved -= delegate(object o, ScEventArgs e)
            {
               
            };
            link1.DeleteFromMemory();
            link1.Dispose();
          
        }

        [TestMethod]
        public void TestChangeLinkContentEvent()
        {

            object obj = null;
            ScArc arc = null;
            ScElement scElement = null;
            ScEventType eventType = ScEventType.SC_EVENT_UNKNOWN;
            ScEventType expectedEventType = ScEventType.SC_EVENT_CONTENT_CHANGED;

            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            commonArc = node.AddOutputArc(link, ScTypes.ArcCommonConstant);
            link.ContentChanged += delegate(object o, ScEventArgs e)
            {
                obj = o;
                arc = e.Arc;
                scElement = e.Element;
                eventType = e.EventType;
                autoResetEvent.Set();
            };
            link.LinkContent = "new link content";
            autoResetEvent.WaitOne();

            Assert.AreEqual(link, (ScLink)obj);
            Assert.AreEqual(link, (ScLink)scElement);
            Assert.IsNotNull(arc);
            Assert.AreEqual(expectedEventType, eventType);

        }


        #endregion
    }
}
