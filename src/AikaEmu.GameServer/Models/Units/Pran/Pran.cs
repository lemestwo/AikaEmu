using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.Units.Pran.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Models.Units.Pran
{
    public class Pran : BaseUnit
    {
        public Account.Account Account { get; }
        public uint DbId { get; set; }
        public byte Food { get; set; }
        public int Devotion { get; set; }
        public Profession Class { get; set; }
        public uint Experience { get; set; }
        public ushort DefPhy { get; set; }
        public ushort DefMag { get; set; }
        private uint ItemId { get; }
        private Dictionary<Personality, ushort> Personalities { get; }

        public Personality Personality
        {
            get
            {
                if (Personalities.Count <= 0) return Personality.Cute;

                var count = 0;
                var per = Personality.Cute;
                foreach (var (key, value) in Personalities)
                {
                    if (value <= count) continue;
                    per = key;
                    count = value;
                }

                return per;
            }
        }

        public override void SetPosition(Position pos)
        {
            Position = MathUtils.CalculateNextFollowPosition(1, pos);
            SendPacketAround(new UpdatePosition(this, 0));
        }

        public void SendPacketAround(GamePacket packet)
        {
            var online = WorldManager.Instance.GetCharacters();
            foreach (var character in online)
            {
                if (character.VisibleUnits.ContainsKey(Id))
                {
                    character.SendPacket(packet);
                }
            }
        }

        public Pran(Account.Account acc, uint itemId)
        {
            Account = acc;
            ItemId = itemId;
            Personalities = new Dictionary<Personality, ushort>();
        }

        public bool Load()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM prans WHERE acc_id=@acc_id and item_id=@item_id";
                command.Parameters.AddWithValue("@acc_id", Account.DbId);
                command.Parameters.AddWithValue("@item_id", ItemId);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return false;
                    }

                    Id = (ushort) (Account.ActiveCharacter.Connection.Id + 10240); // TODO - Find a way to use IdPranManager
                    DbId = reader.GetUInt32("id");
                    Name = reader.GetString("name");
                    Food = reader.GetByte("food");
                    Devotion = reader.GetInt32("devotion");
                    Class = (Profession) reader.GetInt16("class");
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
                    DefPhy = reader.GetUInt16("def_p");
                    DefMag = reader.GetUInt16("def_m");
                    BodyTemplate = new BodyTemplate
                    {
                        Width = reader.GetByte("width"),
                        Chest = reader.GetByte("chest"),
                        Leg = reader.GetByte("leg")
                    };
                    Position = MathUtils.CalculateNextFollowPosition(1, Account.ActiveCharacter.Position);
                }
            }

            return true;
        }
    }
}