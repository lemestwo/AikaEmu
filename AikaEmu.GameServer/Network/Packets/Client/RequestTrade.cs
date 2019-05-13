using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestTrade : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var targetConId = stream.ReadUInt16();

            Log.Debug("RequestTrade, target: {0}", targetConId);

            var target = WorldManager.Instance.GetCharacter(targetConId);
            target?.SendPacket(new SendTradeRequest(targetConId));
        }
    }
}