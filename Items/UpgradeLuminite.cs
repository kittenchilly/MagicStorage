using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class UpgradeLuminite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminite Storage Upgrade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Люминитовое Улучшение Ячейки Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepszenie jednostki magazynującej (Luminowany)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Amélioration d'Unité de stockage (Luminite)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Actualización de Unidad de Almacenamiento (Luminita)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储升级珠(夜明)");

            Tooltip.SetDefault("Upgrades Storage Unit to 320 capacity"
                + "\n<right> a Blue Chlorophyte Storage Unit to use");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Увеличивает количество слотов в Ячейке Хранилища до 320"
                + "\n<right> на Синей Хлорофитовой Ячейке Хранилища для улучшения");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepsza jednostkę magazynującą do 320 miejsc"
                + "\n<right> na Jednostkę magazynującą (Niebieski Chlorofit), aby użyć");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "améliore la capacité de unité de stockage à 320"
                + "\n<right> l'unité de stockage (Chlorophylle Bleu) pour utiliser");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Capacidad de unidad de almacenamiento mejorada a 320"
                + "\n<right> en la unidad de almacenamiento (Clorofita Azul) para utilizar");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "将存储单元升级至320容量"
                + "\n<right>一个存储单元(夜明)可镶嵌");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = 10;
            Item.value = Item.sellPrice(0, 1, 50, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 10)
            .AddIngredient(ItemID.FragmentSolar, 5)
            .AddIngredient(ItemID.FragmentVortex, 5)
            .AddIngredient(ItemID.FragmentNebula, 5)
            .AddIngredient(ItemID.FragmentStardust, 5)
            .AddIngredient(ItemID.Ruby)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Ruby);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyRuby");
            //}
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
