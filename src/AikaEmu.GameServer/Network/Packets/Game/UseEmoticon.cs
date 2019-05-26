using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UseEmoticon : GamePacket
    {
        private readonly uint _emoticonId;

        public UseEmoticon(ushort conId, uint emoticonId)
        {
            _emoticonId = emoticonId;
            
            Opcode = (ushort) GameOpcode.UseEmoticon;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_emoticonId);
            stream.Write(0);
            return stream;
        }
    }
}