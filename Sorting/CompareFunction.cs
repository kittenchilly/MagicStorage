using System;
using System.Collections.Generic;
using Terraria;

namespace MagicStorage.Sorting
{
    public abstract class CompareFunction
    {
        public abstract int Compare(Item Item1, Item Item2);

        public int Compare(object object1, object object2)
        {
            if (object1 is Item && object2 is Item)
            {
                return Compare((Item)object1, (Item)object2);
            }
            if (object1 is Recipe && object2 is Recipe)
            {
                return Compare(((Recipe)object1).createItem, ((Recipe)object2).createItem);
            }
            return 0;
        }
    }

    public class CompareID : CompareFunction
    {
        public override int Compare(Item Item1, Item Item2)
        {
            return Item1.type - Item2.type;
        }
    }

    public class CompareName : CompareFunction
    {
        public override int Compare(Item Item1, Item Item2)
        {
            return string.Compare(Item1.Name, Item2.Name, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class CompareQuantity : CompareFunction
    {
        public override int Compare(Item Item1, Item Item2)
        {
            return (int)Math.Ceiling((float)Item2.stack / (float)Item2.maxStack) - (int)Math.Ceiling((float)Item1.stack / (float)Item1.maxStack);
        }
    }
}