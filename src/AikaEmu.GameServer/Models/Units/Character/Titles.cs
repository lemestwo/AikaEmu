using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Character.CharTitle;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Model;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Titles : ISaveData
    {
        private readonly Character _character;
        private readonly Dictionary<ushort, Title> _titles;
        private readonly List<ushort> _removedTitles;
        public Title ActiveTitle { get; private set; }

        public Titles(Character character)
        {
            _character = character;
            _titles = new Dictionary<ushort, Title>();
            _removedTitles = new List<ushort>();
            ActiveTitle = null;
        }

        public void Init(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM character_titles WHERE char_id=@char_id";
                command.Parameters.AddWithValue("@char_id", _character.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var title = new Title
                        {
                            Id = reader.GetUInt16("id"),
                            Level = reader.GetByte("level"),
                            IsActive = reader.GetBoolean("is_active"),
                        };
                        title.TitleData = DataManager.Instance.TitlesData.GetTitleData(title.Id, title.Level);

                        if (title.TitleData == null) continue;

                        if (title.IsActive) ActiveTitle = title;
                        _titles.Add(title.Id, title);
                    }
                }
            }
        }

        public void SetActiveTitle(ushort titleId)
        {
            if (ActiveTitle?.Id == titleId) return;

            if (titleId > 0)
            {
                if (_titles.ContainsKey(titleId))
                {
                    if (ActiveTitle != null)
                        _titles[ActiveTitle.Id].IsActive = false;
                    _titles[titleId].IsActive = true;
                    ActiveTitle = _titles[titleId];
                }
            }
            else
            {
                ActiveTitle = null;
            }

            _character.SendPacket(new UpdateActiveTitle(_character.Id, ActiveTitle));
        }

        public void SendTitles()
        {
            _character.Connection.SendPacket(new SendCharacterTitles(_titles));
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            if (_removedTitles.Count > 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM character_titles WHERE id IN(" + string.Join(",", _removedTitles) + ")";
                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                _removedTitles.Clear();
            }

            foreach (var title in _titles.Values)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"id", title.Id},
                    {"level", title.Level},
                    {"char_id", _character.DbId},
                };
                DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "character_titles", parameters, connection, transaction);
            }
        }
    }
}