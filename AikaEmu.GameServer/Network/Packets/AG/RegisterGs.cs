using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.AG
{
    public class RegisterGs : AuthGamePacket
    {
        protected override void Read(PacketStream stream)
        {
            if (stream.ReadByte() == 1)
            {
                Log.Info("AuthServer registered with success.");
            }
            else
            {
                Log.Info("AuthServer registration error.");
                Connection.Close();
            }
        }
    }
}