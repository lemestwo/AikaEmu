using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.GA
{
    public class RegisterGs : GameAuthPacket
    {
        protected override void Read(PacketStream stream)
        {
            var gsId = stream.ReadByte();
            AuthGameManager.Instance.Add(gsId, Connection);
        }
    }
}