using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestCancelTrade : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            // stream.ReadUInt32();
            TradeHelper.TradeRequestCancel(Connection);
        }
    }
}