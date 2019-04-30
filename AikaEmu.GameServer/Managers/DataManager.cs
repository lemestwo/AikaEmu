using System.IO;
using AikaEmu.GameServer.Models.Data;
using AikaEmu.GameServer.Models.Data.Npcs;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly string _curDir;
        public CharInitialData CharInitial { get; private set; }
        public ExperienceData ExperienceData { get; set; }
        public ExperienceData PranExperienceData { get; set; }
        public ItemsData ItemsData { get; private set; }
        public MnData MnData { get; private set; }
        public MobEffectsData MobEffectsData { get; private set; }
        public NpcData NpcData { get; private set; }
        public MobPosData MobPosData { get; private set; }
        public QuestData QuestData { get; private set; }
        public SPositionData SPositionData { get; private set; }

        protected DataManager()
        {
            _curDir = Directory.GetCurrentDirectory() + "\\";
        }

        public void Init()
        {
            CharInitial = new CharInitialData(GetPath("Character\\CharacterConfig"));
            _log.Info("Loaded CharacterConfig...");

            ItemsData = new ItemsData(GetPath("Game\\ItemList.bin"));
            _log.Info("Loaded {0} items.", ItemsData.Count);

            MnData = new MnData(GetPath("Game\\MN.bin"));
            _log.Info("Loaded {0} monster names.", MnData.Count);

            NpcData = new NpcData(GetPath("Npcs\\", false));
            _log.Info("Loaded {0} npcs.", NpcData.Count);

            MobEffectsData = new MobEffectsData(GetPath("Game\\MobEffects.csv"));
            _log.Info("Loaded {0} mobs effects.", MobEffectsData.Count);

            MobPosData = new MobPosData(GetPath("Game\\MobPos.bin"));
            _log.Info("Loaded {0} mobs.", MobPosData.Count);

            QuestData = new QuestData(GetPath("Game\\Quest.bin"));
            _log.Info("Loaded {0} quests.", QuestData.Count);

            SPositionData = new SPositionData(GetPath("Game\\SPosition.bin"));
            _log.Info("Loaded {0} teleport positions.", SPositionData.Count);

            ExperienceData = new ExperienceData(GetPath("Game\\ExpList.bin"));
            _log.Info("Loaded {0} levels experience.", ExperienceData.Count);
            
            PranExperienceData = new ExperienceData(GetPath("Game\\PranExpList.bin"));
            _log.Info("Loaded {0} pran levels experience.", PranExperienceData.Count);
        }

        private string GetPath(string dir, bool json = true)
        {
            return _curDir + "Data\\" + dir + (json ? ".json" : "");
        }
    }
}