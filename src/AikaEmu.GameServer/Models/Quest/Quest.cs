using AikaEmu.GameServer.Models.Data.JsonModel;

namespace AikaEmu.GameServer.Models.Quest
{
    public class Quest
    {
        public ushort Id { get; set; }
        public byte[] Completed { get; }
        public bool IsDone { get; set; }
        public QuestJson QuestData { get; set; }

        public bool IsCompleted
        {
            get
            {
                var isDone = QuestData.Requires.Count;
                for (var i = 0; i < QuestData.Requires.Count; i++)
                {
                    if (Completed[i] >= QuestData.Requires[i].Quantity2) isDone--;
                }

                return isDone <= 0;
            }
        }

        public Quest()
        {
            Completed = new byte[] {0, 0, 0, 0, 0};
        }
    }
}