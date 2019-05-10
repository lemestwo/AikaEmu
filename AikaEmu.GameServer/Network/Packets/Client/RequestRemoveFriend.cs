using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestRemoveFriend : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var id = stream.ReadUInt32();
            Log.Debug("RequestRemoveFriend, Id: {0}", id);
        }
    }
}