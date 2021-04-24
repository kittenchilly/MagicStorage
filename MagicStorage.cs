using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MagicStorage
{
    public class MagicStorage : Mod
    {
        public static MagicStorage Instance;
        public static Mod bluemagicMod;
        public static Mod legendMod;

        public static readonly Version requiredVersion = new Version(0, 11);

        public override void Load()
        {
            //if (ModLoader.version < requiredVersion)
            //{
            //    throw new Exception("Magic storage requires a tModLoader version of at least " + requiredVersion);
            //}
            Instance = this;
            InterfaceHelper.Initialize();
            ModLoader.TryGetMod("LegendOfTerraria3", out legendMod);
            ModLoader.TryGetMod("Bluemagic", out bluemagicMod);
            AddTranslations();
        }

        public override void Unload()
        {
            Instance = null;
            bluemagicMod = null;
            legendMod = null;
            StorageGUI.Unload();
            CraftingGUI.Unload();
        }

        private void AddTranslations()
        {
            ModTranslation text = CreateTranslation("SetTo");
            text.SetDefault("Set to: X={0}, Y={1}");
            text.AddTranslation((int)GameCulture.CultureName.Polish, "Ustawione na: X={0}, Y={1}");
            text.AddTranslation((int)GameCulture.CultureName.French, "Mis à: X={0}, Y={1}");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Ajustado a: X={0}, Y={1}");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "已设置为: X={0}, Y={1}");
            AddTranslation(text);

            text = CreateTranslation("SnowBiomeBlock");
            text.SetDefault("Snow Biome Block");
            text.AddTranslation((int)GameCulture.CultureName.French, "Bloc de biome de neige");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Bloque de Biomas de la Nieve");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "雪地环境方块");
            AddTranslation(text);

            text = CreateTranslation("DepositAll");
            text.SetDefault("Deposit All");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Переместить всё");
            text.AddTranslation((int)GameCulture.CultureName.French, "Déposer tout");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Depositar todo");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "全部存入");
            AddTranslation(text);

            text = CreateTranslation("Search");
            text.SetDefault("Search");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Поиск");
            text.AddTranslation((int)GameCulture.CultureName.French, "Rechercher");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Buscar");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "搜索");
            AddTranslation(text);

            text = CreateTranslation("SearchName");
            text.SetDefault("Search Name");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Поиск по имени");
            text.AddTranslation((int)GameCulture.CultureName.French, "Recherche par nom");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "búsqueda por nombre");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "搜索名称");
            AddTranslation(text);

            text = CreateTranslation("SearchMod");
            text.SetDefault("Search Mod");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Поиск по моду");
            text.AddTranslation((int)GameCulture.CultureName.French, "Recherche par Mod");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "búsqueda por Mod");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "搜索模组");
            AddTranslation(text);

            text = CreateTranslation("SortDefault");
            text.SetDefault("Default Sorting");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Стандартная сортировка");
            text.AddTranslation((int)GameCulture.CultureName.French, "Tri Standard");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Clasificación por defecto");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "默认排序");
            AddTranslation(text);

            text = CreateTranslation("SortID");
            text.SetDefault("Sort by ID");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Сортировка по ID");
            text.AddTranslation((int)GameCulture.CultureName.French, "Trier par ID");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Ordenar por ID");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "按ID排序");
            AddTranslation(text);

            text = CreateTranslation("SortName");
            text.SetDefault("Sort by Name");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Сортировка по имени");
            text.AddTranslation((int)GameCulture.CultureName.French, "Trier par nom");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Ordenar por nombre");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "按名称排序");
            AddTranslation(text);

            text = CreateTranslation("SortStack");
            text.SetDefault("Sort by Stacks");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Сортировка по стакам");
            text.AddTranslation((int)GameCulture.CultureName.French, "Trier par piles");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Ordenar por pilas");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "按堆栈排序");
            AddTranslation(text);

            text = CreateTranslation("FilterAll");
            text.SetDefault("Filter All");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Всё)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer tout");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar todo");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选全部");
            AddTranslation(text);

            text = CreateTranslation("FilterWeapons");
            text.SetDefault("Filter Weapons");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Оружия)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer par armes");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar por armas");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选武器");
            AddTranslation(text);

            text = CreateTranslation("FilterTools");
            text.SetDefault("Filter Tools");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Инструменты)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer par outils");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar por herramientas");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选工具");
            AddTranslation(text);

            text = CreateTranslation("FilterEquips");
            text.SetDefault("Filter Equipment");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Снаряжения)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer par Équipement");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar por equipamiento");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选装备");
            AddTranslation(text);

            text = CreateTranslation("FilterPotions");
            text.SetDefault("Filter Potions");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Зелья)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer par potions");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar por poción");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选药水");
            AddTranslation(text);

            text = CreateTranslation("FilterTiles");
            text.SetDefault("Filter Placeables");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Размещаемое)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer par placeable");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar por metido");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选放置物");
            AddTranslation(text);

            text = CreateTranslation("FilterMisc");
            text.SetDefault("Filter Misc");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Фильтр (Разное)");
            text.AddTranslation((int)GameCulture.CultureName.French, "Filtrer par miscellanées");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Filtrar por otros");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "筛选杂项");
            AddTranslation(text);

            text = CreateTranslation("CraftingStations");
            text.SetDefault("Crafting Stations");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Станции создания");
            text.AddTranslation((int)GameCulture.CultureName.French, "Stations d'artisanat");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Estaciones de elaboración");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "制作站");
            AddTranslation(text);

            text = CreateTranslation("Recipes");
            text.SetDefault("Recipes");
            text.AddTranslation((int)GameCulture.CultureName.Russian, "Рецепты");
            text.AddTranslation((int)GameCulture.CultureName.French, "Recettes");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Recetas");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "合成配方");
            AddTranslation(text);

            text = CreateTranslation("SelectedRecipe");
            text.SetDefault("Selected Recipe");
            text.AddTranslation((int)GameCulture.CultureName.French, "Recette sélectionnée");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Receta seleccionada");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "选择配方");
            AddTranslation(text);

            text = CreateTranslation("Ingredients");
            text.SetDefault("Ingredients");
            text.AddTranslation((int)GameCulture.CultureName.French, "Ingrédients");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Ingredientes");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "材料");
            AddTranslation(text);

            text = CreateTranslation("StoredItems");
            text.SetDefault("Stored Ingredients");
            text.AddTranslation((int)GameCulture.CultureName.French, "Ingrédients Stockés");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Ingredientes almacenados");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "存储中的材料");
            AddTranslation(text);

            text = CreateTranslation("RecipeAvailable");
            text.SetDefault("Show available recipes");
            text.AddTranslation((int)GameCulture.CultureName.French, "Afficher les recettes disponibles");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Mostrar recetas disponibles");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "显示可合成配方");
            AddTranslation(text);

            text = CreateTranslation("RecipeAll");
            text.SetDefault("Show all recipes");
            text.AddTranslation((int)GameCulture.CultureName.French, "Afficher toutes les recettes");
            text.AddTranslation((int)GameCulture.CultureName.Spanish, "Mostrar todas las recetas");
            text.AddTranslation((int)GameCulture.CultureName.Chinese, "显示全部配方");
            AddTranslation(text);
        }

        public override void PostSetupContent()
        {
            
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Chest",
            ItemID.Chest,
            ItemID.GoldChest,
            ItemID.ShadowChest,
            ItemID.EbonwoodChest,
            ItemID.RichMahoganyChest,
            ItemID.PearlwoodChest,
            ItemID.IvyChest,
            ItemID.IceChest,
            ItemID.LivingWoodChest,
            ItemID.SkywareChest,
            ItemID.ShadewoodChest,
            ItemID.WebCoveredChest,
            ItemID.LihzahrdChest,
            ItemID.WaterChest,
            ItemID.JungleChest,
            ItemID.CorruptionChest,
            ItemID.CrimsonChest,
            ItemID.HallowedChest,
            ItemID.FrozenChest,
            ItemID.DynastyChest,
            ItemID.HoneyChest,
            ItemID.SteampunkChest,
            ItemID.PalmWoodChest,
            ItemID.MushroomChest,
            ItemID.BorealWoodChest,
            ItemID.SlimeChest,
            ItemID.GreenDungeonChest,
            ItemID.PinkDungeonChest,
            ItemID.BlueDungeonChest,
            ItemID.BoneChest,
            ItemID.CactusChest,
            ItemID.FleshChest,
            ItemID.ObsidianChest,
            ItemID.PumpkinChest,
            ItemID.SpookyChest,
            ItemID.GlassChest,
            ItemID.MartianChest,
            ItemID.GraniteChest,
            ItemID.MeteoriteChest,
            ItemID.MarbleChest);
            RecipeGroup.RegisterGroup("MagicStorage:AnyChest", group);
            group = new RecipeGroup(() => Lang.misc[37].Value + " " + Language.GetTextValue("Mods.MagicStorage.SnowBiomeBlock"), ItemID.SnowBlock, ItemID.IceBlock, ItemID.PurpleIceBlock, ItemID.PinkIceBlock);
            if (bluemagicMod != null)
            {
                bluemagicMod.TryFind<ModItem>("DarkBlueIce", out ModItem Item);
                group.ValidItems.Add(Item.Type);
            }
            RecipeGroup.RegisterGroup("MagicStorage:AnySnowBiomeBlock", group);
            group = new RecipeGroup(() => Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Diamond), ItemID.Diamond, ModContent.ItemType<Items.ShadowDiamond>());
            if (legendMod != null)
            {
                legendMod.TryFind<ModItem>("GemChrysoberyl", out ModItem Item);
                group.ValidItems.Add(Item.Type);

                legendMod.TryFind<ModItem>("GemAlexandrite", out ModItem Item2);
                group.ValidItems.Add(Item2.Type);
            }
            RecipeGroup.RegisterGroup("MagicStorage:AnyDiamond", group);
            if (legendMod != null)
            {
                legendMod.TryFind<ModItem>("GemOnyx", out ModItem Item);
                legendMod.TryFind<ModItem>("GemGarnet", out ModItem Item2);
                legendMod.TryFind<ModItem>("GemCharoite", out ModItem Item3);
                legendMod.TryFind<ModItem>("GemPeridot", out ModItem Item4);
                legendMod.TryFind<ModItem>("GemOpal", out ModItem Item5);
                legendMod.TryFind<ModItem>("GenSpinel", out ModItem Item6);

                group = new RecipeGroup(() => Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Amethyst), ItemID.Amethyst, Item.Type, Item6.Type);
                RecipeGroup.RegisterGroup("MagicStorage:AnyAmethyst", group);
                group = new RecipeGroup(() => Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Topaz), ItemID.Topaz, Item2.Type);
                RecipeGroup.RegisterGroup("MagicStorage:AnyTopaz", group);
                group = new RecipeGroup(() => Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Sapphire), ItemID.Sapphire, Item3.Type);
                RecipeGroup.RegisterGroup("MagicStorage:AnySapphire", group);
                group = new RecipeGroup(() => Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Emerald), Item4.Type);
                RecipeGroup.RegisterGroup("MagicStorage:AnyEmerald", group);
                group = new RecipeGroup(() => Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Ruby), ItemID.Ruby, Item5.Type);
                RecipeGroup.RegisterGroup("MagicStorage:AnyRuby", group);
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetHelper.HandlePacket(reader, whoAmI);
        }
    }

    public class MagicStorageSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            InterfaceHelper.ModifyInterfaceLayers(layers);
        }

        public override void PostUpdateInput()
        {
            if (!Main.instance.IsActive)
            {
                return;
            }
            StorageGUI.Update(null);
            CraftingGUI.Update(null);
        }
    }
}

