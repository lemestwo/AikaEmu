using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;

namespace AikaEmu.GameServer.Helpers
{
    public static class FriendHelper
    {
        public static void FriendRequest(GameConnection connection, string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            var target = WorldManager.Instance.GetCharacter(name);
            if (target == null)
            {
                connection.SendPacket(new SendMessage(new Message("Selected character doesn't exist or isn't online.")));
                return;
            }

            var character = connection.ActiveCharacter;
            if (target.Friends.GetFriendByDbId(character.Id) != null || character.Friends.GetFriendByDbId(target.Id) != null || target.Id == character.Id)
            {
                connection.SendPacket(new SendMessage(new Message("Already friends.")));
                return;
            }

            FriendManager.Instance.AddRequest(character.Id, target.Id, target.Name);
            target.SendPacket(new SendRequestFriend(character));
        }

        public static void FriendRequestResult(GameConnection connection, bool result, ushort conId)
        {
            var owner = WorldManager.Instance.GetCharacter(conId);
            if (owner == null) return;

            var request = FriendManager.Instance.GetRequest(owner.Id, connection.ActiveCharacter.Id);
            if (request == null)
            {
                connection.SendPacket(new SendMessage(new Message("Friend request has expired.")));
                return;
            }

            if (result)
            {
                owner.Friends.AddFriend(request);
                var requestTarget = (Friend) request.Clone();
                requestTarget.SwapIds(owner.Name);
                connection.ActiveCharacter.Friends.AddFriend(requestTarget);
            }

            FriendManager.Instance.RemoveRequest(request);
        }

        public static void FriendRequestChat(GameConnection connection, uint friendId)
        {
            var friendData = connection.ActiveCharacter.Friends.GetFriend(friendId);
            if (friendData == null) return;
            
            if (WorldManager.Instance.GetCharacter(friendData.FriendId) != null)
                connection.SendPacket(new StartFriendChat(connection.Id, friendId));
            else
                connection.SendPacket(new SendMessage(new Message("Friend is offline.")));
        }

        public static void FriendRequestRemove(GameConnection connection, uint friendId)
        {
            var character = connection.ActiveCharacter;
            var friendData = character.Friends.GetFriend(friendId);
            if (friendData == null) return;

            var friendChar = WorldManager.Instance.GetCharacter(friendData.FriendId);
            if (friendChar == null)
            {
                DatabaseManager.Instance.RemoveOfflineFriend(friendData);
            }
            else
            {
                var ownerFriendData = friendChar.Friends.GetFriendByDbId(character.Id);
                if (ownerFriendData != null)
                {
                    friendChar.Friends.RemoveFriend(ownerFriendData.Id);
                    friendChar.SendPacket(new RemoveFriend(friendChar.Connection.Id, ownerFriendData.Id));
                    friendChar.Save(SaveType.Friends);
                }
            }

            character.Friends.RemoveFriend(friendId);
            character.SendPacket(new RemoveFriend(connection.Id, friendId));
            character.Save(SaveType.Friends);
        }
    }
}