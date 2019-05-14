using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Group;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class InviteToParty : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var conId = stream.ReadUInt16();
            stream.ReadInt16();
            // stream.ReadBytes(16);

            var target = WorldManager.Instance.GetCharacter(conId);
            if (target == null)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Character doesn't exist or isn't online.")));
                return;
            }

            if (PartyManager.Instance.GetInvite(target.Connection.Id) != null)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal,
                    "This character can't receive invitations right now.")));
                return;
            }

            if (PartyManager.Instance.AddInvitation(Connection.Id, target.Connection.Id))
            {
                target.SendPacket(new SendPartyInvite(target.Connection.Id, Connection.Id, Connection.ActiveCharacter.Name));
            }
        }
    }
}