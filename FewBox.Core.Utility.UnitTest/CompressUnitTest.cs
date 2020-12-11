using FewBox.Core.Utility.Compress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FewBox.Core.Utility.UnitTest
{
    [TestClass]
    public class CompressUnitTest
    {
        private string ShortInputString { get; set; }
        private string LangInputString { get; set; }
        [TestInitialize]
        public void Init()
        {
            this.ShortInputString = "Hello FewBox!";
            this.LangInputString = @"Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!
            Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!Hello FewBox!";
        }

        [TestMethod]
        public void TestShortGzipUtility()
        {
            string gzipString = GzipUtility.Zip(this.ShortInputString);
            //Assert.IsTrue(gzipString.Length < this.ShortInputString.Length);
            string originalString = GzipUtility.Unzip(gzipString);
            Assert.AreEqual(this.ShortInputString, originalString);
        }

        [TestMethod]
        public void TestLangGzipUtility()
        {
            string gzipString = GzipUtility.Zip(this.LangInputString);
            Assert.IsTrue(gzipString.Length < this.LangInputString.Length);
            string originalString = GzipUtility.Unzip(gzipString);
            Assert.AreEqual(this.LangInputString, originalString);
        }
    }
}