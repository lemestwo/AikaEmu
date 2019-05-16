using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Group;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;

namespace AikaEmu.GameServer.Helpers
{
    public static class GroupHelper
    {
        public static void PartyRequest(GameConnection connection, ushort targetConId)
        {
            var target = WorldManager.Instance.GetCharacter(targetConId);
            if (target == null)
            {
                connection.SendPacket(new SendMessage(new Message("Character doesn't exist or isn't online.")));
                return;
            }

            if (PartyManager.Instance.GetInvite(target.Connection.Id) != null)
            {
                connection.SendPacket(new SendMessage(new Message("This character can't receive invitations right now.")));
                return;
            }

            if (PartyManager.Instance.AddInvitation(connection.Id, target.Connection.Id))
            {
                target.SendPacket(new SendPartyInvite(target.Connection.Id, connection.Id, connection.ActiveCharacter.Name));
            }
        }

        public static void PartyRequestResult(GameConnection connection, bool result)
        {
            var invitation = PartyManager.Instance.GetInvite(connection.Id);
            if (invitation == null)
            {
                connection.SendPacket(new SendMessage(new Message("Party not available anymore.")));
                return;
            }

            PartyManager.Instance.RemoveInvite(connection.Id);

            var isPartyAlready = PartyManager.Instance.GetParty(connection.Id);
            if (isPartyAlready != null)
            {
                connection.SendPacket(new SendMessage(new Message("Already in a party.")));
                return;
            }

            var owner = WorldManager.Instance.GetCharacter(invitation.ConInviter);
            if (owner == null)
            {
                connection.SendPacket(new SendMessage(new Message("Party not available right now.")));
                return;
            }

            if (!result) return;

            var partyData = PartyManager.Instance.GetParty(invitation.ConInviter);
            if (partyData != null && partyData.LeaderConId == invitation.ConInviter)
            {
                if (!partyData.AddMember(connection.ActiveCharacter))
                    connection.SendPacket(new SendMessage(new Message("Party's full.")));

                return;
            }

            new Party().CreateNewParty(owner, connection.ActiveCharacter);
        }

        public static void PartyRemoveMember(GameConnection connection, ushort targetConId)
        {
            var partyData = PartyManager.Instance.GetParty(targetConId);
            if (partyData == null) return;

            // Leave
            if (targetConId == connection.Id)
            {
                partyData.RemoveMember(targetConId);
                connection.SendPacket(new SendPartyInfo(null));
                partyData.SendPacketAll(new SendMessage(new Message($"[{connection.ActiveCharacter.Name}] left the party.")));
                connection.SendPacket(new SendMessage(new Message("You left the party.")));
                return;
            }

            // Kick
            if (partyData.LeaderConId != connection.Id)
            {
                connection.SendPacket(new SendMessage(new Message("You are not the party leader.")));
                return;
            }

            partyData.RemoveMember(targetConId);
        }

        public static void PartyDisband(GameConnection connection, ushort conId)
        {
            var myConId = connection.Id;
            // TODO - Maybe change for raid
            if (conId != myConId) return;

            var partyData = PartyManager.Instance.GetParty(myConId);
            if (partyData == null || partyData.LeaderConId != myConId)
            {
                connection.SendPacket(new SendMessage(new Message("You are not the party leader.")));
                return;
            }

            partyData.SendPacketAll(new SendPartyInfo(null));
            PartyManager.Instance.RemoveParty(partyData.Id);
        }

        public static void PartyChangeLeader(GameConnection connection, ushort conId)
        {
            var partyData = PartyManager.Instance.GetParty(connection.Id);
            if (!partyData.IsMember(conId)) return;
            
            partyData.LeaderConId = conId;
            partyData.UpdateParty();
        }
    }
}