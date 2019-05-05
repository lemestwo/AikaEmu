using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public interface ISaveData
    {
        void Init(MySqlConnection connection);
        void Save(MySqlConnection connection, MySqlTransaction transaction);
    }
}