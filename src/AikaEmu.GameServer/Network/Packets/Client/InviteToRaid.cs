using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class InviteToRaid : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var conId = stream.ReadUInt16();

            Log.Debug("InviteToRaid, conId: {0}", conId);
        }
    }
}