using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class QuestRewardChoice : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var questId = stream.ReadUInt32();
            var choice = stream.ReadUInt32();

            Log.Debug("QuestRewardChoice");
        }
    }
}