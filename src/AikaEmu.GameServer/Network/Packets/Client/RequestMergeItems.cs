using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestMergeItems : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var slotFrom = (ushort) stream.ReadUInt32();
            var slotTo = (ushort) stream.ReadUInt32();

            Connection.ActiveCharacter.Inventory.MergeItems(slotFrom, slotTo);
        }
    }
}