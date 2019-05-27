using AikaEmu.GameServer.Models.Units.Character.CharTitle;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateActiveTitle : GamePacket
    {
        private readonly Title _title;

        public UpdateActiveTitle(ushort conId, Title title)
        {
            _title = title;

            Opcode = (ushort) GameOpcode.UpdateActiveTitle;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            if (_title == null)
            {
                stream.Write((long) 0);
            }
            else
            {
                stream.Write((uint) _title.Id);
                stream.Write((uint) _title.Level);
            }

            return stream;
        }
    }
}