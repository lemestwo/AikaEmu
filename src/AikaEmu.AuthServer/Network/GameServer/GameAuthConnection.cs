using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Network.GameServer
{
    public class GameAuthConnection : BaseConnection
    {
        public Models.GameServer GameServer { get; set; }
        public GameAuthConnection(Session session) : base(session)
        {
        }
        
        public void SendPacket(GameAuthPacket packet)
        {
            Session?.SendPacket(packet.Encode());
        }
    }
}