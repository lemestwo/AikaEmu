using AikaEmu.GameServer.Models.Base;

namespace AikaEmu.GameServer.Models
{
	public class Pran : BaseUnit
	{
		public Account Account { get; set; }
		public uint Experience { get; set; }
		public ushort PDef { get; set; }
		public ushort MDef { get; set; }
		public ushort ConnectionId => Account.ConnectionId;
	}
}