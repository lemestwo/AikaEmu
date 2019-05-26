using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class NpcStartTalk : GamePacket
    {
        private readonly int _talkId;

        public NpcStartTalk(int talkId)
        {
            _talkId = talkId;
            Opcode = (ushort) GameOpcode.NpcStartTalk;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_talkId);
            return stream;
        }
    }
}