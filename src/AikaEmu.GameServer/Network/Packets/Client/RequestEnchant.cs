using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestEnchant : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var actionType = (ActionType) stream.ReadInt32();
            var slotEquip = (byte) (stream.ReadInt32() & 0xFF);
            var slotReagent = (byte) (stream.ReadInt32() & 0xFF);
            var slotCash = (byte) (stream.ReadInt32() & 0xFF);

            switch (actionType)
            {
                case ActionType.Reinforcement:
                    EnchantHelper.Reinforcement(Connection, slotEquip, slotReagent, slotCash);
                    break;
                case ActionType.Enchant:
                    break;
                case ActionType.LevelDown:
                    break;
                case ActionType.Craft:
                    break;
                case ActionType.Dismantle:
                    break;
                case ActionType.PranCostumeEnchant:
                    break;
                case ActionType.StoneRefinement:
                    break;
                case ActionType.StoneEnchant:
                    break;
            }
            
            Log.Warn("ActionType: {0}", actionType);
        }
    }
}