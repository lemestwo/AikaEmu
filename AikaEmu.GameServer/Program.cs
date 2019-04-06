using System;

namespace AikaEmu.GameServer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                GameServer.Instance.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}