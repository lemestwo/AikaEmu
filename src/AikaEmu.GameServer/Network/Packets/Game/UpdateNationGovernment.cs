using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateNationGovernment : GamePacket
	{
		private readonly Nation _nation;

		public UpdateNationGovernment(ushort conId, Nation nation)
		{
			_nation = nation;
			Opcode = (ushort) GameOpcode.UpdateNationGovernment;
			SenderId = conId;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write("", 20); // Marshal
			stream.Write("", 20); // Tactician
			stream.Write("", 20); // Judge
			stream.Write("", 20); // Treasurer

			// guild id?
			stream.Write((short) 0); // unk
			stream.Write((short) 0); // unk 
			stream.Write((short) 0); // unk 
			stream.Write((short) 0); // unk 

			stream.Write((int) _nation.Id);
			stream.Write(_nation.Settlement);

			stream.Write((byte) 0);
			stream.Write(_nation.TaxCitizen);
			stream.Write(_nation.TaxVisitor);
			stream.Write((byte) 0); // always 0?

			stream.Write(0); // unk - when visiting anothers faction?
			stream.Write(0); // unk
			stream.Write((byte) 0); // unk
			stream.Write("", 27); // empty?
			
			stream.Write((byte) 1); // positionRank
			stream.Write((ushort) _nation.StabilizationTime); // (byte)
			
			stream.Write((byte) 0); // no use?
			stream.Write(0); // empty fill
			return stream;
		}
	}
}