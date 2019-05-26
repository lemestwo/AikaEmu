using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class BuyFromNpcStore : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var npcConId = stream.ReadUInt16();
            stream.ReadUInt16();
            var itemIndex = stream.ReadInt32();
            var quantity = stream.ReadUInt32();

            NpcDialogHelper.BuyFromShop(Connection.ActiveCharacter, npcConId, itemIndex, quantity);
        }
    }
}