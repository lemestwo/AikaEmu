using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdItemManager : IdFactory
    {
        private static IdItemManager _instance;
        private const uint FirstId = 0x00000001;
        private const uint LastId = 0xFFFFFFFF;
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{"items", "id"}};

        public static IdItemManager Instance => _instance ?? (_instance = new IdItemManager());

        public IdItemManager() : base("IdItemManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}