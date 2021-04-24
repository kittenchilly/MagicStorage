using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageUnitDemonite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonite Storage Unit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Демонитовая Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Jednostka magazynująca (Demonit)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de stockage (Démonite)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento (Endemoniado)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元(魔金)");
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
            Item.placeStyle = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.StorageUnit>())
            .AddIngredient(ModContent.ItemType<Items.UpgradeDemonite>())
            .Register();
        }
    }
}
