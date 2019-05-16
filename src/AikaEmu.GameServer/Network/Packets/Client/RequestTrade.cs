using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestTrade : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var targetConId = stream.ReadUInt16();

            TradeHelper.TradeRequest(Connection, targetConId);
        }
    }
}