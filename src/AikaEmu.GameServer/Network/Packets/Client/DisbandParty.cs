using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class DisbandParty : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var id = stream.ReadUInt16();


            var conId = Connection.Id;
            if (id != conId) return;

            var partyData = PartyManager.Instance.GetParty(conId);
            if (partyData == null || partyData.LeaderConId != conId)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "You are not the party leader.")));
                return;
            }

            partyData.SendPacketAll(new SendPartyInfo(null));
            PartyManager.Instance.RemoveParty(partyData.Id);
        }
    }
}