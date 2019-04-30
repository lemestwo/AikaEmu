using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class MobPosData : BaseData<MobPosJson>
    {
        public MobPosData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<MobPosJson> mobPosData);
            foreach (var mobPos in mobPosData)
                Objects.Add(mobPos.LoopId, mobPos);
        }

        public IEnumerable<Mob> GetAllMob()
        {
            var list = new List<Mob>();

            foreach (var mob in Objects.Values)
            {
                foreach (var mobI in mob.Position)
                {
                    // TODO - Mob data unknown
                    var temp = new Mob
                    {
                        Id = IdMobSpawnManager.Instance.GetNextId(),
                        MobId = mob.MobId,
                        Hp = 2000,
                        Mp = 2000,
                        MaxHp = 2000,
                        MaxMp = 2000,
                        Name = DataManager.Instance.MnData.GetUnitName(mob.MobId),
                        Position = new Position
                        {
                            NationId = 1,
                            CoordX = mobI.CoordX,
                            CoordY = mobI.CoordY
                        },
                        BodyTemplate = new BodyTemplate
                        {
                            Width = 7,
                            Chest = 119,
                            Leg = 119,
                            Body = 0
                        }
                    };
                    list.Add(temp);
                }
            }

            return list;
        }
    }
}