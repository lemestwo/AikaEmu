using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendFriendOn : GamePacket
    {
        private readonly Character _friend;

        public SendFriendOn(ushort conId, Character friend)
        {
            _friend = friend;

            Opcode = (ushort) GameOpcode.SendFriendOn;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_friend.DbId); // dbId
            stream.Write(_friend.Connection.Id);
            stream.Write((byte) FriendStatus.Online);
            var a = 0;
            var b = 0;
            stream.Write((byte) a); // unk
            stream.Write((byte) b); // unk
            stream.Write((byte) _friend.Level);
            stream.Write((byte) _friend.Account.NationId);
            stream.Write((byte) _friend.Profession);
            return stream;
        }
    }
}