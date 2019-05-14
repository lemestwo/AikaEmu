using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Group;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class InvitePartyResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadUInt32() == 1;

            var invitation = PartyManager.Instance.GetInvite(Connection.Id);
            if (invitation == null)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Party not available anymore.")));
                return;
            }

            PartyManager.Instance.RemoveInvite(Connection.Id);

            var isPartyAlready = PartyManager.Instance.GetParty(Connection.Id);
            if (isPartyAlready != null)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Already in a party.")));
                return;
            }

            var owner = WorldManager.Instance.GetCharacter(invitation.ConInviter);
            if (owner == null)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Party not available right now.")));
                return;
            }

            if (!result) return;

            var partyData = PartyManager.Instance.GetParty(invitation.ConInviter);
            if (partyData != null && partyData.LeaderConId == invitation.ConInviter)
            {
                if (!partyData.AddMember(Connection.ActiveCharacter))
                    Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Party's full.")));

                return;
            }

            new Party().CreateNewParty(owner, Connection.ActiveCharacter);
        }
    }
}