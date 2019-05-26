using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class OpenNpcChat : GamePacket
    {
        private readonly uint _npcId;

        public OpenNpcChat(uint npcId)
        {
            _npcId = npcId;
            Opcode = (ushort) GameOpcode.OpenNpcChat;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_npcId);
            return stream;
        }
    }
}