using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk3057 : GamePacket
    {
        public Unk3057()
        {
            Opcode = (ushort) GameOpcode.Unk3057;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0);
            stream.Write((ushort) 128);
            stream.Write((ushort) 2);
            stream.Write(0);
            return stream;
        }
    }
}