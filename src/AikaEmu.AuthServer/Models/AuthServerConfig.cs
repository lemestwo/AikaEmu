using AikaEmu.Shared.Model.Configuration;

namespace AikaEmu.AuthServer.Models
{
	public class AuthServerConfig
	{
		public NetworkConfig Network { get; set; }
		public NetworkConfig InternalNetwork { get; set; }
		public SqlConnection Database { get; set; }
	}
}