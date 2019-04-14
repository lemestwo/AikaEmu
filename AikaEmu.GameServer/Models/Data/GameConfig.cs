using AikaEmu.Shared.Model.Configuration;

namespace AikaEmu.GameServer.Models.Data
{
    public class GameConfig
    {
        public byte Id { get; set; }
        public NetworkConfig Network { get; set; }
        public NetworkConfig AuthNetwork { get; set; }
        public SqlConnection Database { get; set; }
    }
}