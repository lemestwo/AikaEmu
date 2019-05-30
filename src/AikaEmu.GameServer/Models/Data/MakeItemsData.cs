using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class MakeItemsData : BaseData<MakeItemDataModel>
    {
        public MakeItemsData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_make_items`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var makeItem = new MakeItemDataModel
                        {
                            ResultItemId = reader.GetUInt16("id"),
                            ResultSupItemId = reader.GetUInt16("result_sup_item_id"),
                            Level = reader.GetByte("level"),
                            Price = reader.GetUInt64("price"),
                            Quantity = reader.GetUInt16("quantity"),
                            Rate = reader.GetUInt32("rate"),
                            RateSup = reader.GetUInt32("rate_sup"),
                            RateDouble = reader.GetUInt32("rate_double"),
                            Ingredients = new List<MakeItemIngredients>()
                        };
                        Objects.Add(makeItem.ResultItemId, makeItem);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_make_item_ingredients`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetUInt16("id");
                        var makeItemIngredients = new MakeItemIngredients
                        {
                            ItemId = reader.GetUInt16("item_id"),
                            Quantity = reader.GetByte("quantity")
                        };
                        if (Objects.ContainsKey(id))
                            Objects[id].Ingredients.Add(makeItemIngredients);
                    }
                }
            }
        }
    }
}