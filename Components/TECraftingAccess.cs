using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MagicStorage.Components
{
    public class TECraftingAccess : TEStorageComponent
    {
        public Item[] stations = new Item[10];

        public TECraftingAccess()
        {
            for (int k = 0; k < 10; k++)
            {
                stations[k] = new Item();
            }
        }

        public override bool ValidTile(Tile tile)
        {
            return tile.type == ModContent.TileType<CraftingAccess>() && tile.frameX == 0 && tile.frameY == 0;
        }

        public void TryDepositStation(Item Item)
        {
            if (Main.netMode == 1)
            {
                return;
            }
            foreach (Item station in stations)
            {
                if (station.type == Item.type)
                {
                    return;
                }
            }
            for (int k = 0; k < stations.Length; k++)
            {
                if (stations[k].IsAir)
                {
                    stations[k] = Item.Clone();
                    stations[k].stack = 1;
                    Item.stack--;
                    if (Item.stack <= 0)
                    {
                        Item.SetDefaults(0);
                    }
                    NetHelper.SendTEUpdate(ID, Position);
                    return;
                }
            }
        }

        public Item TryWithdrawStation(int slot)
        {
            if (Main.netMode == 1)
            {
                return new Item();
            }
            if (!stations[slot].IsAir)
            {
                Item Item = stations[slot];
                stations[slot] = new Item();
                NetHelper.SendTEUpdate(ID, Position);
                return Item;
            }
            return new Item();
        }

        public Item DoStationSwap(Item Item, int slot)
        {
            if (Main.netMode == 1)
            {
                return new Item();
            }
            if (!Item.IsAir)
            {
                for (int k = 0; k < stations.Length; k++)
                {
                    if (k != slot && stations[k].type == Item.type)
                    {
                        return Item;
                    }
                }
            }
            if ((Item.IsAir || Item.stack == 1) && !stations[slot].IsAir)
            {
                Item temp = Item;
                Item = stations[slot];
                stations[slot] = temp;
                NetHelper.SendTEUpdate(ID, Position);
                return Item;
            }
            else if (!Item.IsAir && stations[slot].IsAir)
            {
                stations[slot] = Item.Clone();
                stations[slot].stack = 1;
                Item.stack--;
                if (Item.stack <= 0)
                {
                    Item.SetDefaults(0);
                }
                NetHelper.SendTEUpdate(ID, Position);
                return Item;
            }
            return Item;
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            IList<TagCompound> listStations = new List<TagCompound>();
            foreach (Item Item in stations)
            {
                listStations.Add(ItemIO.Save(Item));
            }
            tag["Stations"] = listStations;
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            IList<TagCompound> listStations = tag.GetList<TagCompound>("Stations");
            if (listStations != null && listStations.Count > 0)
            {
                for (int k = 0; k < stations.Length; k++)
                {
                    stations[k] = ItemIO.Load(listStations[k]);
                }
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            foreach (Item Item in stations)
            {
                ItemIO.Send(Item, writer, true, false);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            for (int k = 0; k < stations.Length; k++)
            {
                stations[k] = ItemIO.Receive(reader, true, false);
            }
        }
    }
}