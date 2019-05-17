using System.Collections.Generic;

namespace AikaEmu.GameServer.Models
{
    public class Trade
    {
        public uint Id { get; set; }
        public ushort OwnerConId { get; set; }
        public ushort TargetConId { get; set; }
        public bool OwnerOk { get; set; }
        public bool TargetOk { get; set; }
        public bool OwnerConfirm { get; set; }
        public bool TargetConfirm { get; set; }

        public ulong OwnerMoney { get; set; }
        public ulong TargetMoney { get; set; }
        public Dictionary<byte, ushort> OwnerSlots { get; set; }
        public Dictionary<byte, ushort> TargetSlots { get; set; }
        public Dictionary<byte, Item.Item> OwnerItems { get; set; }
        public Dictionary<byte, Item.Item> TargetItems { get; set; }

        public Trade()
        {
            OwnerOk = false;
            TargetOk = false;
            OwnerConfirm = false;
            TargetConfirm = false;
            OwnerItems = new Dictionary<byte, Item.Item>();
            TargetItems = new Dictionary<byte, Item.Item>();
            OwnerSlots = new Dictionary<byte, ushort>();
            TargetSlots = new Dictionary<byte, ushort>();
        }
    }
}