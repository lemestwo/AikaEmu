using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendQuestInfo : GamePacket
    {
        public SendQuestInfo()
        {
            Opcode = (ushort) GameOpcode.SendQuestInfo;
        }

        public override PacketStream Write(PacketStream stream)
        {
            
            return stream;
        }
    }
}