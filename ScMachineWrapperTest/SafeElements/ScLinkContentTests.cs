using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScEngineNet;
using ScEngineNet.LinkContent;
using ScEngineNet.ScElements;

namespace ScMachineWrapperTest.SafeElements
{
    [TestClass]
    public class ScLinkContentTests
    {
        private static ScLink link;
        private static ScMemoryContext context;

        #region InitializeMemory

        [ClassInitialize]
        public static void InitializeMemory(TestContext testContext)
        {
            if (!ScMemory.IsInitialized)
            {
                ScMemory.Initialize(true, TestParams.ConfigFile, TestParams.RepoPath, TestParams.ExtensionPath,
                    TestParams.NetExtensionPath);
            }
            context = new ScMemoryContext(ScAccessLevels.MinLevel);

            //создаем элементы

            link = context.CreateLink();

            Assert.IsTrue(link.ScAddress.IsValid);
            Assert.AreEqual(ScTypes.Link, link.ElementType);
        }

        #endregion

        #region ShutDownMemory

        [ClassCleanup]
        public static void ShutDown()
        {
            link.Dispose();
            context.Dispose();
            if (ScMemory.IsInitialized)
            {
                ScMemory.ShutDown(true);
            }
        }

        #endregion

        [TestMethod]
        public void CreateScStringLinkTest()
        {
            const string content = "TeststringТестовая строка 1234567890";
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScString), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScInt32LinkTest()
        {
           const Int32 content = Int32.MinValue;
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScInt32), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScLongLinkTest()
        {
            const long content = long.MinValue;
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScLong), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScDoubleLinkTest()
        {
            const double content = double.MinValue;
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScDouble), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();

            const double nAnContent = double.NaN;
            var linkWithNanContent = context.CreateLink(nAnContent);
            context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedNanLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithNanContent.LinkContent, findedNanLinkContent);
            Assert.IsTrue(nAnContent == findedNanLinkContent);
            Assert.AreEqual(typeof (ScDouble), findedNanLinkContent.GetType());

