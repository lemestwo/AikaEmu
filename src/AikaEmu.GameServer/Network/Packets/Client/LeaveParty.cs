using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class LeaveParty : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var id = stream.ReadUInt16();

            var partyData = PartyManager.Instance.GetParty(id);
            if (partyData == null) return;

            // Leave
            if (id == Connection.Id)
            {
                partyData.RemoveMember(id);
                Connection.SendPacket(new SendPartyInfo(null));
                partyData.SendPacketAll(new SendMessage(new Message(MessageSender.System, MessageType.Normal,
                    $"[{Connection.ActiveCharacter.Name}] left the party.")));
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "You left the party.")));
                return;
            }

            // Kick
            if (partyData.LeaderConId != Connection.Id)
            {
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "You are not the party leader.")));
                return;
            }

            partyData.RemoveMember(id);
        }
    }
}