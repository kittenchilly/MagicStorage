using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageUnitHallowed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hallowed Storage Unit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Святая Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Jednostka magazynująca (Święcona)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de stockage (Sacré)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento (Sagrado)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元(神圣)");
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
            Item.rare = 4;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.createTile = ModContent.TileType<Components.StorageUnit>();
            Item.placeStyle = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.StorageUnitHellstone>())
            .AddIngredient(ModContent.ItemType<Items.UpgradeHallowed>())
            .Register();
        }
    }
}
