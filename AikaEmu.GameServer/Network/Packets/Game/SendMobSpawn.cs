using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class SendMobSpawn : GamePacket
	{
		private readonly Mob _mob;

		public SendMobSpawn(Mob mob)
		{
			_mob = mob;
			Opcode = (ushort) GameOpcode.SendMobSpawn;
			SenderId = (ushort) mob.Id;
		}

		public override PacketStream Write(PacketStream stream)
		{
			// 16bytes (mobId+?)
			stream.Write(DataManager.Instance.MobEffectsData.GetFace((ushort) _mob.MobId));
			stream.Write("", 14);

			stream.Write(_mob.Position.CoordX + 0.4f);
			stream.Write(_mob.Position.CoordY + 0.4f);
			stream.Write((short) 122); // rotation?

			stream.Write((short) 0);
			stream.Write(_mob.Hp);
			stream.Write(0); // 0?
			stream.Write(_mob.Hp);
			stream.Write(0);
			stream.Write((byte) 0);
			stream.Write((byte) 25); // almost always 25
			stream.Write((byte) 1);
			stream.Write((byte) 0);
			stream.Write((ushort) 0); // empty fill
			stream.Write((short) 0);
			stream.Write((byte) 0);
			stream.Write((byte) 0);
			stream.Write((byte) 0);
			stream.Write((byte) 0);
			stream.Write((byte) 0);
			stream.Write(_mob.BodyTemplate.Width);
			stream.Write(_mob.BodyTemplate.Chest);
			stream.Write(_mob.BodyTemplate.Leg);
			stream.Write((byte) 0); // empty fill
			stream.Write((byte) 0);
			stream.Write((byte) 1); // char * << 12
			stream.Write((byte) 4);
			stream.Write(_mob.MobId);
			stream.Write((uint) 0);
			return stream;
		}
	}
}