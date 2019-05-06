using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdatePosition : GamePacket
    {
        private readonly BaseUnit _unit;
        private readonly byte _state;
        private readonly byte _speed;

        public UpdatePosition(BaseUnit unit, byte state, byte speed = 50)
        {
            _unit = unit;
            _state = state;
            _speed = speed;

            Opcode = (ushort) GameOpcode.UpdatePosition;
            SetSenderIdWithUnit(_unit);
        }

        public override PacketStream Write(PacketStream stream)
        {
            // INFO - MOSTLY DATA IS FOR MOVE OTHERS THAN PLAYER ITSELF
            stream.Write(_unit.Position.CoordX);
            stream.Write(_unit.Position.CoordY);
            stream.Write(0); // unk
            stream.Write((ushort) 45); // TODO - rotation
            stream.Write(_state); // tp = 1 / move = 0 / 4 = fly? / if (& 2 > 0) unique behavior
            stream.Write(_speed); // TODO - Movement Speed (0 for TP)

            stream.Write((byte) 0); // if > 0 replace state in client
            stream.Write((byte) 0); // data +29
            stream.Write((byte) 0);
            stream.Write((byte) 0);
            return stream;
        }
    }
}