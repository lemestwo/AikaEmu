using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateBuffs : GamePacket
	{
		private readonly BaseUnit _unit;

		public UpdateBuffs(BaseUnit unit)
		{
			_unit = unit;

			Opcode = (ushort) GameOpcode.UpdateBuffs;
			SetSenderIdWithUnit(unit);
		}

		public override PacketStream Write(PacketStream stream)
		{
			var bid = (ushort) 9031;
			var test1 = (byte) 206;
			var test2 = (byte) 8;
			var test3 = (ushort) 23736;
			stream.Write(0); // unk
			stream.Write((ushort) bid); // buffId
			stream.Write((ushort) 0); //unk
			stream.Write(0L); //unk
			stream.Write((byte) test1); // unk
			stream.Write((byte) test2); //unk
			stream.Write((ushort) test3); // unk
			return stream;
			
			/*
			 
			v5 = *(_WORD *)(data + 12);
			v6 = *(_WORD *)(data + 14);
			v7 = *(_WORD *)(data + 16);
			v8 = *(_DWORD *)(data + 20);
			v9 = *(_DWORD *)(data + 24);
			v10 = *(_DWORD *)(data + 28);
			
			 */
		}
	}
}