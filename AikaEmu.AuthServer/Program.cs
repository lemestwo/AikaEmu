using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;

namespace AikaEmu.AuthServer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                AuthServer.Instance.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
     }
}