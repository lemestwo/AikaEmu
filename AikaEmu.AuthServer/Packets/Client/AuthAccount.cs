using AikaEmu.AuthServer.Network;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets
{
    public class AuthAccount : AuthPacket
    {
        public override void Read(PacketStream stream)
        {
            Connection.SendPacket(new ACAuthSuccess());
            Log.Info("CAAuthLogin");
        }
    }
}