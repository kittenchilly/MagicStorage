using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace MagicStorage
{
    public struct ItemData
    {
        public readonly int Type;
        public readonly int Prefix;

        public ItemData(int type, int prefix = 0)
        {
            this.Type = type;
            this.Prefix = prefix;
        }

        public ItemData(Item Item)
        {
            this.Type = Item.netID;
            this.Prefix = Item.prefix;
        }

        public override bool Equals(Object other)
        {
            if (!(other is ItemData))
            {
                return false;
            }
            return Matches(this, (ItemData)other);
        }

        public override int GetHashCode()
        {
            return 100 * Type + Prefix;
        }

        public static bool Matches(Item Item1, Item Item2)
        {
            return Matches(new ItemData(Item1), new ItemData(Item2));
        }

        public static bool Matches(ItemData data1, ItemData data2)
        {
            return data1.Type == data2.Type && data1.Prefix == data2.Prefix;
        }

        public static int Compare(Item Item1, Item Item2)
        {
            ItemData data1 = new ItemData(Item1);
            ItemData data2 = new ItemData(Item2);
            if (data1.Type != data2.Type)
            {
                return data1.Type - data2.Type;
            }
            return data1.Prefix - data2.Prefix;
        }
    }
}