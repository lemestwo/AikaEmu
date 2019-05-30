using System.IO;
using AikaEmu.GameServer.Models.Data;
using AikaEmu.GameServer.Models.Data.Mobs;
using AikaEmu.GameServer.Models.Data.Npcs;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly string _curDir;
        public CharacterData CharacterData { get; private set; }
        public ConvertCoreData ConvertCoreData { get; private set; }
        public ExpData ExpData { get; private set; }
        public ExpData PranExpData { get; private set; }
        public GearCoresData GearCoresData { get; set; }
        public ItemsData ItemsData { get; private set; }
        public MakeItemsData MakeItemsData { get; private set; }
        public MapsData MapsData { get; private set; }
        public MnData MnData { get; private set; }
        public NpcData NpcData { get; private set; }
        public MobData MobData { get; private set; }
        public QuestData QuestData { get; private set; }
        public ReinforceAData ReinforceAData { get; private set; }
        public ReinforceWData ReinforceWData { get; private set; }
        public RecipesData RecipesData { get; private set; }
        public SetsData SetsData { get; private set; }
        public SkillDataData SkillData { get; private set; }
        public TitlesData TitlesData { get; private set; }

        protected DataManager()
        {
            _curDir = Directory.GetCurrentDirectory() + "\\";
        }

        public void Init()
        {
            CharacterData = new CharacterData(GetPath("Character\\CharacterConfig"));
            _log.Info("Loaded CharacterConfig...");

            ItemsData = new ItemsData(GetPath("Game\\ItemList.bin"));
            _log.Info("Loaded {0} items.", ItemsData.Count);

            MnData = new MnData(GetPath("Game\\MN.bin"));
            _log.Info("Loaded {0} monster names.", MnData.Count);

            NpcData = new NpcData(GetPath("Npcs\\", false));
            _log.Info("Loaded {0} npcs.", NpcData.Count);

            MobData = new MobData(GetPath("Mobs\\", false));
            _log.Info("Loaded {0} mobs.", MobData.Count);

            QuestData = new QuestData(GetPath("Game\\Quest.bin"));
            _log.Info("Loaded {0} quests.", QuestData.Count);

            SkillData = new SkillDataData(GetPath("Game\\SkillData.bin"));
            _log.Info("Loaded {0} skills.", SkillData.Count);

            using (var connection = DatabaseManager.Instance.GetConnection())
            {
                ConvertCoreData = new ConvertCoreData(connection);
                _log.Info("Loaded {0} core converts.", ConvertCoreData.Count);

                GearCoresData = new GearCoresData(connection);
                _log.Info("Loaded {0} core upgrades.", GearCoresData.Count);

                ExpData = new ExpData(connection);
                _log.Info("Loaded {0} levels.", ExpData.Count);

                PranExpData = new ExpData(connection, true);
                _log.Info("Loaded {0} pran levels.", PranExpData.Count);

                MakeItemsData = new MakeItemsData(connection);
                _log.Info("Loaded {0} make items.", MakeItemsData.Count);

                MapsData = new MapsData(connection);
                _log.Info("Loaded {0} maps.", MapsData.Count);

                RecipesData = new RecipesData(connection);
                _log.Info("Loaded {0} recipes.", RecipesData.Count);

                ReinforceAData = new ReinforceAData(connection);
                ReinforceWData = new ReinforceWData(connection);
                _log.Info("Loaded {0} reinforce values.", ReinforceAData.Count + ReinforceWData.Count);

                SetsData = new SetsData(connection);
                _log.Info("Loaded {0} sets.", SetsData.Count);

                TitlesData = new TitlesData(connection);
                _log.Info("Loaded {0} titles.", TitlesData.Count);
            }
        }

        private string GetPath(string dir, bool json = true)
        {
            return _curDir + "Data\\" + dir + (json ? ".json" : "");
        }
    }
}