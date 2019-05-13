using System.Collections.Concurrent;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateBank : GamePacket
    {
        private readonly ulong _gold;
        private readonly ConcurrentDictionary<ushort, Item> _items;
        private readonly int _unk;

        public UpdateBank(ulong gold, ConcurrentDictionary<ushort, Item> items, int unk)
        {
            _gold = gold;
            _items = items;
            _unk = unk;

            Opcode = (ushort) GameOpcode.UpdateBank;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_unk);
            stream.Write(_gold);
            for (ushort i = 0; i < 86; i++)
            {
                if (_items.ContainsKey(i))
                    stream.Write(_items[i]);
                else
                    stream.Write("", 20);
            }

            return stream;
        }
    }
}