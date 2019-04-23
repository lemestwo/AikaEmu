using System;

namespace AikaEmu.AuthServer
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				AuthServer.Run();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}