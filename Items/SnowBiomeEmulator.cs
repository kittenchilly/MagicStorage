﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class SnowBiomeEmulator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Snowglobe");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Сломанная Снежная Сфера");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Emulator Śnieżnego Biomu");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Emulateur de biome de neige");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Emulador de bioma de la nieve");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雪地环境模拟器");

            Tooltip.SetDefault("Allows the Storage Crafting Interface to craft snow biome recipes");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Позволяет Модулю Создания Предметов создавать предметы требующие нахождения игрока в снежном биоме");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "Dodaje funkcje do Interfejsu Rzemieślniczego, pozwalającą na wytwarzanie przedmiotów dostępnych jedynie w Śnieżnym Biomie");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "Permet à L'interface de Stockage Artisanat de créer des recettes de biome de neige");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "Permite la Interfaz de Elaboración de almacenamiento a hacer de recetas de bioma de la nieve");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "允许制作存储单元拥有雪地环境");

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 8));
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("MagicStorage:AnySnowBiomeBlock", 300)
            .AddTile(ModContent.TileType<Components.CraftingAccess>())
            .Register();
        }
    }
}
