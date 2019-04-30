using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendQuestInfo : GamePacket
    {
        public SendQuestInfo()
        {
            Opcode = (ushort) GameOpcode.SendQuestInfo;
        }

        public override PacketStream Write(PacketStream stream)
        {
            ushort questId = 0;
            var require = new byte[] {0x0, 0x0, 0x0, 0x0, 0x0};
            var done = new byte[] {0x0, 0x0, 0x0, 0x0, 0x0};
            var need = new byte[] {0x0, 0x0, 0x0, 0x0, 0x0};
            var needType = new short[] {0x00_00, 0x00_00, 0x00_00, 0x00_00, 0x00_00,};
            byte isCom = 1;
            byte isOld = 1;
            stream.Write((ushort) questId);
            stream.Write(require);
            stream.Write(done);
            stream.Write(need);
            stream.Write((byte) 0);
            stream.Write(needType[0]);
            stream.Write(needType[1]);
            stream.Write(needType[2]);
            stream.Write(needType[3]);
            stream.Write(needType[4]);
            stream.Write(isCom);
            stream.Write(isOld);
            stream.Write((short) 0);
            return stream;

            /*
             * ushort - questId
             * 5 bytes - require type
             * 5 bytes - already have done
             * 5 bytes - require itemid
             * 1 byte - empty?
             * 5 short - require qtt2
             * 1 byte - isComplete
             * 1 byte - unk, could be if its old quest
             * 2 bytes - empty
             */
        }
    }
}