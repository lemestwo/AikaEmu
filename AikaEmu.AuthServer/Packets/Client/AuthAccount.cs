using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.Client
{
    public class AuthAccount : AuthPacket
    {
        protected override void Read(PacketStream stream)
        {
            var user = stream.ReadString(32).Trim();
            var hash = stream.ReadString(32);
            stream.ReadBytes(994);
            var gsId = stream.ReadByte();
            // then 45 more empty bytes

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(hash))
            {
                Log.Error("Login information is empty.");
                return;
            }

            AuthGameManager.Instance.Authenticate(Connection, user, hash);
            Log.Info("Attempt to auth: ({2}) {0}:{1}", user, hash, gsId);
        }
    }
}