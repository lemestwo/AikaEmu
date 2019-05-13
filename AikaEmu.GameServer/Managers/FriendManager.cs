using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class FriendManager : Singleton<FriendManager>
    {
        private readonly List<Friend> _friendsRequest;

        public FriendManager()
        {
            _friendsRequest = new List<Friend>();
        }

        public Friend GetRequest(uint charId, uint targetId)
        {
            foreach (var friend in _friendsRequest)
            {
                // TODO - Maybe change this for performance issues
                if (friend.AddTime <= DateTime.UtcNow) RemoveRequest(friend);
                else if (friend.CharacterId == charId && friend.FriendId == targetId) return friend;
            }

            return null;
        }

        public void AddRequest(uint charId, uint targetId, string name)
        {
            if (GetRequest(charId, targetId) != null) return;

            var temp = new Friend
            {
                CharacterId = charId,
                FriendId = targetId,
                Name = name,
                IsBlocked = false,
                AddTime = DateTime.UtcNow.AddMinutes(2),
            };
            _friendsRequest.Add(temp);
        }

        public void RemoveRequest(Friend friend)
        {
            if (_friendsRequest.Contains(friend)) _friendsRequest.Remove(friend);
        }
    }
}