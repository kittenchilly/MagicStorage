using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Сердце Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Serce Jednostki Magazynującej");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Cœur de Stockage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Corazón de Almacenamiento");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储核心");
        }
        
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 1, 35, 0);
            Item.createTile = ModContent.TileType<Components.StorageHeart>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StorageComponent>())
            .AddRecipeGroup("MagicStorage:AnyDiamond", 3)
            .AddIngredient(ItemID.Emerald, 7)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Emerald, 7);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyEmerald", 7);
            //}
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
