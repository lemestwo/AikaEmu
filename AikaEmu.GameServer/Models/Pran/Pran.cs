using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Unit;
using Microsoft.EntityFrameworkCore.Storage;

namespace AikaEmu.GameServer.Models.Pran
{
    public class Pran : BaseUnit
    {
        public Account Account { get; set; }
        public uint ItemId { get; set; }
        public byte Slot { get; set; }
        public byte Food { get; set; }
        public int Devotion { get; set; }
        public Dictionary<Personality, ushort> Personalities { get; }
        public uint Experience { get; set; }
        public short Face { get; set; }
        public short Hair { get; set; }
        public ushort DefPhy { get; set; }
        public ushort DefMag { get; set; }
        public ushort ConnectionId => Account.ConnectionId;

        public Pran(Account acc, uint itemId)
        {
            Personalities = new Dictionary<Personality, ushort>();
        }

        public bool Load()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM prans WHERE acc_id=@acc_id and item_id=@item_id";
                command.Parameters.AddWithValue("@acc_id", Account.Id);
                command.Parameters.AddWithValue("@item_id", ItemId);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return false;
                    }

                    Slot = reader.GetByte("slot");
                    Name = reader.GetString("name");
                    Food = reader.GetByte("food");
                    Devotion = reader.GetInt32("devotion");
                    Personalities[Personality.Cute] = reader.GetUInt16("p_cute");
                    Personalities[Personality.Smart] = reader.GetUInt16("p_smart");
                    Personalities[Personality.Sexy] = reader.GetUInt16("p_sexy");
                    Personalities[Personality.Energetic] = reader.GetUInt16("p_energetic");
                    Personalities[Personality.Tough] = reader.GetUInt16("p_tough");
                    Personalities[Personality.Corrupt] = reader.GetUInt16("p_corrupt");
                    Level = reader.GetUInt16("level");
                    Hp = reader.GetInt32("hp");
                    MaxHp = reader.GetInt32("max_hp");
                    Mp = reader.GetInt32("mp");
                    MaxMp = reader.GetInt32("max_mp");
                    Experience = reader.GetUInt32("xp");
                    Face = reader.GetInt16("face");
                    Hair = reader.GetInt16("hair");
                    DefPhy = reader.GetUInt16("def_p");
                    DefMag = reader.GetUInt16("def_m");
                    BodyTemplate = new BodyTemplate
                    {
                        Width = reader.GetByte("width"),
                        Chest = reader.GetByte("chest"),
                        Leg = reader.GetByte("leg")
                    };
                }
            }

            return true;
        }
    }
}