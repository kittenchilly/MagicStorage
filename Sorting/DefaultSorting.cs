using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MagicStorage.Sorting
{
    public class CompareDefault : CompareFunction
    {
        public CompareDefault()
        {
            SortClassList.Initialize();
        }

        public override int Compare(Item Item1, Item Item2)
        {
            return SortClassList.Compare(Item1, Item2);
        }
    }

    public static class SortClassList
    {
        private static bool initialized = false;
        private static List<DefaultSortClass> classes = new List<DefaultSortClass>();

        public static int Compare(Item Item1, Item Item2)
        {
            int class1 = classes.Count;
            int class2 = classes.Count;
            for (int k = 0; k < classes.Count; k++)
            {
                if (classes[k].Pass(Item1))
                {
                    class1 = k;
                    break;
                }
            }
            for (int k = 0; k < classes.Count; k++)
            {
                if (classes[k].Pass(Item2))
                {
                    class2 = k;
                    break;
                }
            }
            if (class1 != class2)
            {
                return class1 - class2;
            }
            return classes[class1].Compare(Item1, Item2);
        }

        public static void Initialize()
        {
            if (initialized)
            {
                return;
            }
            classes.Add(new DefaultSortClass(MeleeWeapon, CompareRarity));
            classes.Add(new DefaultSortClass(RangedWeapon, CompareRarity));
            classes.Add(new DefaultSortClass(MagicWeapon, CompareRarity));
            classes.Add(new DefaultSortClass(SummonWeapon, CompareRarity));
            classes.Add(new DefaultSortClass(ThrownWeapon, CompareRarity));
            classes.Add(new DefaultSortClass(Weapon, CompareRarity));
            classes.Add(new DefaultSortClass(Ammo, CompareRarity));
            classes.Add(new DefaultSortClass(Picksaw, ComparePicksaw));
            classes.Add(new DefaultSortClass(Hamaxe, CompareHamaxe));
            classes.Add(new DefaultSortClass(Pickaxe, ComparePickaxe));
            classes.Add(new DefaultSortClass(Axe, CompareAxe));
            classes.Add(new DefaultSortClass(Hammer, CompareHammer));
            classes.Add(new DefaultSortClass(TerraformingTool, CompareTerraformingPriority));
            classes.Add(new DefaultSortClass(AmmoTool, CompareRarity));
            classes.Add(new DefaultSortClass(Armor, CompareRarity));
            classes.Add(new DefaultSortClass(VanityArmor, CompareRarity));
            classes.Add(new DefaultSortClass(Accessory, CompareAccessory));
            classes.Add(new DefaultSortClass(Grapple, CompareRarity));
            classes.Add(new DefaultSortClass(Mount, CompareRarity));
            classes.Add(new DefaultSortClass(Cart, CompareRarity));
            classes.Add(new DefaultSortClass(LightPet, CompareRarity));
            classes.Add(new DefaultSortClass(VanityPet, CompareRarity));
            classes.Add(new DefaultSortClass(Dye, CompareDye));
            classes.Add(new DefaultSortClass(HairDye, CompareHairDye));
            classes.Add(new DefaultSortClass(HealthPotion, CompareHealing));
            classes.Add(new DefaultSortClass(ManaPotion, CompareMana));
            classes.Add(new DefaultSortClass(Elixir, CompareElixir));
            classes.Add(new DefaultSortClass(BuffPotion, CompareRarity));
            classes.Add(new DefaultSortClass(BossSpawn, CompareBossSpawn));
            classes.Add(new DefaultSortClass(Painting, ComparePainting));
            classes.Add(new DefaultSortClass(Wiring, CompareWiring));
            classes.Add(new DefaultSortClass(Material, CompareMaterial));
            classes.Add(new DefaultSortClass(Rope, CompareRope));
            classes.Add(new DefaultSortClass(Extractible, CompareExtractible));
            classes.Add(new DefaultSortClass(Misc, CompareMisc));
            classes.Add(new DefaultSortClass(FrameImportantTile, CompareName));
            classes.Add(new DefaultSortClass(CommonTile, CompareName));
        }

        private static bool MeleeWeapon(Item Item)
        {
            return Item.maxStack == 1 && Item.damage > 0 && Item.ammo == 0 && Item.DamageType == DamageClass.Melee && Item.pick < 1 && Item.hammer < 1 && Item.axe < 1;
        }

        private static bool RangedWeapon(Item Item)
        {
            return Item.maxStack == 1 && Item.damage > 0 && Item.ammo == 0 && Item.DamageType == DamageClass.Ranged;
        }

        private static bool MagicWeapon(Item Item)
        {
            return Item.maxStack == 1 && Item.damage > 0 && Item.ammo == 0 && Item.DamageType == DamageClass.Magic;
        }

        private static bool SummonWeapon(Item Item)
        {
            return Item.maxStack == 1 && Item.damage > 0 && Item.DamageType == DamageClass.Summon;
        }

        private static bool ThrownWeapon(Item Item)
        {
            return Item.damage > 0 && (Item.ammo == 0 || Item.notAmmo) && Item.shoot > 0 && Item.DamageType == DamageClass.Throwing;
        }

        private static bool Weapon(Item Item)
        {
            return Item.damage > 0 && Item.ammo == 0 && Item.pick == 0 && Item.axe == 0 && Item.hammer == 0;
        }

        private static bool Ammo(Item Item)
        {
            return Item.ammo > 0 && Item.damage > 0;
        }

        private static bool Picksaw(Item Item)
        {
            return Item.pick > 0 && Item.axe > 0;
        }

        private static bool Hamaxe(Item Item)
        {
            return Item.hammer > 0 && Item.axe > 0;
        }

        private static bool Pickaxe(Item Item)
        {
            return Item.pick > 0;
        }

        private static bool Axe(Item Item)
        {
            return Item.axe > 0;
        }

        private static bool Hammer(Item Item)
        {
            return Item.hammer > 0;
        }

        private static bool TerraformingTool(Item Item)
        {
            return ItemID.Sets.SortingPriorityTerraforming[Item.type] >= 0;
        }

        private static bool AmmoTool(Item Item)
        {
            return Item.ammo > 0;
        }

        private static bool Armor(Item Item)
        {
            return (Item.headSlot >= 0 || Item.bodySlot >= 0 || Item.legSlot >= 0) && !Item.vanity;
        }

        private static bool VanityArmor(Item Item)
        {
            return (Item.headSlot >= 0 || Item.bodySlot >= 0 || Item.legSlot >= 0) && Item.vanity;
        }

        private static bool Accessory(Item Item)
        {
            return Item.accessory;
        }

        private static bool Grapple(Item Item)
        {
            return Main.projHook[Item.shoot];
        }

        private static bool Mount(Item Item)
        {
            return Item.mountType != -1 && !MountID.Sets.Cart[Item.mountType];
        }

        private static bool Cart(Item Item)
        {
            return Item.mountType != -1 && MountID.Sets.Cart[Item.mountType];
        }

        private static bool LightPet(Item Item)
        {
            return Item.buffType > 0 && Main.lightPet[Item.buffType];
        }

        private static bool VanityPet(Item Item)
        {
            return Item.buffType > 0 && Main.vanityPet[Item.buffType];
        }

        private static bool Dye(Item Item)
        {
            return Item.dye > 0;
        }

        private static bool HairDye(Item Item)
        {
            return Item.hairDye >= 0;
        }

        private static bool HealthPotion(Item Item)
        {
            return Item.consumable && Item.healLife > 0 && Item.healMana < 1;
        }

        private static bool ManaPotion(Item Item)
        {
            return Item.consumable && Item.healLife < 1 && Item.healMana > 0;
        }

        private static bool Elixir(Item Item)
        {
            return Item.consumable && Item.healLife > 0 && Item.healMana > 0;
        }

        private static bool BuffPotion(Item Item)
        {
            return Item.consumable && Item.buffType > 0;
        }

        private static bool BossSpawn(Item Item)
        {
            return ItemID.Sets.SortingPriorityBossSpawns[Item.type] >= 0;
        }

        private static bool Painting(Item Item)
        {
            return ItemID.Sets.SortingPriorityPainting[Item.type] >= 0 || Item.paint > 0;
        }

        private static bool Wiring(Item Item)
        {
            return ItemID.Sets.SortingPriorityWiring[Item.type] >= 0 || Item.mech;
        }

        private static bool Material(Item Item)
        {
            return ItemID.Sets.SortingPriorityMaterials[Item.type] >= 0;
        }

        private static bool Rope(Item Item)
        {
            return ItemID.Sets.SortingPriorityRopes[Item.type] >= 0;
        }

        private static bool Extractible(Item Item)
        {
            return ItemID.Sets.SortingPriorityExtractibles[Item.type] >= 0;
        }

        private static bool Misc(Item Item)
        {
            return Item.createTile < 0 && Item.createWall < 1;
        }

        private static bool FrameImportantTile(Item Item)
        {
            return Item.createTile >= 0 && Main.tileFrameImportant[Item.createTile];
        }

        private static bool CommonTile(Item Item)
        {
            return Item.createTile >= 0 || Item.createWall > 0;
        }

        private static int CompareRarity(Item Item1, Item Item2)
        {
            return Item2.rare - Item1.rare;
        }

        private static int ComparePicksaw(Item Item1, Item Item2)
        {
            int result = Item1.pick - Item2.pick;
            if (result == 0)
            {
                result = Item1.axe - Item2.axe;
            }
            return result;
        }

        private static int CompareHamaxe(Item Item1, Item Item2)
        {
            int result = Item1.axe - Item2.axe;
            if (result == 0)
            {
                result = Item1.hammer - Item2.hammer;
            }
            return result;
        }

        private static int ComparePickaxe(Item Item1, Item Item2)
        {
            return Item1.pick - Item2.pick;
        }

        private static int CompareAxe(Item Item1, Item Item2)
        {
            return Item1.axe - Item2.axe;
        }

        private static int CompareHammer(Item Item1, Item Item2)
        {
            return Item1.hammer - Item2.hammer;
        }

        private static int CompareTerraformingPriority(Item Item1, Item Item2)
        {
            return ItemID.Sets.SortingPriorityTerraforming[Item1.type] - ItemID.Sets.SortingPriorityTerraforming[Item2.type];
        }

        private static int CompareAccessory(Item Item1, Item Item2)
        {
            int result = Item1.vanity.CompareTo(Item2.vanity);
            if (result == 0)
            {
                result = CompareRarity(Item1, Item2);
            }
            return result;
        }

        private static int CompareDye(Item Item1, Item Item2)
        {
            int result = CompareRarity(Item1, Item2);
            if (result == 0)
            {
                result = Item2.dye - Item1.dye;
            }
            return result;
        }

        private static int CompareHairDye(Item Item1, Item Item2)
        {
            int result = CompareRarity(Item1, Item2);
            if (result == 0)
            {
                result = Item2.hairDye - Item1.hairDye;
            }
            return result;
        }

        private static int CompareHealing(Item Item1, Item Item2)
        {
            return Item2.healLife - Item1.healLife;
        }

        private static int CompareMana(Item Item1, Item Item2)
        {
            return Item2.mana - Item1.mana;
        }

        private static int CompareElixir(Item Item1, Item Item2)
        {
            int result = CompareHealing(Item1, Item2);
            if (result == 0)
            {
                result = CompareMana(Item1, Item2);
            }
            return result;
        }

        private static int CompareBossSpawn(Item Item1, Item Item2)
        {
            return ItemID.Sets.SortingPriorityBossSpawns[Item1.type] - ItemID.Sets.SortingPriorityBossSpawns[Item2.type];
        }

        private static int ComparePainting(Item Item1, Item Item2)
        {
            int result = ItemID.Sets.SortingPriorityPainting[Item2.type] - ItemID.Sets.SortingPriorityPainting[Item1.type];
            if (result == 0)
            {
                result = Item1.paint - Item2.paint;
            }
            return result;
        }

        private static int CompareWiring(Item Item1, Item Item2)
        {
            int result = ItemID.Sets.SortingPriorityWiring[Item2.type] - ItemID.Sets.SortingPriorityWiring[Item1.type];
            if (result == 0)
            {
                result = CompareRarity(Item1, Item2);
            }
            return result;
        }

        private static int CompareMaterial(Item Item1, Item Item2)
        {
            return ItemID.Sets.SortingPriorityMaterials[Item2.type] - ItemID.Sets.SortingPriorityMaterials[Item1.type];
        }

        private static int CompareRope(Item Item1, Item Item2)
        {
            return ItemID.Sets.SortingPriorityRopes[Item2.type] - ItemID.Sets.SortingPriorityRopes[Item1.type];
        }

        private static int CompareExtractible(Item Item1, Item Item2)
        {
            return ItemID.Sets.SortingPriorityExtractibles[Item2.type] - ItemID.Sets.SortingPriorityExtractibles[Item1.type];
        }

        private static int CompareMisc(Item Item1, Item Item2)
        {
            int result = CompareRarity(Item1, Item2);
            if (result == 0)
            {
                result = Item2.value - Item1.value;
            }
            return result;
        }

        private static int CompareName(Item Item1, Item Item2)
        {
            return string.Compare(Item1.Name, Item2.Name, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class DefaultSortClass
    {
        private Func<Item, bool> passFunc;
        private Func<Item, Item, int> compareFunc;

        public DefaultSortClass(Func<Item, bool> passFunc, Func<Item, Item, int> compareFunc)
        {
            this.passFunc = passFunc;
            this.compareFunc = compareFunc;
        }

        public bool Pass(Item Item)
        {
            return passFunc(Item);
        }

        public int Compare(Item Item1, Item Item2)
        {
            return compareFunc(Item1, Item2);
        }
    }
}