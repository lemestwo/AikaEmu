using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class NpcStoreOpen : GamePacket
    {
        private readonly ushort _npcConId;
        private readonly StoreType _storeType;
        private readonly ushort[] _items;

        public NpcStoreOpen(ushort npcConId, StoreType storeType, ushort[] items)
        {
            _npcConId = npcConId;
            _storeType = storeType;
            _items = items;

            Opcode = (ushort) GameOpcode.NpcStoreOpen;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_npcConId);
            stream.Write((ushort) _storeType);
            for (var i = 0; i < 40; i++)
                stream.Write(_items[i]);

            return stream;
        }
    }
}