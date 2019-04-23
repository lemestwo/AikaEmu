using AikaEmu.Shared.Model.Configuration;

namespace AikaEmu.GameServer.Models.Data
{
    public class GameServerConfig
    {
        public byte Id { get; set; }
        public NetworkConfig Network { get; set; }
        public NetworkConfig AuthNetwork { get; set; }
        public SqlConnection Database { get; set; }
    }
}