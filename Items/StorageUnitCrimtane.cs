using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageUnitCrimtane : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimtane Storage Unit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Кримтановая Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Jednostka magazynująca (Karmazynium)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de stockage (Carmitane)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento (Carmesí)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元(血腥)");
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
            Item.value = Item.sellPrice(0, 0, 32, 0);
            Item.createTile = ModContent.TileType<Components.StorageUnit>();
            Item.placeStyle = 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.StorageUnit>())
            .AddIngredient(ModContent.ItemType<Items.UpgradeCrimtane>())
            .Register();
        }
    }
}
