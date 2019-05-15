using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class CloseMyPersonalStore : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var unk = stream.ReadUInt32();
            Log.Debug("CloseMyPersonalStore, Unk: {0}", unk);
        }
    }
}