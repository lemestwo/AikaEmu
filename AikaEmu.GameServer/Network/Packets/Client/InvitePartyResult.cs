using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class InvitePartyResult : GamePacket    
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadUInt32() == 1;
            
        }
    }
}