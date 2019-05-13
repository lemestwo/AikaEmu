using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestCreateGuild : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            stream.ReadBytes(12);
            var guildName = stream.ReadString(18);
            stream.ReadBytes(6);
            Connection.SendPacket(new CreateGuildBox(Connection.Id, 1));
        }
    }
}