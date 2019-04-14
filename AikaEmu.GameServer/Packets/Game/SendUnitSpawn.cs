using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Base;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SendUnitSpawn : GamePacket
    {
        private readonly BaseUnit _unit;

        public SendUnitSpawn(BaseUnit unit)
        {
            _unit = unit;
            Opcode = (ushort) GameOpcode.SendUnitSpawn;
        }

        public override PacketStream Write(PacketStream stream)
        {
            if (_unit is Character character)
            {
                stream.Write(character.Name, 16);
                stream.Write((ushort) character.Face);
                stream.Write((ushort) character.Hair);

                stream.Write((short) 0);
                stream.Write((short) 0);
                stream.Write((short) 0);
                stream.Write((short) 0);
                stream.Write((short) 0);
                stream.Write((short) 0);
                stream.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc, 0xcc, 0xcc});

                stream.Write(character.Position.CoordX);
                stream.Write(character.Position.CoordY);
                stream.Write(0);
                stream.Write(character.Hp);
                stream.Write(character.Hp); // TODO - MAX
                stream.Write(character.Mp);
                stream.Write(character.Mp); // TODO - MAX
                stream.Write((byte) 10); // unk
                stream.Write((byte) 45); // unk
                stream.Write((byte) 1); //spawnType
                stream.Write(character.BodyTemplate.Width);
                stream.Write(character.BodyTemplate.Chest);
                stream.Write(character.BodyTemplate.Leg);
                stream.Write((ushort) 0);
                stream.Write((ushort) 0);
                stream.Write((ushort) 0);
                stream.Write("", 60); //buffs
                stream.Write("", 60);

                stream.Write(0);

                stream.Write("", 236);
                stream.Write("", 32); // title
                stream.Write(0);
                stream.Write(0); // test maybe title class
                stream.Write(0);
                stream.Write(0);
                stream.Write(0); // test maybe title class
            }

//
            return stream;
        }
    }
}