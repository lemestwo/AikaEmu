using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendQuestRewardChoice : GamePacket
    {
        private readonly uint _questId;

        public SendQuestRewardChoice(uint questId)
        {
            _questId = questId;
            Opcode = (ushort) GameOpcode.SendQuestRewardChoice;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_questId);
            stream.Write(0);
            return stream;
        }
    }
}