using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class CreateGuildBox : GamePacket
    {
        private readonly int _i;

        public CreateGuildBox(ushort conId, int i)
        {
            _i = i;
            Opcode = (ushort) GameOpcode.CreateGuildBox;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_i);
            if (_i > 0)
            {
                stream.Write(_i);
                stream.Write(0L); // gold?
                stream.Write("guild name", 18);
                stream.Write((short) 0);
                stream.Write(0);
            }

            return stream;
        }
    }
}