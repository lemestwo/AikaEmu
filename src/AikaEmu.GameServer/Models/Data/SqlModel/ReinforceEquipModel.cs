namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class ReinforceEquipModel
    {
        public ushort Id { get; set; }
        public ushort ReagentQty { get; set; }
        public uint Price { get; set; }
        public ushort[] Chance { get; set; }
    }
}