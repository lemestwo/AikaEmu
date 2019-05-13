using AikaEmu.AuthServer.Network.GameServer;

namespace AikaEmu.AuthServer.Models
{
	public class GameServer
	{
		public byte Id { get; set; }
		public string Name { get; set; }
		public string Ip { get; set; }
		public short Port { get; set; }
		public GameAuthConnection Connection { get; set; }
	}
}