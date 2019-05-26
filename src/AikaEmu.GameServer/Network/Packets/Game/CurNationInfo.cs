using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class CurNationInfo : GamePacket
    {
        private readonly Nation _nation;

        public CurNationInfo(Nation nation)
        {
            _nation = nation;
            
            Opcode = (ushort) GameOpcode.CurNationInfo;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((byte) 0); // rank - always 0?
            stream.Write(_nation.TaxCitizen); // citizen tax
            stream.Write(_nation.TaxVisitor); // visitor tax
            stream.Write((byte) 5); // unk
            stream.Write(_nation.Settlement);
            stream.Write((byte) 1); // unk(byte)
            stream.Write("", 7); //empty fill
            return stream;
        }
    }
}