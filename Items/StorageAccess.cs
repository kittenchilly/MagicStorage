using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Модуль Доступа к Хранилищу");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Okno dostępu do magazynu");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Access de Stockage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Acceso de Almacenamiento");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储装置");
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
            Item.value = Item.sellPrice(0, 0, 67, 50);
            Item.createTile = ModContent.TileType<Components.StorageAccess>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StorageComponent>())
            .AddRecipeGroup("MagicStorage:AnyDiamond", 1)
            .AddIngredient(ItemID.Topaz, 7)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Topaz, 7);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyTopaz", 7);
            //}
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
