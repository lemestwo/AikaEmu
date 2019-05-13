using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestMapInfo : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            Connection.SendPacket(new UpdateMapInfo(Connection.Id));
        }
    }
}