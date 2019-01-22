using System;
using System.Text;

namespace FewBox.Core.Utility.Formatter
{
    public static class Base64Utility
    {
        public static string Serialize(string pureString)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pureString));
        }

        public static string Deserialize(string base64String)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        }
    }
}