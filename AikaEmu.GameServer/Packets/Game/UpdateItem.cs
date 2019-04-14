using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateItem : GamePacket
    {
        private readonly ushort _slot;

        public UpdateItem(ushort slot)
        {
            _slot = slot;
            Opcode = (ushort) GameOpcode.UpdateItem;
        }

        public override PacketStream Write(PacketStream stream)
        {
            if (_slot == 84)
            {
                stream.Write(new byte[]
                {
                    0x00, 0x02, 0x54, 0x00, 0x68, 0x00, 0x68, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x2B, 0x1B, 0x5B, 0x4C, 0x7F, 0x00, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
                });
            }
            else
            {
                stream.Write(false); // isNotice
                stream.Write((byte) 2); // typeslot
                stream.Write(_slot);
                stream.Write((ushort) 0); // id
                stream.Write((ushort) 0);
                stream.Write((ushort) 0);
                stream.Write(0);
                stream.Write("", 12);
            }

            return stream;
        }
    }
}