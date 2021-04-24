using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class UpgradeHallowed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hallowed Storage Upgrade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Святое Улучшение Ячейки Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepszenie jednostki magazynującej (Święcone)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Amélioration d'Unité de stockage (Sacré)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Actualización de Unidad de Almacenamiento (Sagrado)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储升级珠(神圣)");

            Tooltip.SetDefault("Upgrades Storage Unit to 160 capacity"
                + "\n<right> a Hellstone Storage Unit to use");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Увеличивает количество слотов в Ячейке Хранилища до 160"
                + "\n<right> на Адской Ячейке Хранилища для улучшения");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepsza jednostkę magazynującą do 160 miejsc"
                + "\n<right> na Jednostkę magazynującą (Piekielny kamień), aby użyć");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "améliore la capacité de unité de stockage à 160"
                + "\n<right> l'unité de stockage (Infernale) pour utiliser");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Capacidad de unidad de almacenamiento mejorada a 160"
                + "\n<right> en la unidad de almacenamiento (Piedra Infernal) para utilizar");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "将存储单元升级至160容量"
                + "\n<right>一个存储单元(神圣)可镶嵌");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = 4;
            Item.value = Item.sellPrice(0, 0, 40, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.SoulofFright)
            .AddIngredient(ItemID.SoulofMight)
            .AddIngredient(ItemID.SoulofSight)
            .AddIngredient(ItemID.Sapphire)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Sapphire);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnySapphire");
            //}
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
