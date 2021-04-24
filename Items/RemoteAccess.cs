using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class RemoteAccess : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Remote Storage Access");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Модуль Удаленного Доступа к Хранилищу");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Zdalna Jednostka Dostępu");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Fenêtre d'accès éloigné");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Acceso a Almacenamiento Remoto");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "远程存储装置");
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
            Item.value = Item.sellPrice(0, 1, 72, 50);
            Item.createTile = ModContent.TileType<Components.RemoteAccess>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(null, "StorageComponent")
            .AddRecipeGroup("MagicStorage:AnyDiamond", 3)
            .AddIngredient(ItemID.Ruby, 7)
            //if (MagicStorage.legendMod == null)
            //{
            //    recipe.AddIngredient(ItemID.Ruby, 7);
            //}
            //else
            //{
            //    recipe.AddRecipeGroup("MagicStorage:AnyRuby", 7);
            //}
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
