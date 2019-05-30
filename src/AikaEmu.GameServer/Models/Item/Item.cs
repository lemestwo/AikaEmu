using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Item
{
    public class Item : BasePacket
    {
        public uint DbId { get; set; } = 0;
        public ushort ItemId { get; set; } = 0;
        public uint AccId { get; set; }
        public uint CharId { get; set; }
        public uint PranId { get; set; }
        public SlotType SlotType { get; set; }
        public ushort Slot { get; set; }
        public byte Effect1 { get; set; } = 0;
        public byte Effect2 { get; set; } = 0;
        public byte Effect3 { get; set; } = 0;
        public byte Effect1Value { get; set; } = 0;
        public byte Effect2Value { get; set; } = 0;
        public byte Effect3Value { get; set; } = 0;
        public byte Durability { get; set; } = 0;
        public byte DurMax { get; set; } = 0;
        public byte Quantity { get; set; } = 0;
        public byte DisableDurplus { get; set; } = 0;
        public ushort ItemTime { get; set; } = 0;

        public ItemsJson ItemData { get; }

        public Item(ushort itemId)
        {
            ItemId = itemId;
            ItemData = DataManager.Instance.ItemsData.GetData(ItemId);
        }

        public Item(SlotType slotType, ushort slot, ushort itemId, bool loadData = true)
        {
            SlotType = slotType;
            Slot = slot;
            ItemId = itemId;
            if (loadData)
                ItemData = DataManager.Instance.ItemsData.GetData(ItemId);
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(ItemId);
            stream.Write(ItemId);
            stream.Write(DbId);
            stream.Write(Effect1);
            stream.Write(Effect2);
            stream.Write(Effect3);
            stream.Write(Effect1Value);
            stream.Write(Effect2Value);
            stream.Write(Effect3Value);
            stream.Write(Durability);
            stream.Write(DurMax);
            stream.Write(Quantity);
            stream.Write(DisableDurplus);
            stream.Write(ItemTime);
            return stream;
        }
    }
}