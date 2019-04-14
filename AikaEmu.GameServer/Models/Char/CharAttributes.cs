using System.Collections.Generic;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Char
{
    public class CharAttributes : BasePacket
    {
        public ushort Strenght { get; set; }
        public ushort Agility { get; set; }
        public ushort Intelligence { get; set; }
        public ushort Constitution { get; set; }
        public ushort Spirit { get; set; }

        public CharAttributes(IReadOnlyList<ushort> attr)
        {
            Strenght = attr[0];
            Agility = attr[1];
            Intelligence = attr[2];
            Constitution = attr[3];
            Spirit = attr[4];
        }

        public CharAttributes()
        {
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(Strenght);
            stream.Write(Agility);
            stream.Write(Intelligence);
            stream.Write(Constitution);
            stream.Write(Spirit);
            return stream;
        }
    }
}