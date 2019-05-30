using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Data.JsonModel;

namespace AikaEmu.GameServer.Models.World.Devir
{
    public class DevirSlot
    {
        public byte Slot { get; set; }
        public bool IsActive { get; set; }
        public ushort ItemId { get; }
        public ItemsJson ItemData { get; }
        public string Name { get; set; }
        public uint Time { get; set; }

        public DevirSlot(ushort itemId)
        {
            ItemId = itemId;
            ItemData = DataManager.Instance.ItemsData.GetData(itemId);
        }
    }
}