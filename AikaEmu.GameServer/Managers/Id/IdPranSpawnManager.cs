using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdPranSpawnManager : IdFactory
    {
        private static IdPranSpawnManager _instance;
        private const uint FirstId = 0x000007D1; // 2001
        private const uint LastId = 0x00000FFF; // 4095
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{ }};

        public static IdPranSpawnManager Instance => _instance ?? (_instance = new IdPranSpawnManager());

        public IdPranSpawnManager() : base("IdPranSpawnManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}