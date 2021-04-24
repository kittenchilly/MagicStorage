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
    public class PortableAccess : Locator
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Portable Remote Storage Access");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Портативный Модуль Удаленного Доступа к Хранилищу");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "便携式远程存储装置");

            Tooltip.SetDefault("<right> Storage Heart to store location"
                + "\nCurrently not set to any location"
                + "\nUse Item to access your storage");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "<right> по Cердцу Хранилища чтобы запомнить его местоположение"
                + "\nВ данный момент Сердце Хранилища не привязанно"
                + "\nИспользуйте что бы получить доступ к вашему Хранилищу");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "<right>存储核心可储存其定位点"
                + "\n目前未设置为任何位置"
                + "\n使用可直接访问你的存储");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Purple;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (location.X >= 0 && location.Y >= 0)
                {
                    Tile tile = Main.tile[location.X, location.Y];
                    if (!tile.IsActive || tile.type != ModContent.TileType<Components.StorageHeart>() || tile.frameX != 0 || tile.frameY != 0)
                    {
                        Main.NewText("Storage Heart is missing!");
                    }
                    else
                    {
                        OpenStorage(player);
                    }
                }
                else
                {
                    Main.NewText("Locator is not set to any Storage Heart");
                }
            }
            return true;
        }

        private void OpenStorage(Player player)
        {
            StoragePlayer ModPlayer = player.GetModPlayer<StoragePlayer>();
            if (player.sign > -1)
            {
                Terraria.Audio.SoundEngine.PlaySound(11, -1, -1, 1);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            if (Main.editChest)
            {
                Terraria.Audio.SoundEngine.PlaySound(12, -1, -1, 1);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            if (player.talkNPC > -1)
            {
                player.SetTalkNPC(-1);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = string.Empty;
            }
            bool hadChestOpen = player.chest != -1;
            player.chest = -1;
            Main.stackSplit = 600;
            Point16 toOpen = location;
            Point16 prevOpen = ModPlayer.ViewingStorage();
            if (prevOpen == toOpen)
            {
                ModPlayer.CloseStorage();
                Terraria.Audio.SoundEngine.PlaySound(11, -1, -1, 1);
                Recipe.FindRecipes();
            }
            else
            {
                bool hadOtherOpen = prevOpen.X >= 0 && prevOpen.Y >= 0;
                ModPlayer.OpenStorage(toOpen, true);
                ModPlayer.timeSinceOpen = 0;
                Main.playerInventory = true;
                Main.recBigList = false;
                Terraria.Audio.SoundEngine.PlaySound(hadChestOpen || hadOtherOpen ? 12 : 10, -1, -1, 1);
                Recipe.FindRecipes();
            }
        }

        public override void ModifyTooltips(List<TooltipLine> lines)
        {
            bool isSet = location.X >= 0 && location.Y >= 0;
            for (int k = 0; k < lines.Count; k++)
            {
                if (isSet && lines[k].mod == "Terraria" && lines[k].Name == "Tooltip1")
                {
                    lines[k].text = Language.GetTextValue("Mods.MagicStorage.SetTo", location.X, location.Y);
                }
                else if (!isSet && lines[k].mod == "Terraria" && lines[k].Name == "Tooltip2")
                {
                    lines.RemoveAt(k);
                    k--;
                }
            }
        }

        public override void AddRecipes()
        {
            Mod bluemagicMod = MagicStorage.bluemagicMod;
            Mod calamityMod;
            ModLoader.TryGetMod("CalamityMod", out calamityMod);
            if (calamityMod != null)
            {
                CreateRecipe()
                    .AddIngredient<LocatorDisk>()
                    .AddIngredient(calamityMod, "CosmiliteBar" , 20)
                    .AddRecipeGroup("MagicStorage:AnyDiamond", 3)
                    .AddIngredient(ItemID.Ruby, 7)
                    .AddTile(TileID.LunarCraftingStation)
                    .Register();
            }
            else if (bluemagicMod != null)
            {
                CreateRecipe()
                    .AddIngredient<LocatorDisk>()
                    .AddIngredient(bluemagicMod, "InfinityCrystal")
                    .AddRecipeGroup("MagicStorage:AnyDiamond", 3)
                    .AddIngredient(ItemID.Ruby, 7)
                    .AddTile(bluemagicMod, "PuriumAnvil")
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient<LocatorDisk>()
                    .AddIngredient<RadiantJewel>()
                    .AddRecipeGroup("MagicStorage:AnyDiamond", 3)
                    .AddIngredient(ItemID.Ruby, 7)
                    .AddTile(TileID.LunarCraftingStation)
                    .Register();
            }
        }
    }
}
