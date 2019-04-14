using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Packets;
using AikaEmu.GameServer.Packets.GA;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using RegisterGs = AikaEmu.GameServer.Packets.GA.RegisterGs;

namespace AikaEmu.GameServer.Network.AuthServer
{
    public class AuthGameConnection : BaseConnection
    {
        public AuthGameConnection(Session session) : base(session)
        {
        }

        public void OnConnect()
        {
            // TODO - KEY FOR INTERNAL REGISTRATION
            var gsId = AikaEmu.GameServer.GameServer.Instance.GameConfigs.Id;
            SendPacket(new RegisterGs(gsId));
        }

        public void SendPacket(AuthGamePacket packet)
        {
            Session.SendPacket(packet.Encode());
        }
    }
}