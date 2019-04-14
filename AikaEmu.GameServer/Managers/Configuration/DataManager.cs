using System;
using System.IO;
using AikaEmu.GameServer.Models.Data.Character;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.GameServer.Managers.Configuration
{
    public class DataManager : Singleton<DataManager>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly string _curDir;
        public CharacterConfig CharacterConfig { get; private set; }

        protected DataManager()
        {
            _curDir = Directory.GetCurrentDirectory() + "\\";
        }

        public void Init()
        {
            _log.Info("Loading CharacterConfig...");
            CharacterConfig = CharacterConfig.FromJson(GetConfigFile("Character\\CharacterConfig"));
        }

        public string GetConfigFile(string dir)
        {
            var configFile = _curDir + "Data\\" + dir + ".json";
            if (!File.Exists(configFile)) return string.Empty;

            string content;
            using (var reader = File.OpenText(configFile))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }
    }
}