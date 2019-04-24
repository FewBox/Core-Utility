using System;
using FewBox.Core.Utility.Formatter;
using FewBox.Core.Utility.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FewBox.Core.Utility.UnitTest
{
    [TestClass]
    public class SecurityUnitTest
    {
        [TestMethod]
        public void TestMD5Utility()
        {
            string value = "landpy";
            string salt = "466fe1e1-1efc-4ca9-9409-ee0dc624bf8e";
            string result = SaltMD5Utility.Encrypt(value, salt);
            Assert.IsNotNull(result);
        }
    }
}
