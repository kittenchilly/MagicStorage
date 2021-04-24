using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using MagicStorage.Items;

namespace MagicStorage.Components
{
    public class StorageHeart : StorageAccess
    {
        public override ModTileEntity GetTileEntity()
        {
            return (ModTileEntity)TileEntity.manager.GetTileEntity<TEStorageHeart>(ModContent.TileEntityType<TEStorageHeart>());
        }

        public override int ItemType(int frameX, int frameY)
        {
            return ModContent.ItemType<Items.StorageHeart>();
        }

        public override bool HasSmartInteract()
        {
            return true;
        }

        public override TEStorageHeart GetHeart(int i, int j)
        {
            return (TEStorageHeart)TileEntity.ByPosition[new Point16(i, j)];
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item Item = player.inventory[player.selectedItem];
            if (Item.type == ModContent.ItemType<Items.Locator>() || Item.type == ModContent.ItemType<Items.LocatorDisk>() || Item.type == ModContent.ItemType<Items.PortableAccess>())
            {
                if (Main.tile[i, j].frameX % 36 == 18)
                {
                    i--;
                }
                if (Main.tile[i, j].frameY % 36 == 18)
                {
                    j--;
                }
                Locator locator = (Locator)Item.ModItem;
                locator.location = new Point16(i, j);
                if (player.selectedItem == 58)
                {
                    Main.mouseItem = Item.Clone();
                }
                Main.NewText("Locator successfully set to: X=" + i + ", Y=" + j);
                return true;
            }
            else
            {
                return base.RightClick(i, j);
            }
        }
    }
}