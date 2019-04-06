using System.Net;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.AuthServer.Network.AuthServer
{
    public class AuthConnection : BaseConnection
    {
        public uint AccountId { get; set; }

        public AuthConnection(Session session) : base(session)
        {
        }

        public void SendPacket(AuthPacket packet)
        {
            Session.SendPacket(packet.Encode());
        }
    }
}