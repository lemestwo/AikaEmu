namespace AikaEmu.GameServer.Models.Char.Inventory
{
    public enum SlotType : byte
    {
        Equipments = 0,
        Inventory = 1,
        Bank = 2,
        
        PranEquipments = 5,
        PranInventory = 6,
        
        Unk7 = 7,
        Unk10 = 10, // 0xA
    }
}