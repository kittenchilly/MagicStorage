using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace MagicStorage.Sorting
{
    public abstract class ItemFilter
    {
        public abstract bool Passes(Item Item);

        public bool Passes(object obj)
        {
            if (obj is Item)
            {
                return Passes((Item)obj);
            }
            if (obj is Recipe)
            {
                return Passes(((Recipe)obj).createItem);
            }
            return false;
        }
    }

    public class FilterAll : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return true;
        }
    }

    public class FilterMelee : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.DamageType == DamageClass.Melee && Item.pick == 0 && Item.axe == 0 && Item.hammer == 0;
        }
    }

    public class FilterRanged : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.DamageType == DamageClass.Ranged;
        }
    }

    public class FilterMagic : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.DamageType == DamageClass.Magic;
        }
    }

    public class FilterSummon : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.DamageType == DamageClass.Summon;
        }
    }

    public class FilterThrown : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.DamageType == DamageClass.Throwing;
        }
    }

    public class FilterOtherWeapon : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.DamageType != DamageClass.Melee && Item.DamageType != DamageClass.Ranged && Item.DamageType != DamageClass.Magic && Item.DamageType != DamageClass.Summon && Item.DamageType != DamageClass.Throwing && Item.damage > 0;
        }
    }

    public class FilterWeapon : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.damage > 0 && Item.pick == 0 && Item.axe == 0 && Item.hammer == 0;
        }
    }

    public class FilterPickaxe : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.pick > 0;
        }
    }

    public class FilterAxe : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.axe > 0;
        }
    }

    public class FilterHammer : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.hammer > 0;
        }
    }

    public class FilterTool : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.pick > 0 || Item.axe > 0 || Item.hammer > 0;
        }
    }

    public class FilterEquipment : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.headSlot >= 0 || Item.bodySlot >= 0 || Item.legSlot >= 0 || Item.accessory || Main.projHook[Item.shoot] || Item.mountType >= 0 || (Item.buffType > 0 && (Main.lightPet[Item.buffType] || Main.vanityPet[Item.buffType]));
        }
    }

    public class FilterPotion : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.consumable && (Item.healLife > 0 || Item.healMana > 0 || Item.buffType > 0);
        }
    }

    public class FilterPlaceable : ItemFilter
    {
        public override bool Passes(Item Item)
        {
            return Item.createTile >= 0 || Item.createWall > 0;
        }
    }

    public class FilterMisc : ItemFilter
    {
        private static List<ItemFilter> blacklist = new List<ItemFilter> {
            new FilterWeapon(),
            new FilterTool(),
            new FilterEquipment(),
            new FilterPotion(),
            new FilterPlaceable()
        };

        public override bool Passes(Item Item)
        {
            foreach (var filter in blacklist)
            {
                if (filter.Passes(Item))
                {
                    return false;
                }
            }
            return true;
        }
    }
}