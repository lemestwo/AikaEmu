using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdUnitSpawnManager : IdFactory
    {
        private static IdUnitSpawnManager _instance;
        private const uint FirstId = 0x00001000; // 4096
        private const uint LastId = 0x00003FFF; // 16383
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{ }};

        public static IdUnitSpawnManager Instance => _instance ?? (_instance = new IdUnitSpawnManager());

        public IdUnitSpawnManager() : base("IdUnitSpawnManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}