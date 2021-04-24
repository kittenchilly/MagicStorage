using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageConnector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Соединитель Ячеек Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Łącznik");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Connecteur de Stockage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Conector de Almacenamiento");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储连接器");
        }    
    
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.rare = 0;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.createTile = ModContent.TileType<Components.StorageConnector>(); ;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 16)
            .AddRecipeGroup(RecipeGroupID.IronBar)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
