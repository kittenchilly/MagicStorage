using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class UpgradeTerra : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Storage Upgrade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Терра Улучшение Ячейки Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepszenie jednostki magazynującej (Terra)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Amélioration d'Unité de stockage (Terra)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Actualización de Unidad de Almacenamiento (Tierra)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储升级珠(泰拉)");

            Tooltip.SetDefault("Upgrades Storage Unit to 640 capacity"
                + "\n<right> a Luminite Storage Unit to use");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Увеличивает количество слотов в Ячейке Хранилища до 640"
                + "\n<right> на Люминитовой Ячейке Хранилища для улучшения");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepsza jednostkę magazynującą do 640 miejsc"
                + "\n<right> na Jednostkę magazynującą (Luminowaną), aby użyć");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "améliore la capacité de unité de stockage à 640"
                + "\n<right> l'unité de stockage (Luminite) pour utiliser");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Capacidad de unidad de almacenamiento mejorada a 640"
                + "\n<right> en la unidad de almacenamiento (Luminita) para utilizar");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "将存储单元升级至640容量"
                + "\n<right>一个存储单元(泰拉)可镶嵌");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = 11;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override void AddRecipes()
        {
            Mod bluemagicMod = MagicStorage.bluemagicMod;
            Mod calamityMod;
            ModLoader.TryGetMod("CalamityMod", out calamityMod);
            if (bluemagicMod != null)
            {
                CreateRecipe()
                .AddIngredient(bluemagicMod, "InfinityCrystal")
                .AddRecipeGroup("MagicStorage:AnyDiamond")
                .AddTile(bluemagicMod, "PuriumAnvil")
                .Register();
            }
            else if (calamityMod != null)
            {
                CreateRecipe()
                .AddIngredient(calamityMod, "CosmiliteBar", 20)
                .AddRecipeGroup("MagicStorage:AnyDiamond")
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }
            else
            {
                CreateRecipe()
                .AddIngredient(ModContent.ItemType<RadiantJewel>())
                .AddRecipeGroup("MagicStorage:AnyDiamond")
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }
        }
    }
}
