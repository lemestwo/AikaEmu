using AikaEmu.Shared.Model;
using AikaEmu.Shared.Model.Configuration;

namespace AikaEmu.GameServer.Configuration
{
    public class GameConfig
    {
        public byte Id { get; set; }
        public NetworkConfig Network { get; set; }
        public NetworkConfig AuthNetwork { get; set; }
        public SqlConnection Database { get; set; }
    }
}