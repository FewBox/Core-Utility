using System;
using System.Security.Cryptography;
using System.Text;

namespace FewBox.Core.Utility.Security
{
    public static class SaltMD5Utility
    {
        public static string Encrypt(string value, string salt)
        {
            byte[] data = Encoding.ASCII.GetBytes(salt + value);
            data = MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}
