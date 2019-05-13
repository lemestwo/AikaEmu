namespace AikaEmu.GameServer.Models.Guild
{
    public class Guild
    {
        public ushort LogoIndex { get; set; }
        public ushort Id { get; set; }
        public byte NationId { get; set; }
        public string Name { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
        public string Message4 { get; set; }
        public uint Points { get; set; }
        public byte Level { get; set; }
    }
}