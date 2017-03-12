using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;

namespace ScMachineWrapperTest
{
    [TestClass()]
    public class ScMemoryTests
    {



        [TestInitialize]
        public void InitializeTest()
        {
            Assert.IsFalse(ScMemory.IsInitialized);
            if (!ScMemory.IsInitialized) { ScMemory.Initialize(true, TestParams.ConfigFile, TestParams.RepoPath, TestParams.ExtensionPath, TestParams.NetExtensionPath); }
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
