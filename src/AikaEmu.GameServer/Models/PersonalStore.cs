using System.Collections.Generic;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models
{
    public class PersonalStore : BasePacket
    {
        private Character Owner { get; set; }
        public string Title { get; set; }
        public Dictionary<byte, (ulong price, Item.Item item)> Items { get; set; }

        public PersonalStore(PacketStream stream, Character owner)
        {
            Owner = owner;
            Title = stream.ReadString(32);
            Items = new Dictionary<byte, (ulong price, Item.Item item)>();

            for (byte i = 0; i < 10; i++)
            {
                var price = stream.ReadUInt64();
                var slot = stream.ReadUInt16();
                stream.ReadUInt16();
                stream.ReadBytes(20);

                if (slot == ushort.MaxValue) continue;
                var item = owner.Inventory.GetItem(SlotType.Inventory, slot);
                // TODO - CHECK UNTRADABLE
                if (item == null) continue;

                Items.Add(i, (price, item));
            }
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) Owner.Connection.Id);
            stream.Write(Title, 32);

            for (byte i = 0; i < 10; i++)
            {
                if (Items.ContainsKey(i))
                {
                    var (price, item) = Items[i];
                    stream.Write(price);
                    stream.Write((uint) item.Slot);
                    stream.Write(item);
                }
                else
                {
                    stream.Write("", 20);
                    stream.Write(ushort.MaxValue);
                    stream.Write("", 10);
                }
            }

            return stream;
        }
    }
}