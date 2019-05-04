namespace AikaEmu.GameServer.Models.Units.Mob
{
    public class Mob : BaseUnit
    {
        public uint MobId { get; set; }
        public uint Model { get; set; }

        public int Unk7 { get; set; }

        public uint Hp1 { get; set; }
        public uint Hp2 { get; set; }
        public uint Hp3 { get; set; }

        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public byte Unk4 { get; set; }
        public byte Unk5 { get; set; }
        public uint Unk6 { get; set; }
    }
}