using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.NpcM;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data.Npcs
{
    public class NpcColection
    {
        public NpcSpawnJson NpcSpawnJson { get; set; }
        public NpcDialogJson NpcDialogJson { get; set; }
    }

    public class NpcData
    {
        private readonly Dictionary<ushort, NpcColection> _npcs;
        public int Count => _npcs.Count;

        public NpcData(string path)
        {
            _npcs = new Dictionary<ushort, NpcColection>();

            try
            {
                foreach (var dir in Directory.GetDirectories(path))
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
                        }
                    }

                    if (collection.NpcSpawnJson?.NpcId > 0)
                    {
                        _npcs.Add(collection.NpcSpawnJson.NpcId, collection);
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

            foreach (var npc in _npcs.Values)
            {
                // TODO - Find if npc data is in the client
                // Hair, BodyTemplate, Hp, Mp
                var temp = new Npc
                {
                    Id = IdUnitSpawnManager.Instance.GetNextId(),
                    NpcId = npc.NpcSpawnJson.NpcId,
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
                    Unk2 = npc.NpcSpawnJson.Unk2,
                    Title = npc.NpcSpawnJson.Title.Trim(),
                    Unk3 = npc.NpcSpawnJson.Unk4
                };
                if (npc.NpcDialogJson != null)
                {
                    temp.SoundId = npc.NpcDialogJson.SoundId;
                    temp.SoundType = npc.NpcDialogJson.SoundType;
                    temp.DialogList = npc.NpcDialogJson.DialogData;
                }

                list.Add(temp);
            }

            return list;
        }
    }
}