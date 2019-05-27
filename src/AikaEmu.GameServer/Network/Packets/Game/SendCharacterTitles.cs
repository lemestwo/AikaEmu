using System.Collections.Generic;
using AikaEmu.GameServer.Models.Units.Character.CharTitle;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendCharacterTitles : GamePacket
    {
        private readonly Dictionary<ushort, Title> _titles;

        public SendCharacterTitles(Dictionary<ushort, Title> titles)
        {
            _titles = titles;

            Opcode = (ushort) GameOpcode.SendCharacterTitles;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            for (var i = 0; i < 128; i++)
            {
                var j = (ushort) (i * 2);
                var j2 = (ushort) (j + 1);
                var titleLeft = _titles.ContainsKey(j2) ? _titles[j2].Level : (byte) 255;
                var titleRight = _titles.ContainsKey(j) ? _titles[j].Level : (byte) 255;
                stream.Write(JoinLevel(titleLeft, titleRight));
            }

            return stream;
        }

        private static byte JoinLevel(byte levelLeft, byte levelRight)
        {
            var result = (byte) ((FormatLevel(levelLeft, true) + FormatLevel(levelRight)) & 0xFF);
            return result;
        }

        private static byte FormatLevel(byte level, bool isLeft = false)
        {
            if (level == 255) return 0;

            if (isLeft) level += 4;
            return (byte) ((0 | 1 << level) & 0xFF);
        }
    }
}