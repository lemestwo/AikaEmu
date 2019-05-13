namespace AikaEmu.Shared.Model.Configuration
{
    public class SqlConnection
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string Database { get; set; }
    }
}