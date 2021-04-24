using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class CraftingAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storage Crafting Interface");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Модуль Создания Предметов");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Interfejs Rzemieślniczy Magazynu");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Interface de Stockage Artisanat");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Interfaz de Elaboración de almacenamiento");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "制作存储单元");
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
            Item.value = Item.sellPrice(0, 1, 16, 25);
            Item.createTile = ModContent.TileType<Components.CraftingAccess>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StorageComponent>())
            .AddRecipeGroup("MagicStorage:AnyDiamond", 3)
            .AddIngredient(ItemID.Sapphire, 7)
            //if (MagicStorage.legendMod == null)
            //    .AddIngredient(ItemID.Sapphire, 7);
            //else
            //    .AddRecipeGroup("MagicStorage:AnySapphire", 7)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
