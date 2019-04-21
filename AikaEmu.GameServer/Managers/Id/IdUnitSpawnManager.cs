using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdUnitSpawnManager : IdFactory
    {
        private static IdUnitSpawnManager _instance;
        private const uint FirstId = 0x00000BBB; // 3003
        private const uint LastId = 0x00000FFE; // 4094
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{ }};

        public static IdUnitSpawnManager Instance => _instance ?? (_instance = new IdUnitSpawnManager());

        public IdUnitSpawnManager() : base("IdUnitSpawnManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}