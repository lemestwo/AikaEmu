using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestFriendChat : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var friendId = stream.ReadUInt32();
            stream.ReadUInt32();

            var friendData = Connection.ActiveCharacter.Friends.GetFriend(friendId);
            if (friendData == null) return;
            
            if (WorldManager.Instance.GetCharacter(friendData.FriendId) != null)
                Connection.SendPacket(new StartFriendChat(Connection.Id, friendId));
            else
                Connection.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Friend is offline.")));
        }
    }
}