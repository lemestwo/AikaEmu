using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.GameServer
{
    public class GameConnection : BaseConnection
    {
        public GameConnection(Session session) : base(session)
        {
        }

        public void SendPacket(GamePacket packet)
        {
            packet.Connection = this;
            Session.SendPacket(packet.Encode());
        }
    }
}