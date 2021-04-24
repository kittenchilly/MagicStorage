using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class UpgradeHellstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellstone Storage Upgrade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Адское Улучшение Ячейки Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepszenie jednostki magazynującej (Piekielny kamień)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Amélioration d'Unité de stockage (Infernale)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Actualización de Unidad de Almacenamiento (Piedra Infernal)");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储升级珠(狱岩)");

            Tooltip.SetDefault("Upgrades Storage Unit to 120 capacity"
                + "\n<right> a Demonite/Crimtane Storage Unit to use");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Увеличивает количество слотов в Ячейке Хранилища до 120"
                + "\n<right> на Демонитовой/Кримтановой Ячейке Хранилища для улучшения");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ulepsza jednostkę magazynującą do 120 miejsc"
                + "\n<right> na Jednostkę magazynującą (Karmazynit/Demonit), aby użyć");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "améliore la capacité de unité de stockage à 120"
                + "\n<right> l'unité de stockage (Démonite/Carmitane) pour utiliser");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Capacidad de unidad de almacenamiento mejorada a 120"
                + "\n<right> en la unidad de almacenamiento (Endemoniado/Carmesí) para utilizar");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "将存储单元升级至120容量"
                + "\n<right>一个存储单元(血腥/魔金)可镶嵌");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 99;
            Item.rare = 2;
            Item.value = Item.sellPrice(0, 0, 40, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 10)
            .AddIngredient(ItemID.Topaz)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Topaz);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyTopaz");
            //}
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
