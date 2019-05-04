using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestSplitItem : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var slot = stream.ReadUInt32();
            var quantity = stream.ReadUInt32();
            var slotType = (SlotType) stream.ReadInt32();

            Connection.ActiveCharacter.Inventory.SplitItem(slotType, (ushort) slot, quantity);
        }
    }
}