namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class GearCoreDataModel
    {
        public ushort Id { get; set; }
        public ushort Idx { get; set; }
        public ushort Unk { get; set; }
        public ushort ItemId { get; set; }
        public ushort ResultItemId { get; set; }
        public byte Chance { get; set; }
        public byte ExtChance { get; set; }
        public byte ConcExtChance { get; set; }
    }
}