            linkWithNanContent.DeleteFromMemory();
            linkWithNanContent.Dispose();
        }

        [TestMethod]
        public void CreateScByteLinkTest()
        {
            const byte content = byte.MinValue;
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScByte), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScBoolTrueLinkTest()
        {
            const bool content = true;
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScBool), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScBoolFalseLinkTest()
        {
            const bool content = false;
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScBool), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScBynaryLinkTest()
        {
            byte[] content = {1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof (ScBinary), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }

        [TestMethod]
        public void CreateScBitmapLinkTest()
        {
            ScBitmap content = new Bitmap("images/testimage.png");

            var linkWithContent = context.CreateLink(content);
            var findedLinks = context.FindLinks(content);
            Assert.IsTrue(findedLinks.Count == 1);
            var findedLinkContent = findedLinks[0].LinkContent;
            Assert.AreEqual(linkWithContent.LinkContent, findedLinkContent);
            Assert.IsTrue(content == findedLinkContent);
            Assert.AreEqual(typeof(ScBitmap), findedLinkContent.GetType());

            linkWithContent.DeleteFromMemory();
            linkWithContent.Dispose();
        }


        [TestMethod]
        public void EqualsTest()
        {
            #region ScString

            var scString = new ScString("testString");
            var scString1 = new ScString("testString");
            var scString2 = new ScString("тест строки");
            Assert.AreEqual(scString, scString1);
            Assert.AreNotEqual(scString, scString2);
            Assert.IsTrue(scString == scString1);
            Assert.IsTrue(scString != scString2);
            Assert.IsTrue(scString == "testString");
            Assert.IsTrue(scString != "текст строки");

            #endregion

            #region ScInt32

            var scInt32 = new ScInt32(Int32.MaxValue);
            var scInt321 = new ScInt32(Int32.MaxValue);
            var scInt322 = new ScInt32(Int32.MinValue);
            Assert.AreEqual(scInt32, scInt321);
            Assert.AreNotEqual(scInt32, scInt322);
            Assert.IsTrue(scInt32 == scInt321);
            Assert.IsTrue(scInt32 != scInt322);
            Assert.IsTrue(scInt32 == Int32.MaxValue);
            Assert.IsTrue(scInt32 != Int32.MinValue);

            #endregion

            #region ScLong

            var scLong = new ScLong(long.MaxValue);
            var scLong1 = new ScLong(long.MaxValue);
            var scLong2 = new ScLong(long.MinValue);
            Assert.AreEqual(scLong, scLong1);
            Assert.AreNotEqual(scLong, scLong2);
            Assert.IsTrue(scLong == scLong1);
            Assert.IsTrue(scLong != scLong2);
            Assert.IsTrue(scLong == long.MaxValue);
            Assert.IsTrue(scLong != long.MinValue);

            #endregion

            #region ScDouble

            var scDouble = new ScDouble(double.MaxValue);
            var scDouble1 = new ScDouble(double.MaxValue);
            var scDouble2 = new ScDouble(double.MinValue);
            Assert.AreEqual(scDouble, scDouble1);
            Assert.AreNotEqual(scDouble, scDouble2);
            Assert.IsTrue(scDouble == scDouble1);
            Assert.IsTrue(scDouble != scDouble2);
            Assert.IsTrue(scDouble == double.MaxValue);
            Assert.IsTrue(scDouble != double.MinValue);

            #endregion

            #region ScByte

            var scByte = new ScByte(byte.MaxValue);
            var scByte1 = new ScByte(byte.MaxValue);
            var scByte2 = new ScByte(byte.MinValue);
            Assert.AreEqual(scByte, scByte1);
            Assert.AreNotEqual(scByte, scByte2);
            Assert.IsTrue(scByte == scByte1);
            Assert.IsTrue(scByte != scByte2);
            Assert.IsTrue(scByte == byte.MaxValue);
            Assert.IsTrue(scByte != byte.MinValue);

            #endregion

            #region ScBool

            var scBool = ScBool.True;
            var scBool1 = ScBool.True;
            var scBool2 = ScBool.False;
            Assert.AreEqual(scBool, scBool1);
            Assert.AreNotEqual(scBool, scBool2);
            Assert.IsTrue(scBool == scBool1);
            Assert.IsTrue(scBool != scBool2);
            Assert.IsTrue(scBool);
            Assert.IsTrue(scBool);

            #endregion

            #region ScBinary

            var scBinary = new ScBinary(new byte[] {1, 2, 3, 4, 5});
            var scBinary1 = new ScBinary(new byte[] {1, 2, 3, 4, 5});
            var scBinary2 = new ScBinary(new byte[] {5, 4, 3, 2, 1});
            Assert.AreEqual(scBinary, scBinary1);
            Assert.AreNotEqual(scBinary, scBinary2);
            Assert.IsTrue(scBinary == scBinary1);
            Assert.IsTrue(scBinary != scBinary2);
            Assert.IsTrue(scBinary == new byte[] {1, 2, 3, 4, 5});
            Assert.IsTrue(scBinary != new byte[] {5, 4, 3, 2, 1});

            #endregion

            #region TypesEquals

            Assert.AreNotEqual(scString, null);
            Assert.AreNotEqual(scString, scBinary);
            Assert.AreNotEqual(scString, scInt32);
            Assert.AreNotEqual(scString, scLong);
            Assert.AreNotEqual(scString, scDouble);
            Assert.AreNotEqual(scString, scByte);
            Assert.AreNotEqual(scString, true);

            Assert.AreNotEqual(scBinary, null);
            Assert.AreNotEqual(scBinary, scInt32);
            Assert.AreNotEqual(scBinary, scLong);
            Assert.AreNotEqual(scBinary, scDouble);
            Assert.AreNotEqual(scBinary, scByte);
            Assert.AreNotEqual(scBinary, true);

            Assert.AreNotEqual(scInt32, null);
            Assert.AreNotEqual(scInt32, scLong);
            Assert.AreNotEqual(scInt32, scDouble);
            Assert.AreNotEqual(scInt32, scByte);
            Assert.AreNotEqual(scInt32, true);

            Assert.AreNotEqual(scLong, null);
            Assert.AreNotEqual(scLong, scDouble);
            Assert.AreNotEqual(scLong, scByte);
            Assert.AreNotEqual(scLong, true);

            Assert.AreNotEqual(scDouble, null);
            Assert.AreNotEqual(scDouble, scByte);
            Assert.AreNotEqual(scDouble, true);

            Assert.AreNotEqual(scByte, null);
            Assert.AreNotEqual(scByte, true);


            #endregion
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            const string text = "testString";
            var scString = new ScString(text);
            var scString1 = new ScString(text);

            Assert.AreEqual(scString.GetHashCode(), scString1.GetHashCode());
        }

        [TestMethod]
        public void DisposeTest()
        {
            var scString = new ScString("testString");
            Assert.IsFalse(scString.Disposed);
            scString.Dispose();
            Assert.IsTrue(scString.Disposed);
        }
    }
}