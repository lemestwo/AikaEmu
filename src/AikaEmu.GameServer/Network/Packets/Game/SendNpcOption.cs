using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendNpcOption : GamePacket
    {
        private readonly NpcDialog _dialog;

        public SendNpcOption(NpcDialog dialog)
        {
            _dialog = dialog;
            Opcode = (ushort) GameOpcode.SendNpcOption;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_dialog);
            return stream;
        }
    }
}