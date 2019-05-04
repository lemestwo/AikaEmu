using AikaEmu.GameServer.Controller;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class BuyFromNpcStore : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var npcConId = stream.ReadUInt32();
            var itemIndex = stream.ReadInt32();
            var quantity = stream.ReadUInt32();

            NpcInteractionController.BuyFromShop(Connection.ActiveCharacter, npcConId, itemIndex, quantity);
        }
    }
}