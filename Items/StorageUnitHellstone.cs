using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageUnitHellstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellstone Storage Unit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Адская Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Jednostka magazynująca (Piekielny kamień)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de stockage (Infernale)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento (Piedra Infernal)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元(狱岩)");
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
            Item.rare = 2;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.createTile = ModContent.TileType<Components.StorageUnit>();
            Item.placeStyle = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.StorageUnitDemonite>())
            .AddIngredient(ModContent.ItemType<Items.UpgradeHellstone>())
            .Register();

            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.StorageUnitCrimtane>())
            .AddIngredient(ModContent.ItemType<Items.UpgradeHellstone>())
            .Register();
        }
    }
}
