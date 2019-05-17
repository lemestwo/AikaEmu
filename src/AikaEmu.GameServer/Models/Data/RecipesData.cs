using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models.Data
{
    public class RecipesData : BaseData<RecipeDataModel>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public RecipesData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_recipes`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var makeItem = new RecipeDataModel
                        {
                            ItemId = reader.GetUInt16("item_id"),
                            ResultItemId = reader.GetUInt16("result_item_id"),
                            ResultItemId2 = reader.GetUInt16("result2_item_id"),
                            Price = reader.GetUInt64("price"),
                            Quantity = reader.GetUInt16("quantity"),
                            Level = reader.GetByte("level"),
                            Rate = reader.GetUInt32("rate"),
                            RateSup = reader.GetUInt32("rate_sup"),
                            RateDouble = reader.GetUInt32("rate_double"),
                            Ingredients = new Dictionary<ushort, RecipeIngredients>()
                        };
                        Objects.Add(makeItem.ItemId, makeItem);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_recipe_ingredients`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetUInt16("recipe_id");
                        var itemId = reader.GetUInt16("item_id");
                        var makeItemIngredients = new RecipeIngredients
                        {
                            Quantity = reader.GetUInt16("quantity"),
                            Rate = new ushort[16],
                            IsRateItem = false
                        };
                        if (Objects.ContainsKey(id))
                            Objects[id].Ingredients.Add(itemId, makeItemIngredients);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_recipe_rates`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var recipeId = reader.GetUInt16("recipe_id");
                        var itemId = reader.GetUInt16("item_id");
                        if (!Objects.ContainsKey(recipeId) || !Objects[recipeId].Ingredients.ContainsKey(itemId)) continue;

                        Objects[recipeId].Ingredients[itemId].IsRateItem = true;
                        Objects[recipeId].Ingredients[itemId].IncreasedChance = reader.GetBoolean("increased_chance");
                        for (var i = 0; i < 16; i++)
                            Objects[recipeId].Ingredients[itemId].Rate[i] = reader.GetUInt16("rate_" + i);
                    }
                }
            }
        }
    }
}