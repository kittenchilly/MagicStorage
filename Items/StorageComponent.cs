using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageComponent : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Компонент Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Komponent Magazynu");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Composant de Stockage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Componente de Almacenamiento");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储组件");
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
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.createTile = ModContent.TileType<Components.StorageComponent>(); ;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 10)
            .AddRecipeGroup(RecipeGroupID.IronBar, 2)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
