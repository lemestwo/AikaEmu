using System;

namespace AikaEmu.WebServer
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                WebServer.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}