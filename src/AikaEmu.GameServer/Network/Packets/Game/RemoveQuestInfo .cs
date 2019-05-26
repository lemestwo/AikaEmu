using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class RemoveQuestInfo : GamePacket
    {
        private readonly ushort _questId;

        public RemoveQuestInfo(ushort questId)
        {
            _questId = questId;
            Opcode = (ushort) GameOpcode.RemoveQuestInfo;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _questId);
            return stream;
        }
    }
}