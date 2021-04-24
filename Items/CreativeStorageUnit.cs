using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class CreativeStorageUnit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Креативная Ячейка Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Kreatywna Jednostka Magazynująca");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Unité de Stockage Créatif");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Unidad de Almacenamiento Creativa");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "创造储存单元");
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
            Item.createTile = ModContent.TileType<Components.CreativeStorageUnit>();
        }
    }
}
