using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestDeleteItem : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var slot = stream.ReadInt32();
            var slotType = (SlotType) stream.ReadInt32();
            
            Connection.ActiveCharacter.Inventory.RemoveItem(slotType, (ushort) slot);
        }
    }
}