using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestAddFriend : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var unk = stream.ReadUInt32();
            var name = stream.ReadString(16);
            Log.Debug("RequestAddFriend, Unk: {0}, Name: {1}", unk, name);
        }
    }
}