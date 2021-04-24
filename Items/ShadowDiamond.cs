using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class ShadowDiamond : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Теневой Алмаз");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Mroczny Diament");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Diamant sombre");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Diamante sombreado");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "暗影钻石");

            Tooltip.SetDefault("Traces of light still linger inside");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Следы света все еще мелькают внутри");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Ślady światła wciąż pozostają w środku");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "Des traces de lumière s'attarde encore à l'intérieur");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Sigue habiendo huellas de luz en el interior");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "那道光所余留的痕迹依旧");
        }
        

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
