using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestTradeResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var conId = stream.ReadUInt16();
            stream.ReadUInt16();
            var result = stream.ReadBoolean(true);

            TradeHelper.TradeRequestResult(Connection, conId, result);
        }
    }
}