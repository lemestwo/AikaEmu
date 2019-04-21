using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models
{
	public class Quest : BasePacket
	{
		public ushort QuestId { get; set; }
		public bool isCompleted { get; set; }

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(QuestId);
			stream.Write("", 26); // datA?
			stream.Write(isCompleted);
			stream.Write((byte) 0);
			stream.Write((ushort) 0);
			return stream;
		}
	}
}