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
            if (target.Friends.GetFriendByDbId(character.DbId) != null || character.Friends.GetFriendByDbId(target.DbId) != null || target.DbId == character.DbId)
            {
                connection.SendPacket(new SendMessage(new Message("Already friends.")));
                return;
            }

            FriendManager.Instance.AddRequest(character.DbId, target.DbId, target.Name);
            target.SendPacket(new SendRequestFriend(character));
        }

        public static void FriendRequestResult(GameConnection connection, bool result, ushort conId)
        {
            var owner = WorldManager.Instance.GetCharacter(conId);
            if (owner == null) return;

            var request = FriendManager.Instance.GetRequest(owner.DbId, connection.ActiveCharacter.DbId);
            if (request == null)
            {
                connection.SendPacket(new SendMessage(new Message("Friend request has expired.")));
                return;
            }

            if (result)
            {
                owner.Friends.AddFriend(request);
                owner.SendPacket(new SendMessage(new Message($"[{connection.ActiveCharacter.Name}] is now your friend.")));
                var requestTarget = (Friend) request.Clone();
                requestTarget.SwapIds(owner.Name);
                connection.ActiveCharacter.Friends.AddFriend(requestTarget);
                connection.SendPacket(new SendMessage(new Message($"[{owner.Name}] is now your friend.")));
            }

            FriendManager.Instance.RemoveRequest(request);
        }

        public static void FriendRequestChat(GameConnection connection, uint friendId)
        {
            var friendData = connection.ActiveCharacter.Friends.GetFriendByDbId(friendId);
            if (friendData == null) return;

            if (WorldManager.Instance.GetCharacter(friendData.FriendId) != null)
                connection.SendPacket(new StartFriendChat(connection.Id, friendId));
            else
                connection.SendPacket(new SendMessage(new Message("Friend is offline.")));
        }

        public static void FriendRequestRemove(GameConnection connection, uint friendId)
        {
            var character = connection.ActiveCharacter;
            var friendData = character.Friends.GetFriendByDbId(friendId);
            if (friendData == null) return;

            var friendChar = WorldManager.Instance.GetCharacter(friendData.FriendId);
            if (friendChar == null)
            {
                DatabaseManager.Instance.RemoveOfflineFriend(friendData);
            }
            else
            {
                var ownerFriendData = friendChar.Friends.GetFriendByDbId(character.DbId);
                if (ownerFriendData != null)
                {
                    friendChar.Friends.RemoveFriend(ownerFriendData.Id);
                    friendChar.SendPacket(new RemoveFriend(friendChar.Connection.Id, character.DbId));
                    friendChar.Save(SaveType.Friends);
                }
            }

            character.Friends.RemoveFriend(friendId);
            character.SendPacket(new RemoveFriend(connection.Id, friendId));
            character.Save(SaveType.Friends);
        }
    }
}