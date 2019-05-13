using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestAddFriend : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            stream.ReadUInt32();
            var name = stream.ReadString(16).Trim();

            if (string.IsNullOrEmpty(name)) return;

            var target = WorldManager.Instance.GetCharacter(name);
            if (target == null)
            {
                Connection.SendPacket(
                    new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Selected character doesn't exist or isn't online.")));
                return;
            }

            var character = Connection.ActiveCharacter;
            if (target.Friends.GetFriendByDbId(character.Id) != null || character.Friends.GetFriendByDbId(target.Id) != null || target.Id == character.Id)
            {
                Connection.SendPacket(
                    new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Already friends.")));
                return;
            }

            FriendManager.Instance.AddRequest(character.Id, target.Id, target.Name);
            target.SendPacket(new SendRequestFriend(character));
        }
    }
}