using AikaEmu.GameServer.Packets;
using AikaEmu.GameServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.AuthServer
{
    public class AuthGameConnection : BaseConnection
    {
        public AuthGameConnection(Session session) : base(session)
        {
        }

        public override void OnConnect()
        {
            var gsId = AikaEmu.GameServer.GameServer.Instance.GameConfigs.Id;
            SendPacket(new RegisterGS(gsId));
        }

        public void SendPacket(AuthGamePacket packet)
        {
            Session.SendPacket(packet.Encode());
        }
    }
}