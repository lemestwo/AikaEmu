using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk3057 : GamePacket
    {
        public Unk3057()
        {
            Opcode = (ushort) GameOpcode.Unk3057;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0);
            stream.Write(128); // always this value?
            stream.Write(0);
            return stream;
        }
    }
}