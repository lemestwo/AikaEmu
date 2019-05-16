using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class InviteToPartyResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadBoolean(true);
            GroupHelper.PartyRequestResult(Connection, result);
        }
    }
}