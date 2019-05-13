using System.Collections.Generic;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Attributes : BasePacket
    {
        private readonly ushort _baseStr;
        private readonly ushort _baseAgi;
        private readonly ushort _baseInt;
        private readonly ushort _baseCon;
        private readonly ushort _baseSpi;

        public ushort Strenght
        {
            get => _baseStr;
        }

        public ushort Agility
        {
            get => _baseAgi;
        }

        public ushort Intelligence
        {
            get => _baseInt;
        }

        public ushort Constitution
        {
            get => _baseCon;
        }

        public ushort Spirit
        {
            get => _baseSpi;
        }

        public Attributes(IReadOnlyList<ushort> attr)
        {
            _baseStr = attr[0];
            _baseAgi = attr[1];
            _baseInt = attr[2];
            _baseCon = attr[3];
            _baseSpi = attr[4];
        }

        public Attributes(ushort str, ushort agi, ushort inte, ushort con, ushort spi)
        {
            _baseStr = str;
            _baseAgi = agi;
            _baseInt = inte;
            _baseCon = con;
            _baseSpi = spi;
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