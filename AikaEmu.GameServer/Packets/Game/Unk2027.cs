using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk2027 : GamePacket
    {
        public Unk2027()
        {
            Opcode = (ushort) GameOpcode.Unk2027;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((ushort) 0);
            stream.Write(2000);
            stream.Write("", 6);
            stream.Write(100000);
            return stream;
        }
    }
}