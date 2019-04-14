using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdConnectionManager : IdFactory
    {
        private static IdConnectionManager _instance;
        private const uint FirstId = 0x00000001; // 1
        private const uint LastId = 0x00000FFF; // 4095
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{ }};

        public static IdConnectionManager Instance => _instance ?? (_instance = new IdConnectionManager());

        public IdConnectionManager() : base("IdConnectionManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}