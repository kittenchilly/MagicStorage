using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class RadiantJewel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Сияющая Драгоценность");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Promieniejący klejnot");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Bijou Rayonnant");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Joya Radiante");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "光芒四射的宝石");

            Tooltip.SetDefault("'Shines with a dazzling light'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "'Блестит ослепительным светом'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "'Świeci oślepiającym światłem'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "'Il brille avec une lumière aveuglante'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "'Brilla con una luz deslumbrante'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "'闪耀着耀眼的光芒'");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 99;
            Item.rare = 11;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.position, 1f, 1f, 1f);
        }
    }
}
