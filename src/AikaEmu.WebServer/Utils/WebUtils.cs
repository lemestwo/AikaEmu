using System;
using System.Security.Cryptography;
using System.Text;

namespace AikaEmu.WebServer.Utils
{
    public static class WebUtils
    {
        public static string GenerateRandomHash()
        {
            using (var md5 = MD5.Create())
            {
                var md5Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                var hash = new StringBuilder();
                foreach (var mByte in md5Byte)
                {
                    hash.Append(mByte.ToString("x2"));
                }

                return hash.ToString();
            }
        }
    }
}