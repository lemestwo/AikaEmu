using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class MoveItem : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var typeTo = (SlotType) stream.ReadUInt16();
            var slotTo = stream.ReadUInt16();
            var typeFrom = (SlotType) stream.ReadUInt16();
            var slotFrom = stream.ReadUInt16();

            Log.Debug("MoveItem, From: {0}/{1} -> {2}/{3}", typeFrom, slotFrom, typeTo, slotTo);

            Connection.ActiveCharacter.Inventory.SwapItems(typeFrom, typeTo, slotFrom, slotTo);
        }
    }
}