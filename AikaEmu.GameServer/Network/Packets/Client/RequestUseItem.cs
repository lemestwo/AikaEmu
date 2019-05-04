using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestUseItem : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var slotType = (SlotType) stream.ReadUInt32();
            var slot = (ushort) stream.ReadUInt32();
            var data = stream.ReadInt32();

            Connection.ActiveCharacter?.Inventory?.UseItem(slotType, slot, data);
        }
    }
}