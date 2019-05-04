using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Units.Npc
{
    public class NpcDialog : GamePacket
    {
        private readonly DialogType _type;
        private readonly uint _subType;
        private readonly string _text;

        public NpcDialog(DialogType type, uint subType, string text)
        {
            _type = type;
            _subType = subType;
            _text = text;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _type);
            stream.Write(_subType);
            stream.Write(_text, 60);
            stream.Write(0);
            stream.Write(-1);
            return stream;
        }
    }
}