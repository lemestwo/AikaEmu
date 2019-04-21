using System;
using System.Collections.Generic;
using System.IO;
using AikaEmu.GameServer.Models.Data;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.GameServer.Managers.Configuration
{
	public class DataManager : Singleton<DataManager>
	{
		private readonly Logger _log = LogManager.GetCurrentClassLogger();
		private readonly string _curDir;
		public CharInitialData CharInitial { get; private set; }
		public ItemsData ItemsData { get; private set; }
		public MnData MnData { get; private set; }
		public MobEffectsData MobEffectsData { get; private set; }
		public NpcPosData NpcPosData { get; private set; }
		public MobPosData MobPosData { get; private set; }

		protected DataManager()
		{
			_curDir = Directory.GetCurrentDirectory() + "\\";
		}

		public void Init()
		{
			_log.Info("Loading CharacterConfig...");
			CharInitial = new CharInitialData(GetPath("Character\\CharacterConfig"));

			_log.Info("Loading ItemList...");
			ItemsData = new ItemsData(GetPath("Game\\ItemList.bin"));
			_log.Info("Loaded {0} items.", ItemsData.Count);

			_log.Info("Loading MN...");
			MnData = new MnData(GetPath("Game\\MN.bin"));
			_log.Info("Loaded {0} names.", MnData.Count);

			_log.Info("Loading MobEffects...");
			MobEffectsData = new MobEffectsData(GetPath("Game\\MobEffects.csv"));
			_log.Info("Loaded {0} mobs.", MobEffectsData.Count);

			_log.Info("Loading NpcPos...");
			NpcPosData = new NpcPosData(GetPath("Game\\npcpos.bin"));
			_log.Info("Loaded {0} npcs.", NpcPosData.Count);

			_log.Info("Loading MobPos...");
			MobPosData = new MobPosData(GetPath("Game\\MobPos.bin"));
			_log.Info("Loaded {0} mobs.", MobPosData.Count);

			// TODO EXP / PRAN XP
		}

		private string GetPath(string dir)
		{
			return _curDir + "Data\\" + dir + ".json";
		}
	}
}