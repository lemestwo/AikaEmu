using AikaEmu.Shared.Model.Configuration;

namespace AikaEmu.AuthServer.Configuration
{
    public class AuthConfig
    {
        public NetworkConfig Network { get; set; }
        public NetworkConfig InternalNetwork { get; set; }
        public SqlConnection Database { get; set; }
    }
}