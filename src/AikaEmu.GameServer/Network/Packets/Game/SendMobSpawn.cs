using AikaEmu.GameServer.Models.Units.Mob;
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
            stream.Write(_mob.Model);
            stream.Write(0L);
            stream.Write(_mob.Unk7);
            stream.Write(_mob.Position.CoordX + 0.4f);
            stream.Write(_mob.Position.CoordY + 0.4f);
            stream.Write((short) _mob.Position.Rotation); // rotation?
            stream.Write((short) 0);
            stream.Write(_mob.Hp1);
            stream.Write(_mob.Hp2);
            stream.Write(_mob.Hp3);
            stream.Write(0);
            stream.Write((byte) 0);
            stream.Write(_mob.Unk1); // almost always 25
            stream.Write(_mob.Unk2);
            stream.Write(_mob.Unk3);
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
            stream.Write(_mob.Unk4); // char * << 12
            stream.Write(_mob.Unk5);
            stream.Write(_mob.MobId);
            stream.Write(_mob.Unk6);
            return stream;
        }
    }
}