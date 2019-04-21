using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdMobSpawnManager: IdFactory
    {
        private static IdMobSpawnManager _instance;
        private const uint FirstId = 0x00000FFF; // 4095
        private const uint LastId = 0x0000FFFF; // 65535
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{ }};

        public static IdMobSpawnManager Instance => _instance ?? (_instance = new IdMobSpawnManager());

        public IdMobSpawnManager() : base("IdMobSpawnManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}