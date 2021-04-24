using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageUnitLuminite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminite Storage Unit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Люминитовая Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Jednostka magazynująca (Luminowana)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de stockage (Luminite)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento (Luminita)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元(夜明)");
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
            Item.rare = 10;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.createTile = ModContent.TileType<Components.StorageUnit>();
            Item.placeStyle = 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.StorageUnitBlueChlorophyte>())
                .AddIngredient(ModContent.ItemType<Items.UpgradeLuminite>())
                .Register();
        }
    }
}
