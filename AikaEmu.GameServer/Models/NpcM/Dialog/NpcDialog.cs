using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.NpcM.Dialog
{
    public class NpcDialog : GamePacket
    {
        private readonly DialogType _type;
        private readonly DialogSubType3 _subType;
        private readonly string _text;

        public NpcDialog(DialogType type, DialogSubType3 subType, string text)
        {
            _type = type;
            _subType = subType;
            _text = text;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _type);
            stream.Write((uint) _subType);
            stream.Write(_text, 60);
            stream.Write(0);
            stream.Write(-1);
            return stream;
        }
    }
}