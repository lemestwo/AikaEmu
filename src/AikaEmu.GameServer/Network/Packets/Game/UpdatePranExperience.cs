using AikaEmu.GameServer.Models.Units.Pran;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdatePranExperience : GamePacket
    {
        private readonly Pran _pran;

        public UpdatePranExperience(Pran pran)
        {
            _pran = pran;
            Opcode = (ushort) GameOpcode.UpdatePranExperience;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_pran.Level);
            stream.Write((ushort) 0); // empty fill
            stream.Write(_pran.Experience);
            stream.Write("", 20); // empty fill?
            return stream;
        }
    }
}
