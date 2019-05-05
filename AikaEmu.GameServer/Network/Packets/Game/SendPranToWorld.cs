using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Units.Pran;
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
            SenderId = pran.Account.Connection.Id;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_pran.Name, 16);
            // 63 = fire / 73 = water / 83 = air
            stream.Write((byte) _pran.Class); // profession
            stream.Write(_pran.Food); // food (max is 120)
            stream.Write((short) _pran.Personality); 
            stream.Write(_pran.Devotion); // devotion % (max is 225?)
            
            stream.Write(_pran.MaxHp);
            stream.Write(_pran.Hp);
            stream.Write(_pran.MaxMp);
            stream.Write(_pran.Mp);
            stream.Write(_pran.Experience);
            stream.Write(_pran.DefPhy);
            stream.Write(_pran.DefMag);

            stream.Write(-1); // FF FF FF FF
            // sometimes it changes from FF FF FF FF
            stream.Write((byte) 255);
            stream.Write((byte) 255);
            stream.Write((byte) 253); // 253
            stream.Write((byte) 243); // 243

            stream.Write(0); // 1987
            stream.Write(0);

            // 0 = model? element?
            // 6 = hair icon
            for (var i = 0; i < 16; i++)
            {
                if (i == 0) stream.Write((ushort) _pran.Face);
                else if (i == 6) stream.Write((ushort) _pran.Hair);
                else stream.Write((ushort) 0);
                stream.Write("", 18);
            }

            for (var i = 0; i < 42; i++)
            {
                if (i == 40) stream.Write((ushort) 5301);
                else if (i == 41) stream.Write((ushort) 5301);
                else stream.Write((ushort) 0);
                stream.Write("", 18);
            }

            stream.Write((byte) 30); // pran quick skill
            stream.Write((byte) 58); // pran quick skill
            stream.Write((byte) 75); // pran quick skill
            stream.Write((byte) 0); // unk

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