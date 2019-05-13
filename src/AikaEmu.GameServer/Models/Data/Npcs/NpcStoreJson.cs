namespace AikaEmu.GameServer.Models.Data.Npcs
{
    public class NpcStoreJson
    {
        public ushort NpcId { get; set; }
        public ushort StoreType { get; set; }
        public ushort[] Items { get; set; }
    }
}