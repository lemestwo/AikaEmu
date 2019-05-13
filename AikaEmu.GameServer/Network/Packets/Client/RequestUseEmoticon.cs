using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestUseEmoticon : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var emoticonId = stream.ReadUInt32();
            // stream.ReadUInt32();

            // TODO - VERIFICATIONS ABOUT EMOTICONS
            Connection.SendPacket(new UseEmoticon(Connection.Id, emoticonId));
        }
    }
}