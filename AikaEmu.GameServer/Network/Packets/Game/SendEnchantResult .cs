using System;
using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendEnchantResult : GamePacket
    {
        private readonly ActionType _actionType;
        private readonly ActionResult _actionResult;

        public SendEnchantResult(ushort conId, ActionType actionType, ActionResult actionResult)
        {
            _actionType = actionType;
            _actionResult = actionResult;

            Opcode = (ushort) GameOpcode.SendEnchantResult;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // Dynamic packet size, can have multiple sizes
            // 20, 24, 28,
            stream.Write((int) _actionType);
            switch (_actionType)
            {
                case ActionType.Refinement:
                    stream.Write((int) _actionResult);
                    if (_actionResult == ActionResult.MajorFailure)
                        stream.Write(0); // itemSlot to remove
                    break;
                case ActionType.Enchant:
                    stream.Write((int) ActionResult.Success);
                    break;
                default:
                    Log.Warn("SendEnchantResult: Out of range: {0}", _actionType);
                    break;
            }

            return stream;
        }
    }
}