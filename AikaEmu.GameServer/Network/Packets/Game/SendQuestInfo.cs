using AikaEmu.GameServer.Models.Quest;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendQuestInfo : GamePacket
    {
        private readonly Quest _quest;
        private readonly bool _isOld;

        public SendQuestInfo(Quest quest, bool isOld = false)
        {
            _quest = quest;
            _isOld = isOld;

            Opcode = (ushort) GameOpcode.SendQuestInfo;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_quest.Id);

            foreach (var requireType in _quest.QuestData.Requires)
                stream.Write((byte) requireType.TypeId);
            for (var i = 0; i < 5 - _quest.QuestData.Requires.Count; i++)
                stream.Write((byte) 0);

            foreach (var completed in _quest.Completed)
                stream.Write(completed);

            foreach (var requireQty in _quest.QuestData.Requires)
                stream.Write((byte) requireQty.ItemId1);
            for (var i = 0; i < 5 - _quest.QuestData.Requires.Count; i++)
                stream.Write((byte) 0);

            stream.Write((byte) 0);

            foreach (var requireType2 in _quest.QuestData.Requires)
                stream.Write((short) requireType2.Quantity2);
            for (var i = 0; i < 5 - _quest.QuestData.Requires.Count; i++)
                stream.Write((short) 0);

            stream.Write(_quest.IsCompleted);
            stream.Write(_isOld);
            stream.Write((short) 0);
            return stream;
        }
    }
}