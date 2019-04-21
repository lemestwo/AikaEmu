using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class ApplyBuff : GamePacket
    {
        public ApplyBuff(ushort conId)
        {
            Opcode = (ushort) GameOpcode.ApplyBuff;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((ushort) 0); // id
            stream.Write((ushort) 0);
            stream.Write(0); // unk
            stream.Write(0);
            return stream;
        }
    }
}