using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;
using MagicStorage.Components;

namespace MagicStorage
{
    public class StoragePlayer : ModPlayer
    {
        public int timeSinceOpen = 1;
        private Point16 storageAccess = new Point16(-1, -1);
        public bool remoteAccess = false;

        public override void UpdateDead()
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                CloseStorage();
            }
        }

        public override void ResetEffects()
        {
            if (Player.whoAmI != Main.myPlayer)
            {
                return;
            }
            if (timeSinceOpen < 1)
            {
                Player.SetTalkNPC(-1);
                Main.playerInventory = true;
                timeSinceOpen++;
            }
            if (storageAccess.X >= 0 && storageAccess.Y >= 0 && (Player.chest != -1 || !Main.playerInventory || Player.sign > -1 || Player.talkNPC > -1))
            {
                CloseStorage();
                Recipe.FindRecipes();
            }
            else if (storageAccess.X >= 0 && storageAccess.Y >= 0)
            {
                int playerX = (int)(Player.Center.X / 16f);
                int playerY = (int)(Player.Center.Y / 16f);
                if (!remoteAccess && (playerX < storageAccess.X - Player.tileRangeX || playerX > storageAccess.X + Player.tileRangeX + 1 || playerY < storageAccess.Y - Player.tileRangeY || playerY > storageAccess.Y + Player.tileRangeY + 1))
                {
                    Terraria.Audio.SoundEngine.PlaySound(11, -1, -1, 1);
                    CloseStorage();
                    Recipe.FindRecipes();
                }
                else if (!(TileLoader.GetTile(Main.tile[storageAccess.X, storageAccess.Y].type) is StorageAccess))
                {
                    Terraria.Audio.SoundEngine.PlaySound(11, -1, -1, 1);
                    CloseStorage();
                    Recipe.FindRecipes();
                }
            }
        }

        public void OpenStorage(Point16 point, bool remote = false)
        {
            storageAccess = point;
            remoteAccess = remote;
            StorageGUI.RefreshItems();
        }

        public void CloseStorage()
        {
            storageAccess = new Point16(-1, -1);
            Main.blockInput = false;
            if (StorageGUI.searchBar != null)
            {
                StorageGUI.searchBar.Reset();
            }
            if (StorageGUI.searchBar2 != null)
            {
                StorageGUI.searchBar2.Reset();
            }
            if (CraftingGUI.searchBar != null)
            {
                CraftingGUI.searchBar.Reset();
            }
            if (CraftingGUI.searchBar2 != null)
            {
                CraftingGUI.searchBar2.Reset();
            }
        }

        public Point16 ViewingStorage()
        {
            return storageAccess;
        }

        public static void GetItem(Item Item, bool toMouse)
        {
            Player player = Main.player[Main.myPlayer];
            if (toMouse && Main.playerInventory && Main.mouseItem.IsAir)
            {
                Main.mouseItem = Item;
                Item = new Item();
            }
            else if (toMouse && Main.playerInventory && Main.mouseItem.type == Item.type)
            {
                int total = Main.mouseItem.stack + Item.stack;
                if (total > Main.mouseItem.maxStack)
                {
                    total = Main.mouseItem.maxStack;
                }
                int difference = total - Main.mouseItem.stack;
                Main.mouseItem.stack = total;
                Item.stack -= difference;
            }
            if (!Item.IsAir)
            {
                Item = player.GetItem(Main.myPlayer, Item, GetItemSettings.InventoryEntityToPlayerInventorySettings);
                if (!Item.IsAir)
                {
                    player.QuickSpawnClonedItem(Item, Item.stack);
                }
            }
        }

        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (context != ItemSlot.Context.InventoryItem && context != ItemSlot.Context.InventoryCoin && context != ItemSlot.Context.InventoryAmmo)
            {
                return false;
            }
            if (storageAccess.X < 0 || storageAccess.Y < 0)
            {
                return false;
            }
            Item Item = inventory[slot];
            if (Item.favorited || Item.IsAir)
            {
                return false;
            }
            int oldType = Item.type;
            int oldStack = Item.stack;
            if (StorageCrafting())
            {
                if (Main.netMode == 0)
                {
                    GetCraftingAccess().TryDepositStation(Item);
                }
                else
                {
                    NetHelper.SendDepositStation(GetCraftingAccess().ID, Item);
                    Item.SetDefaults(0, true);
                }
            }
            else
            {
                if (Main.netMode == 0)
                {
                    GetStorageHeart().DepositItem(Item);
                }
                else
                {
                    NetHelper.SendDeposit(GetStorageHeart().ID, Item);
                    Item.SetDefaults(0, true);
                }
            }
            if (Item.type != oldType || Item.stack != oldStack)
            {
                Terraria.Audio.SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
                StorageGUI.RefreshItems();
            }
            return true;
        }

        public TEStorageHeart GetStorageHeart()
        {
            if (storageAccess.X < 0 || storageAccess.Y < 0)
            {
                return null;
            }
            Tile tile = Main.tile[storageAccess.X, storageAccess.Y];
            if (tile == null)
            {
                return null;
            }
            int tileType = tile.type;
            ModTile ModTile = TileLoader.GetTile(tileType);
            if (ModTile == null || !(ModTile is StorageAccess))
            {
                return null;
            }
            return ((StorageAccess)ModTile).GetHeart(storageAccess.X, storageAccess.Y);
        }

        public TECraftingAccess GetCraftingAccess()
        {
            if (storageAccess.X < 0 || storageAccess.Y < 0 || !TileEntity.ByPosition.ContainsKey(storageAccess))
            {
                return null;
            }
            return TileEntity.ByPosition[storageAccess] as TECraftingAccess;
        }

        public bool StorageCrafting()
        {
            if (storageAccess.X < 0 || storageAccess.Y < 0)
            {
                return false;
            }
            Tile tile = Main.tile[storageAccess.X, storageAccess.Y];
            return tile != null && tile.type == ModContent.TileType<CraftingAccess>();
        }

        public static bool IsStorageCrafting()    
        {
            return Main.player[Main.myPlayer].GetModPlayer<StoragePlayer>().StorageCrafting();
        }
    }
}