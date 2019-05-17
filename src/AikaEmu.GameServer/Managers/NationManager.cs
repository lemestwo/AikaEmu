using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.World.Devir;
using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.GameServer.Managers
{
    public class NationManager : Singleton<NationManager>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<NationId, Dictionary<DevirId, Devir>> _devirsDictionary;

        public NationManager()
        {
            // Reliques
            _devirsDictionary = new Dictionary<NationId, Dictionary<DevirId, Devir>>();
            foreach (NationId nationId in Enum.GetValues(typeof(NationId)))
            {
                if (nationId == NationId.None) continue;
                _devirsDictionary.Add(nationId, new Dictionary<DevirId, Devir>());
                foreach (DevirId devirId in Enum.GetValues(typeof(DevirId)))
                    _devirsDictionary[nationId].Add(devirId, new Devir(nationId, devirId));
            }
        }

        public void Init()
        {
            // Reliques
            using (var con = DatabaseManager.Instance.GetConnection())
            {
                using (var query = con.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM devir_slots";
                    query.Prepare();
                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var nationId = (NationId) reader.GetByte("nation_id");
                            var slotId = reader.GetByte("slot_id");
                            var devirId = (DevirId) reader.GetByte("devir_id");
                            var devirslot = new DevirSlot(reader.GetUInt16("item_id"))
                            {
                                Slot = slotId,
                                IsActive = reader.GetBoolean("is_active"),
                                Name = reader.GetString("put_name"),
                                Time = reader.GetUInt32("put_time")
                            };
                            _devirsDictionary[nationId][devirId].Slots.Add(slotId, devirslot);
                        }
                    }
                }
            }

            _log.Info("Loaded {0} nations and their devirs.", _devirsDictionary.Count);
        }

        public Dictionary<DevirId, Devir> GetNationDevirs(NationId nationId)
        {
            return _devirsDictionary.ContainsKey(nationId) ? _devirsDictionary[nationId] : null;
        }

        public List<ushort> GetReliquesList(NationId nationId)
        {
            var list = new List<ushort>();
            if (_devirsDictionary.ContainsKey(nationId))
            {
                foreach (var devir in _devirsDictionary[nationId].Values)
                foreach (var devirSlot in devir.Slots.Values)
                    if (devirSlot.ItemId != 0)
                        list.Add(devirSlot.ItemId);
            }

            return list;
        }
    }
}