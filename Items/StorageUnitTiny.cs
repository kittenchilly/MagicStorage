using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageUnitTiny : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Storage Unit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Малая Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Mała jednostka magazynująca");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de Stockage Miniscule");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento Minúsculo");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元(小)");
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
            Item.rare = 0;
            Item.value = Item.sellPrice(0, 0, 6, 0);
            Item.createTile = ModContent.TileType<Components.StorageUnit>();
            Item.placeStyle = 8;
        }
    }
}
