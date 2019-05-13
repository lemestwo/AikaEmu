using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestTokenResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadInt32();
            
            // TODO
            Connection.SendPacket(new SendTokenResult(result));
        }
    }
}