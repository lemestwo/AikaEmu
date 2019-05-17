using System;
using System.Collections.Generic;
using System.IO;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Mob;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data.Mobs
{
    public class MobData : BaseData<MobCollection>
    {
        public MobData(string path)
        {
            try
            {
                JsonUtil.DeserializeFile(path + "MobPos.bin.json", out List<MobPosJson> mobPosData);

                foreach (var dir in Directory.GetDirectories(path))
                {
                    var collection = new MobCollection();
                    foreach (var file in Directory.GetFiles(dir, "*.json"))
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        switch (fileName)
                        {
                            case "MobData":
                                JsonUtil.DeserializeFile(file, out MobDataJson dialog);
                                collection.MobData = dialog;
                                break;
                        }
                    }

                    if (collection.MobData?.MobId > 0)
                    {
                        Objects.Add((ushort) collection.MobData.MobId, collection);
                    }
                }

                foreach (var mobPos in mobPosData)
                {
                    if (Objects.ContainsKey(mobPos.MobId))
                        Objects[mobPos.MobId].MobPos = mobPos;
                }

                var keys = new List<ushort>();
                foreach (var (key, value) in Objects)
                {
                    if (value.MobPos == null || value.MobData == null)
                        keys.Add(key);
                }

                foreach (var key in keys)
                {
                    Objects.Remove(key);
                }
            }
            catch (Exception)
            {
                throw new FileNotFoundException("MOB data is missing.");
            }
        }

        public IEnumerable<Mob> GetAllMob()
        {
            var list = new List<Mob>();

            foreach (var mob in Objects.Values)
            {
                if (mob.MobPos == null) continue;
                foreach (var mobP in mob.MobPos.Position)
                {
                    // TODO - Mob data unknown
                    var temp = new Mob
                    {
                        Id = (ushort) IdMobSpawnManager.Instance.GetNextId(),
                        MobId = mob.MobData.MobId,
                        Model = mob.MobData.MobModel,
                        Hp1 = mob.MobData.Hp1,
                        Hp2 = mob.MobData.Hp2,
                        Hp3 = mob.MobData.Hp3,
                        Unk1 = (byte) mob.MobData.Unk1,
                        Unk2 = (byte) mob.MobData.Unk2,
                        Unk3 = (byte) mob.MobData.Unk3,
                        Unk4 = (byte) mob.MobData.Unk4,
                        Unk5 = (byte) mob.MobData.Unk5,
                        Unk6 = mob.MobData.Unk6,
                        Unk7 = mob.MobData.Unk7,
                        Position = new Position
                        {
                            NationId = 1,
                            CoordX = mobP.CoordX,
                            CoordY = mobP.CoordY,
                            Rotation = 0
                        },
                        BodyTemplate = new BodyTemplate
                        {
                            Width = (byte) mob.MobData.Width,
                            Chest = (byte) mob.MobData.Chest,
                            Leg = (byte) mob.MobData.Leg,
                        }
                    };
                    list.Add(temp);
                }
            }

            return list;
        }
    }
}