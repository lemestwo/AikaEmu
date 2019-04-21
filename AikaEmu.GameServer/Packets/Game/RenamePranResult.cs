using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class RenamePranResult : GamePacket
    {
        public RenamePranResult(ushort conId)
        {
            Opcode = (ushort) GameOpcode.RenamePranResult;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // TODO
            stream.Write(0); // pran slot
            stream.Write("", 16); // pran name
            stream.Write(0); // accId
            return stream;
        }
    }
}
