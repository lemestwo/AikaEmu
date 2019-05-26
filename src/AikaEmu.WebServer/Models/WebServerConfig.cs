using AikaEmu.Shared.Model.Configuration;

namespace AikaEmu.WebServer.Models
{
    public class WebServerConfig
    {
        public SqlConnection DatabaseAuth { get; set; }
        public SqlConnection DatabaseGame { get; set; }
    }
}