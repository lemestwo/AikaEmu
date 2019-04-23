using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendPranToWorld : GamePacket
    {
        private readonly Pran _pran;

        public SendPranToWorld(Pran pran)
        {
            _pran = pran;
            Opcode = (ushort) GameOpcode.SendPranToWorld;
            SenderId = pran.Account.ConnectionId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_pran.Name, 16);
            stream.Write((byte) 10); // id?
            stream.Write((byte) 0); // personality ? +5 when food
            stream.Write((short) 0); // unk
            stream.Write(0); // +1 when food used, maybe food qtd uses?

            stream.Write(_pran.Hp);
            stream.Write(_pran.MaxHp);
            stream.Write(_pran.Mp);
            stream.Write(_pran.MaxMp);
            stream.Write(_pran.Experience);
            stream.Write(_pran.PDef);
            stream.Write(_pran.MDef);

            stream.Write("", 8); // fill mostly with FF
            stream.Write(0);
            stream.Write(0);

            for (var i = 0; i < 16; i++)
                stream.Write("", 20);
            
            for (var i = 0; i < 42; i++)
                stream.Write("", 20);

            stream.Write((ushort) 0); // hunger? up to 20.000
            stream.Write((ushort) 0); // family? up to 6.000

            stream.Write(0);
            stream.Write(0);
            stream.Write(0);
            stream.Write(0);
            stream.Write(0);
            stream.Write(0); // updated to 52 when food otherwise 0
            stream.Write(0);
            stream.Write(0);
            stream.Write(0);
            stream.Write(0);
            return stream;
        }
    }
}
