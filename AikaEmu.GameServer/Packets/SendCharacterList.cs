using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets
{
    public class SendCharacterList : GamePacket
    {
        public SendCharacterList()
        {
            Opcode = (ushort) GameOpcode.SendCharacterList;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(1);
            stream.Write(1);
            return stream;
        }
    }
}