using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
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