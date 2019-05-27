using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestEventItem : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            // var unk = stream.ReadUInt32();
            Connection.SendPacket(new EventItemDone(Connection.Id));
            Connection.SendPacket(new SendMessage(new Message("No more event items to receive.")));
        }
    }
}