using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendEnchantResult : GamePacket
    {
        private readonly ActionType _actionType;
        private readonly object _arg1;
        private readonly object _arg2;
        private readonly object _arg3;
        private readonly object _arg4;

        public SendEnchantResult(ushort conId, ActionType actionType, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null)
        {
            _actionType = actionType;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;

            Opcode = (ushort) GameOpcode.SendEnchantResult;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // Dynamic packet size
            // 20, 24, 28,
            stream.Write((uint) _actionType);
            if (_arg1 != null) stream.Write((uint) _arg1);
            if (_arg2 != null) stream.Write((uint) _arg2);
            if (_arg3 != null) stream.Write((uint) _arg3);
            if (_arg4 != null) stream.Write((uint) _arg4);
            return stream;
        }
    }
}