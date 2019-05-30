using System;
using AikaEmu.GameServer.Helpers;
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
                default:
                    Log.Warn("ActionType out of range: {0}", actionType);
                    break;
            }
        }
    }
}