﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class UpgradeDemonite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonite Storage Upgrade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Демонитовое Улучшение Ячейки Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepszenie jednostki magazynującej (Demonit)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Amélioration d'Unité de stockage (Démonite)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Actualización de Unidad de Almacenamiento (Endemoniado)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储升级珠(魔金)");

            Tooltip.SetDefault("Upgrades Storage Unit to 80 capacity"
                + "\n<right> a Storage Unit to use");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Увеличивает количество слотов в Ячейке Хранилища до 80"
                + "\n<right> на Ячейке Хранилища для улучшения");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepsza jednostkę magazynującą do 80 miejsc"
                + "\n<right> na Jednostkę magazynującą (Standardową), aby użyć");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "améliore la capacité de unité de stockage à 80"
                + "\n<right> l'unité de stockage pour utiliser");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Capacidad de unidad de almacenamiento mejorada a 80"
                + "\n<right> en la unidad de almacenamiento para utilizar");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "将存储单元升级至80容量"
                + "\n<right>一个存储单元(魔金)可镶嵌");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 0, 32, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddIngredient(ItemID.Amethyst)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Amethyst);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyAmethyst");
            //}
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
