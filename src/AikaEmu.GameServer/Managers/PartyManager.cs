using System.Collections.Generic;
using AikaEmu.GameServer.Models.Group;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class PartyManager : Singleton<PartyManager>
    {
        private readonly Dictionary<uint, Party> _partiesList;

        public PartyManager()
        {
            _partiesList = new Dictionary<uint, Party>();
        }
    }
}