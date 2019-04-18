using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Base;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class UpdatePosition : GamePacket
	{
		private readonly BaseUnit _unit;
		private readonly bool _isTp;

		public UpdatePosition(BaseUnit unit, bool isTp)
		{
			_unit = unit;
			_isTp = isTp;

			Opcode = (ushort) GameOpcode.UpdatePosition;
			SetSenderIdWithUnit(_unit);
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_unit.Position.CoordX);
			stream.Write(_unit.Position.CoordY);
			stream.Write(0); // unk
			stream.Write((ushort) 0); // unk
			stream.Write(_isTp);
			stream.Write((byte) 0); // TODO - Movement Speed (0 for TP)
			stream.Write(0); // unk
			return stream;
		}
	}
}