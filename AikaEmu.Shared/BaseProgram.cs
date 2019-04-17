using System;
using System.IO;
using System.Reflection;
using AikaEmu.Shared.Database;
using AikaEmu.Shared.Model;
using AikaEmu.Shared.Model.Configuration;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;

namespace AikaEmu.Shared
{
    public abstract class BaseProgram
    {
        protected readonly Logger Log = LogManager.GetLogger("GameServer");
        private bool _running;
        private string _appDirectory;

        protected BaseProgram()
        {
        }

        public virtual void Start()
        {
            if (_running)
            {
                Log.Error("Already running.");
                return;
            }

            _running = true;

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            _appDirectory = Directory.GetCurrentDirectory() + "\\";
            LogManager.Configuration = new XmlLoggingConfiguration(_appDirectory + "NLog.config", false);
        }

        protected void SetupDatabase(DatabaseManager db, SqlConnection sql)
        {
            try
            {
                db.Initialize(sql.Host, sql.User, sql.Pass, sql.Database, sql.Port.ToString());
                Log.Info("Database \"{0}\" started.", sql.Database);
            }
            catch (Exception e)
            {
                Log.Error("Database error: {0}", e.Message);
                throw;
            }
        }

        protected void SetupConfig<T>(ref T configVar)
        {
            if (File.Exists(_appDirectory + "Config.json"))
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile(_appDirectory + "Config.json")
                    .Build();

                configurationBuilder.Bind(configVar);
            }
            else
            {
                Log.Error("Config.json not found.");
                Environment.Exit(1);
            }
        }
    }
}