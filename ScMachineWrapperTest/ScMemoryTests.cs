using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScMemoryNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ScMemoryNet.Tests
{
    [TestClass()]
    public class ScMemoryTests
    {
        const string configFile = @"d:\OSTIS\sc-machine-master\bin\sc-memory.ini";
        const string repoPath = @"d:\OSTIS\sc-machine-master\bin\repo";
        const string extensionPath = @"d:\OSTIS\sc-machine-master\bin\extensions";
        const string netExtensionPath = "";


        [TestInitialize]
        public void InitializeTest()
        {
            Assert.IsFalse(ScMemory.IsInitialized);
            if (!ScMemory.IsInitialized) { ScMemory.Initialize(true, configFile, repoPath, extensionPath, netExtensionPath); }
        }

        [TestMethod]
        public void IsInitializedTest()
        {
            Assert.IsTrue(ScMemory.IsInitialized);
        }

        [TestCleanup]
        public void ShutDownTest()
        {
            if (ScMemory.IsInitialized) { ScMemory.ShutDown(true); }
        }
    }
}
