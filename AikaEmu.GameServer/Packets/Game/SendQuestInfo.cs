using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
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