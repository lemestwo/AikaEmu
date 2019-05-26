using AikaEmu.AuthServer.Models;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Network.AuthServer
{
    public class AuthConnection : BaseConnection
    {
        public Account Account { get; set; }

        public AuthConnection(Session session) : base(session)
        {
        }

        public void SendPacket(AuthPacket packet)
        {
            Session.SendPacket(packet.Encode());
        }
    }
}