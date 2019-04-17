using System.Collections.Generic;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class NpcPosData
    {
        private readonly Dictionary<ushort, NpcPosJson> _npcs;
        public int Count => _npcs.Count;

        public NpcPosData(string path)
        {
            _npcs = new Dictionary<ushort, NpcPosJson>();
            JsonUtil.DeserializeFile(path, out List<NpcPosJson> npcPosData);
            foreach (var npcPos in npcPosData)
                _npcs.Add(npcPos.LoopId, npcPos);
        }

        public IEnumerable<Npc> GetAllNpc()
        {
            var list = new List<Npc>();

            foreach (var npc in _npcs.Values)
            {
                // TODO - Find if npc data is in the client
                // Hair, BodyTemplate, Hp, Mp
                var temp = new Npc
                {
                    Id = IdUnitSpawnManager.Instance.GetNextId(),
                    NpcId = npc.NpcId,
                    Hp = 2000,
                    Mp = 2000,
                    MaxHp = 2000,
                    MaxMp = 2000,
                    Name = DataManager.Instance.MnData.GetUnitName(npc.NpcId),
                    Position = new Position
                    {
                        WorldId = 1,
                        CoordX = npc.CoordX,
                        CoordY = npc.CoordY
                    },
                    BodyTemplate = new BodyTemplate
                    {
                        Width = 7,
                        Chest = 119,
                        Leg = 119,
                        Body = 219
                    }
                };
                list.Add(temp);
            }

            return list;
        }
    }
}