using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Mob;
using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Pran;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class WorldManager : Singleton<WorldManager>
    {
        private readonly ConcurrentDictionary<ushort, Character> _characters;
        private readonly ConcurrentDictionary<ushort, Npc> _npcs;
        private readonly ConcurrentDictionary<ushort, Mob> _mobs;
        private readonly ConcurrentDictionary<ushort, Pran> _prans;

        protected WorldManager()
        {
            _characters = new ConcurrentDictionary<ushort, Character>();
            _npcs = new ConcurrentDictionary<ushort, Npc>();
            _mobs = new ConcurrentDictionary<ushort, Mob>();
            _prans = new ConcurrentDictionary<ushort, Pran>();
        }

        public Npc GetNpc(ushort conId)
        {
            return _npcs.ContainsKey(conId) ? _npcs[conId] : null;
        }

        public IEnumerable<Character> GetCharacters()
        {
            return _characters.Values.ToList();
        }

        public Character GetCharacter(ushort id)
        {
            return _characters.ContainsKey(id) ? _characters[id] : null;
        }

        public Character GetCharacter(string name)
        {
            return _characters.Values.FirstOrDefault(character => character.Name.Equals(name));
        }

        public Character GetCharacter(uint id)
        {
            return _characters.Values.FirstOrDefault(character => character.DbId == id);
        }

        public static void SpawnUnits()
        {
            foreach (var npc in DataManager.Instance.NpcData.GetAllNpc())
                npc.Spawn();

            foreach (var mob in DataManager.Instance.MobData.GetAllMob())
                mob.Spawn();
        }

        public void Spawn(BaseUnit unit)
        {
            if (unit == null) return;

            switch (unit)
            {
                case Character character:
                    _characters.TryAdd(character.Id, character);
                    break;
                case Npc npc:
                    _npcs.TryAdd(npc.Id, npc);
                    break;
                case Mob mob:
                    _mobs.TryAdd(mob.Id, mob);
                    break;
                case Pran pran:
                    _prans.TryAdd(pran.Id, pran);
                    break;
            }

            foreach (var onlineChar in GetCharacters())
                if (onlineChar.IsAround(unit.Position, 70))
                    ShowVisibleUnits(onlineChar);
        }

        public void Despawn(BaseUnit unit)
        {
            if (unit == null) return;

            switch (unit)
            {
                case Character character:
                    _characters.TryRemove(character.Id, out _);
                    if (character.ActivePran != null)
                        Despawn(character.ActivePran);
                    break;
                case Npc npc:
                    _npcs.TryRemove(npc.Id, out _);
                    break;
                case Mob mob:
                    _mobs.TryRemove(mob.Id, out _);
                    break;
                case Pran pran:
                    _prans.TryRemove(pran.Id, out _);
                    break;
            }

            foreach (var character in GetCharacters())
                if (character.IsAround(unit.Position, 70) && character.Id != unit.Id)
                    ShowVisibleUnits(character);
        }

        public void ShowVisibleUnits(BaseUnit unit)
        {
            if (!(unit is Character character)) return;

            if (character.VisibleUnits.Count <= 0)
            {
                character.VisibleUnits = GetUnitsAround(character.Position, character.Id);

                foreach (var (_, tempUnit) in character.VisibleUnits)
                {
                    switch (tempUnit)
                    {
                        case Mob mob:
                            character.Connection.SendPacket(new SendMobSpawn(mob));
                            break;
                        case Npc npc:
                            character.Connection.SendPacket(new SendUnitSpawn(tempUnit, 0, character));
                            break;
                        default:
                            character.Connection.SendPacket(new SendUnitSpawn(tempUnit));
                            break;
                    }
                }
            }
            else
            {
                var newUnits = GetUnitsAround(character.Position, character.Id);

                var oldIds = new List<uint>(character.VisibleUnits.Keys);
                var newIds = new List<uint>(newUnits.Keys);

                var deleteUnits = oldIds.Except(newIds);
                var spawnUnits = newIds.Except(oldIds);

                foreach (var unitDel in deleteUnits)
                    character.Connection.SendPacket(new DespawnUnit(unitDel));

                foreach (var unitSpawn in spawnUnits)
                {
                    var tempUnit = newUnits[unitSpawn];
                    switch (tempUnit)
                    {
                        case Mob mob:
                            character.Connection.SendPacket(new SendMobSpawn(mob));
                            break;
                        case Npc npc:
                            character.Connection.SendPacket(new SendUnitSpawn(tempUnit, 0, character));
                            break;
                        default:
                            character.Connection.SendPacket(new SendUnitSpawn(tempUnit));
                            break;
                    }
                }

                character.VisibleUnits = newUnits;
            }
        }

        private Dictionary<uint, BaseUnit> GetUnitsAround(Position pos, uint myId)
        {
            var list = new Dictionary<uint, BaseUnit>();

            foreach (var character in _characters.Values)
            {
                if (character.IsAround(pos, 70) && character.Id != myId) list.Add(character.Connection.Id, character);
            }

            foreach (var pran in _prans.Values)
            {
                if (pran.IsAround(pos, 70) && pran.Id != myId) list.Add(pran.Id, pran);
            }

            foreach (var npc in _npcs.Values)
            {
                if (npc.IsAround(pos, 30) && npc.Id != myId) list.Add(npc.Id, npc);
            }

            foreach (var mob in _mobs.Values)
            {
                if (mob.IsAround(pos, 30) && mob.Id != myId) list.Add(mob.Id, mob);
            }

            return list;
        }
    }
}