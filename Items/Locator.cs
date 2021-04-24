using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class Locator : ModItem
    {
        public Point16 location = new Point16(-1, -1);

        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Локатор");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Lokalizator");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Localisateur");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Locador");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "定位器");

            Tooltip.SetDefault("<right> Storage Heart to store location"
                + "\n<right> Remote Storage Access to set it");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "<right> по Cердцу Хранилища чтобы запомнить его местоположение"
                + "\n<right> на Модуль Удаленного Доступа к Хранилищу чтобы привязать его к Сердцу Хранилища");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "<right> na serce jednostki magazynującej, aby zapisać jej lokalizację"
                + "\n<right> na bezprzewodowe okno dostępu aby je ustawić");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "<right> le Cœur de Stockage pour enregistrer son emplacement"
                + "\n<right> le Stockage Éloigné pour le mettre en place");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "<right> el Corazón de Almacenamiento para registrar su ubicación"
                + "\n<right> el Acceso de Almacenamiento Remoto para establecerlo"
                + "\n<right> Stockage Éloigné pour le mettre en place");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "<right>存储核心可储存其定位点"
                + "\n<right>远程存储装置以设置其定位点");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 1;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void ModifyTooltips(List<TooltipLine> lines)
        {
            bool isSet = location.X >= 0 && location.Y >= 0;
            for (int k = 0; k < lines.Count; k++)
            {
                if (isSet && lines[k].mod == "Terraria" && lines[k].Name == "Tooltip0")
                {
                    lines[k].text = Language.GetTextValue("Mods.MagicStorage.SetTo", location.X, location.Y);
                }
                else if (!isSet && lines[k].mod == "Terraria" && lines[k].Name == "Tooltip1")
                {
                    lines.RemoveAt(k);
                    k--;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.Amber, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Set("X", location.X);
            tag.Set("Y", location.Y);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            location = new Point16(tag.GetShort("X"), tag.GetShort("Y"));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(location.X);
            writer.Write(location.Y);
        }

        public override void NetReceive(BinaryReader reader)
        {
            location = new Point16(reader.ReadInt16(), reader.ReadInt16());
        }
    }
}
