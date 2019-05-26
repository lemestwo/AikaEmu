using System;
using System.Collections.Generic;
using System.IO;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data.Npcs
{
    public class NpcData : BaseData<NpcColection>
    {
        public NpcData(string path)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(path /*, "*", SearchOption.AllDirectories*/))
                {
                    var collection = new NpcColection();
                    foreach (var file in Directory.GetFiles(dir, "*.json"))
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        switch (fileName)
                        {
                            case "DialogData":
                                JsonUtil.DeserializeFile(file, out NpcDialogJson dialog);
                                collection.NpcDialogJson = dialog;
                                break;
                            case "UnitData":
                                JsonUtil.DeserializeFile(file, out NpcSpawnJson spawn);
                                collection.NpcSpawnJson = spawn;
                                break;
                            case "StoreData":
                                JsonUtil.DeserializeFile(file, out NpcStoreJson store);
                                collection.NpcStoreJson = store;
                                break;
                        }
                    }

                    if (collection.NpcSpawnJson?.NpcId > 0)
                    {
                        Objects.Add(collection.NpcSpawnJson.NpcId, collection);
                    }
                }
            }
            catch (Exception)
            {
                throw new FileNotFoundException("NPC data is missing.");
            }
        }

        public IEnumerable<Npc> GetAllNpc()
        {
            var list = new List<Npc>();

            foreach (var npc in Objects.Values)
            {
                var temp = new Npc
                {
                    Id = (ushort) IdUnitSpawnManager.Instance.GetNextId(),
                    NpcId = npc.NpcSpawnJson.NpcId,
                    NpcIdX = npc.NpcSpawnJson.NpcIdX,
                    Hp = npc.NpcSpawnJson.Hp,
                    Mp = npc.NpcSpawnJson.Mp,
                    MaxHp = npc.NpcSpawnJson.MaxHp,
                    MaxMp = npc.NpcSpawnJson.MaxMp,
                    Position = new Position
                    {
                        NationId = 1,
                        CoordX = npc.NpcSpawnJson.CoordX,
                        CoordY = npc.NpcSpawnJson.CoordY,
                        Rotation = npc.NpcSpawnJson.Rotation
                    },
                    BodyTemplate = new BodyTemplate
                    {
                        Width = npc.NpcSpawnJson.Width,
                        Chest = npc.NpcSpawnJson.Chest,
                        Leg = npc.NpcSpawnJson.Leg
                    },
                    Hair = npc.NpcSpawnJson.Hair,
                    Face = npc.NpcSpawnJson.Face,
                    Helmet = npc.NpcSpawnJson.Helmet,
                    Armor = npc.NpcSpawnJson.Armor,
                    Gloves = npc.NpcSpawnJson.Gloves,
                    Pants = npc.NpcSpawnJson.Pants,
                    Weapon = npc.NpcSpawnJson.Weapon,
                    Shield = npc.NpcSpawnJson.Shield,
                    Refinements = npc.NpcSpawnJson.Refinements,
                    Unk = npc.NpcSpawnJson.Unk,
                    SpawnType = (byte) npc.NpcSpawnJson.SpawnType,
                    UnkId = npc.NpcSpawnJson.ConId,
                    Title = npc.NpcSpawnJson.Title.Trim(),
                    Unk3 = npc.NpcSpawnJson.Unk4,
                    Quests = DataManager.Instance.QuestData.GetQuestByNpc(npc.NpcSpawnJson.NpcIdX),
                };
                if (npc.NpcDialogJson != null)
                {
                    temp.SoundId = npc.NpcDialogJson.SoundId;
                    temp.SoundType = npc.NpcDialogJson.SoundType;
                    temp.DialogList = npc.NpcDialogJson.DialogData;
                }

                if (npc.NpcStoreJson != null && npc.NpcStoreJson.Items.Length == 40)
                {
                    temp.StoreType = (StoreType) npc.NpcStoreJson.StoreType;
                    temp.StoreItems = npc.NpcStoreJson.Items;
                }

                list.Add(temp);
            }

            return list;
        }
    }
}