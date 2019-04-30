using AikaEmu.GameServer.Models.Data.JsonModel;

namespace AikaEmu.GameServer.Models.QuestM
{
    public class Quest
    {
        public ushort Id { get; set; }
        public byte[] Completed { get; set; }
        public QuestJson QuestData { get; set; }
    }
}