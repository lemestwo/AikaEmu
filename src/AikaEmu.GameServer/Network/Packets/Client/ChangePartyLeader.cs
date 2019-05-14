using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class ChangePartyLeader : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var id = stream.ReadUInt16();

            var partyData = PartyManager.Instance.GetParty(Connection.Id);
            if (partyData.IsMember(id))
            {
                partyData.LeaderConId = id;
                partyData.UpdateParty();
            }
        }
    }
}