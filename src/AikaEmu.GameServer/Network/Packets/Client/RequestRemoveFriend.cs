using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestRemoveFriend : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var id = stream.ReadUInt32();

            var character = Connection.ActiveCharacter;
            var friendData = character.Friends.GetFriend(id);
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

            character.Friends.RemoveFriend(id);
            character.SendPacket(new RemoveFriend(Connection.Id, id));
            character.Save(SaveType.Friends);
        }
    }
}