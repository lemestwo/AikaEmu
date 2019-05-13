using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestFriendResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadUInt16() == 1;
            var id = stream.ReadUInt16();
            var name = stream.ReadString(16);

            var owner = WorldManager.Instance.GetCharacter(id);
            if (owner == null) return;

            var request = FriendManager.Instance.GetRequest(owner.Id, Connection.ActiveCharacter.Id);
            if (request == null)
            {
                Connection.SendPacket(
                    new SendMessage(new Message(MessageSender.System, MessageType.Normal, "Friend request has expired.")));
                return;
            }

            if (result)
            {
                owner.Friends.AddFriend(request);
                var requestTarget = (Friend) request.Clone();
                requestTarget.SwapIds(owner.Name);
                Connection.ActiveCharacter.Friends.AddFriend(requestTarget);
            }

            FriendManager.Instance.RemoveRequest(request);
        }
    }
}