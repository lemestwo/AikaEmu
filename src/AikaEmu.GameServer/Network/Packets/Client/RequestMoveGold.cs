using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestMoveGold : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var slotTypeDest = (SlotType) stream.ReadUInt32();
            var amount = stream.ReadInt64();

            Connection.ActiveCharacter.Inventory.MoveMoney(slotTypeDest, amount);
        }
    }
}