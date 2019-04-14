using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateStatus : GamePacket
    {
        public UpdateStatus()
        {
            Opcode = (ushort) GameOpcode.UpdateStatus;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x09, 0x00, 0x29, 0x00, 0x19, 0x00, 0x4A, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x2C, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x02, 0x13, 0x01, 0x00, 0x01, 0x00,
            });
//            stream.Write((ushort) 27); // pAtt
//            stream.Write((ushort) 48); // pDef
//            stream.Write((ushort) 11); // mAtt
//            stream.Write((ushort) 53); // mDef
//            stream.Write((ushort) 0);
//            stream.Write((ushort) 0);
//            stream.Write((ushort) 0);
//            stream.Write((ushort) 45);
//            stream.Write((ushort) 10);
//            stream.Write((ushort) 0);
//            stream.Write((ushort) 0);
//            stream.Write((ushort) 0);
//            stream.Write((ushort) 5); // crit
//            stream.Write((byte) 2);
//            stream.Write((byte) 17);
//            stream.Write((ushort) 1);
//            stream.Write((ushort) 1);
            return stream;
        }
    }
}