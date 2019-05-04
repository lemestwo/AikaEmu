using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestEnchant : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var actionType = (ActionType) stream.ReadInt32();
            var slotEquip = stream.ReadInt32();
            var slotReagent = stream.ReadInt32();
            var slotCash = stream.ReadInt32();
            Connection.SendPacket(new SendEnchantResult(Connection.Id, actionType, ActionResult.Success));
        }
    }
}