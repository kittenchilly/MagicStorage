using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class UpgradeBlueChlorophyte : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Chlorophyte Storage Upgrade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Синее Хлорофитовое Улучшение Ячейки");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepszenie jednostki magazynującej (Niebieski Chlorofit)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Amélioration d'Unité de stockage (Chlorophylle Bleu)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Actualización de Unidad de Almacenamiento (Clorofita Azul)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储升级珠(蓝色叶绿)");

            Tooltip.SetDefault("Upgrades Storage Unit to 240 capacity"
                + "\n<right> a Hallowed Storage Unit to use");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Увеличивает количество слотов в Ячейке Хранилища до 240"
                + "\n<right> на Святой Ячейке Хранилища для улучшения");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepsza jednostkę magazynującą do 240 miejsc"
                + "\n<right> na Jednostkę magazynującą (Święconą), aby użyć");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "améliore la capacité de unité de stockage à 240"
                + "\n<right> l'unité de stockage (Sacré) pour utiliser");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Capacidad de unidad de almacenamiento mejorada a 240"
                + "\n<right> en la unidad de almacenamiento (Sagrado) para utilizar");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "将存储单元升级至240容量"
                + "\n<right>一个存储单元(神圣)可镶嵌");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = 7;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ShroomiteBar, 5)
            .AddIngredient(ItemID.SpectreBar, 5)
            .AddIngredient(ItemID.BeetleHusk, 2)
            .AddIngredient(ItemID.Emerald)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Emerald);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyEmerald");
            //}
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
