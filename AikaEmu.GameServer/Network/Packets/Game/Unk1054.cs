using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk1054 : GamePacket
    {
        public Unk1054()
        {
            Opcode = (ushort) GameOpcode.Unk1054;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // guild buff? 1 hour duration 
            stream.Write(0); // id
            stream.Write(0); // duration
            return stream;
        }
    }
}
