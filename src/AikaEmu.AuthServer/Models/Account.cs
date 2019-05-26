using System;
using System.Net;

namespace AikaEmu.AuthServer.Models
{
    public class Account
    {
        public uint Id { get; set; }
        public string User { get; set; }
        public byte Level { get; set; }
        public IPAddress LastIp { get; set; }
        public byte IsBlock { get; set; }
        public string SessionHash { get; set; }
        public DateTime SessionTime { get; set; }
    }
}