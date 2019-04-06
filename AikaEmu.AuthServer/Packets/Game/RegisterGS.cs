using AikaEmu.AuthServer.Controllers;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.Game
{
    public class RegisterGS : GameAuthPacket
    {
        public override void Read(PacketStream stream)
        {
            var gsId = stream.ReadByte();
            GameAuthController.Instance.Add(gsId, Connection);
        }
    }
}