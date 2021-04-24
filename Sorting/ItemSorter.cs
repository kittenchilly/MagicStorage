using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace MagicStorage.Sorting
{
    public static class ItemSorter
    {
        public static IEnumerable<Item> SortAndFilter(IEnumerable<Item> Items, SortMode sortMode, FilterMode filterMode, string ModFilter, string nameFilter)
        {
            ItemFilter filter;
            switch (filterMode)
            {
            case FilterMode.All:
                filter = new FilterAll();
                break;
            case FilterMode.Weapons:
                filter = new FilterWeapon();
                break;
            case FilterMode.Tools:
                filter = new FilterTool();
                break;
            case FilterMode.Equipment:
                filter = new FilterEquipment();
                break;
            case FilterMode.Potions:
                filter = new FilterPotion();
                break;
            case FilterMode.Placeables:
                filter = new FilterPlaceable();
                break;
            case FilterMode.Misc:
                filter = new FilterMisc();
                break;
            default:
                filter = new FilterAll();
                break;
            }
            IEnumerable<Item> filteredItems = Items.Where((Item) => filter.Passes(Item) && FilterName(Item, ModFilter, nameFilter));
            CompareFunction func;
            switch (sortMode)
            {
            case SortMode.Default:
                func = new CompareDefault();
                break;
            case SortMode.Id:
                func = new CompareID();
                break;
            case SortMode.Name:
                func = new CompareName();
                break;
            case SortMode.Quantity:
                func = new CompareID();
                break;
            default:
                return filteredItems;
            }
            BTree<Item> sortedTree = new BTree<Item>(func);
            foreach (Item Item in filteredItems)
            {
                sortedTree.Insert(Item);
            }
            if (sortMode == SortMode.Quantity)
            {
                BTree<Item> oldTree = sortedTree;
                sortedTree = new BTree<Item>(new CompareQuantity());
                foreach (Item Item in oldTree.GetSortedItems())
                {
                    sortedTree.Insert(Item);
                }
            }
            return sortedTree.GetSortedItems();
        }

        public static IEnumerable<Recipe> GetRecipes(SortMode sortMode, FilterMode filterMode, string ModFilter, string nameFilter)
        {
            ItemFilter filter;
            switch (filterMode)
            {
            case FilterMode.All:
                filter = new FilterAll();
                break;
            case FilterMode.Weapons:
                filter = new FilterWeapon();
                break;
            case FilterMode.Tools:
                filter = new FilterTool();
                break;
            case FilterMode.Equipment:
                filter = new FilterEquipment();
                break;
            case FilterMode.Potions:
                filter = new FilterPotion();
                break;
            case FilterMode.Placeables:
                filter = new FilterPlaceable();
                break;
            case FilterMode.Misc:
                filter = new FilterMisc();
                break;
            default:
                filter = new FilterAll();
                break;
            }
            IEnumerable<Recipe> filteredRecipes = Main.recipe.Where((recipe, index) => index < Recipe.numRecipes && filter.Passes(recipe) && FilterName(recipe.createItem, ModFilter, nameFilter));
            CompareFunction func;
            switch (sortMode)
            {
            case SortMode.Default:
                func = new CompareDefault();
                break;
            case SortMode.Id:
                func = new CompareID();
                break;
            case SortMode.Name:
                func = new CompareName();
                break;
            default:
                return filteredRecipes;
            }
            BTree<Recipe> sortedTree = new BTree<Recipe>(func);
            foreach (Recipe recipe in filteredRecipes)
            {
                sortedTree.Insert(recipe);
                if (CraftingGUI.threadNeedsRestart)
                {
                    return new List<Recipe>();
                }
            }
            return sortedTree.GetSortedItems();
        }

        private static bool FilterName(Item Item, string ModFilter, string filter)
        {
            string ModName = "Terraria";
            if (Item.ModItem != null)
            {
                ModName = Item.ModItem.Mod.DisplayName;
            }
            return ModName.ToLowerInvariant().IndexOf(ModFilter.ToLowerInvariant()) >= 0 && Item.Name.ToLowerInvariant().IndexOf(filter.ToLowerInvariant()) >= 0;
        }
    }
}
