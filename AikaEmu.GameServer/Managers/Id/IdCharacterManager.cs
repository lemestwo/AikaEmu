using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Managers.Id
{
    public class IdCharacterManager : IdFactory
    {
        private static IdCharacterManager _instance;
        private const uint FirstId = 0x00000001;
        private const uint LastId = 0xFFFFFFFF;
        private static readonly uint[] Exclude = { };
        private static readonly string[,] ObjTables = {{"characters", "id"}};

        public static IdCharacterManager Instance => _instance ?? (_instance = new IdCharacterManager());

        public IdCharacterManager() : base("CharacterIdManager", FirstId, LastId, ObjTables, Exclude)
        {
        }
    }
}