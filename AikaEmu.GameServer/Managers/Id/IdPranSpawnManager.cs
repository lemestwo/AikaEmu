using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdPranSpawnManager : IdFactory
    {
        private static IdPranSpawnManager _instance;
        private const uint FirstId = 0x00002800; // 10240
        private const uint LastId = 0x0000FFFF; // 
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{ }};

        public static IdPranSpawnManager Instance => _instance ?? (_instance = new IdPranSpawnManager());

        public IdPranSpawnManager() : base("IdPranSpawnManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}