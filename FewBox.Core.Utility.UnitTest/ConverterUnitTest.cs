using System;
using System.Diagnostics;
using FewBox.Core.Utility.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FewBox.Core.Utility.UnitTest
{
    [TestClass]
    public class ConverterUnitTest
    {
        [TestMethod]
        public void TestConvert()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            dynamic from = Guid.NewGuid();
            // Guid to Guid
            dynamic to = TypeUtility.Converte<Guid>(from);
            Assert.AreEqual(from, to);
            // Guid to String
            to = TypeUtility.Converte<string>(from);
            Assert.AreEqual(from.ToString(), to);
            // String 2 Guid
            to = TypeUtility.Converte<Guid>(from.ToString());
            Assert.AreEqual(from, to);
            // String to String
            from = "FewBox";
            to = TypeUtility.Converte<string>(from);
            Assert.AreEqual(from, to);
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.Elapsed.Milliseconds < 500, $"Time: {stopwatch.Elapsed.Milliseconds} milliseconds.");
        }
    }
}