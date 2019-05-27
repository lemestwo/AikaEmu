using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Model;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Friends : ISaveData
    {
        private readonly Character _character;
        private readonly Dictionary<uint, Friend> _friends;
        private readonly List<uint> _removedFriends;

        public Friends(Character character)
        {
            _character = character;
            _friends = new Dictionary<uint, Friend>();
            _removedFriends = new List<uint>();
        }

        public Friend GetFriendByDbId(uint dbId)
        {
            foreach (var friend in _friends.Values)
            {
                if (friend.FriendId == dbId) return friend;
            }

            return null;
        }

        public Friend GetFriend(uint id)
        {
            return _friends.ContainsKey(id) ? _friends[id] : null;
        }


        public void SendFriends()
        {
            foreach (var friend in _friends.Values)
            {
                _character.SendPacket(new SendFriendInfo(_character.Connection.Id, friend));

                // Send online notification
                var friendData = WorldManager.Instance.GetCharacter(friend.FriendId);
                friendData?.SendPacket(new SendFriendOn(friendData.Connection.Id, _character));
            }
        }

        public void GetOffline()
        {
            foreach (var friend in _friends.Values)
            {
                var friendData = WorldManager.Instance.GetCharacter(friend.FriendId);
                friendData?.SendPacket(new SendFriendOff(friendData.Connection.Id, _character.DbId));
            }
        }

        public void AddFriend(Friend friend)
        {
            if (GetFriendByDbId(friend.Id) != null) return;

            friend.Id = DatabaseManager.Instance.InsertFriend(_character, friend);

            if (friend.Id > 0 && _friends.TryAdd(friend.Id, friend))
            {
                _character.SendPacket(new SendFriendInfo(_character.Connection.Id, friend));
            }
        }

        public void RemoveFriend(uint id)
        {
            if (!_friends.ContainsKey(id)) return;

            _friends.Remove(id);
            _removedFriends.Add(id);
        }

        public void Init(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM character_friends WHERE char_id=@char_id";
                command.Parameters.AddWithValue("@char_id", _character.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new Friend
                        {
                            Id = reader.GetUInt32("id"),
                            FriendId = reader.GetUInt32("friend_id"),
                            Name = reader.GetString("name"),
                            IsBlocked = reader.GetBoolean("is_blocked")
                        };
                        _friends.Add(template.Id, template);
                    }
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            if (_removedFriends.Count > 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM character_friends WHERE id IN(" + string.Join(",", _removedFriends) + ")";
                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                _removedFriends.Clear();
            }

            foreach (var friend in _friends.Values)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"id", friend.Id},
                    {"char_id", _character.DbId},
                    {"friend_id", friend.FriendId},
                    {"name", friend.Name},
                    {"is_blocked", friend.IsBlocked}
                };
                DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "character_friends", parameters, connection, transaction);
            }
        }
    }
}