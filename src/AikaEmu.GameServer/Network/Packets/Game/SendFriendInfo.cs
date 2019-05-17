using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendFriendInfo : GamePacket
    {
        private readonly Friend _friend;

        public SendFriendInfo(ushort conId, Friend friend)
        {
            _friend = friend;
            Opcode = (ushort) GameOpcode.SendFriendInfo;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            var friend = WorldManager.Instance.GetCharacter(_friend.FriendId);
            if (friend == null)
            {
                stream.Write(_friend.Name, 16);
                stream.Write(_friend.FriendId);
                stream.Write((byte) (_friend.IsBlocked ? FriendStatus.OfflineBlocked : FriendStatus.Offline));
                stream.Write("", 7);
            }
            else
            {
                stream.Write(friend.Name, 16);
                stream.Write(_friend.FriendId);
                stream.Write((byte) (_friend.IsBlocked ? FriendStatus.OnlineBlocked : FriendStatus.Online));
                stream.Write((byte) 0); // TODO - Server channel
                stream.Write(friend.Connection.Id); // unk - conId?
                stream.Write((byte) friend.Profession);
                stream.Write((byte) 59); // TODO - Map
                stream.Write((byte) friend.Level);
                stream.Write((byte) friend.Account.NationId);
            }

            return stream;
        }
    }
}