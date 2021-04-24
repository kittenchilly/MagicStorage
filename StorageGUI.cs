using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using MagicStorage.Components;
using MagicStorage.Sorting;

namespace MagicStorage
{
    public static class StorageGUI
    {
        private const int padding = 4;
        private const int numColumns = 10;
        public const float inventoryScale = 0.85f;

        public static MouseState curMouse;
        public static MouseState oldMouse;
        public static bool MouseClicked
        {
            get
            {
                return curMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released;
            }
        }

        private static UIPanel basePanel;
        private static float panelTop;
        private static float panelLeft;
        private static float panelWidth;
        private static float panelHeight;

        private static UIElement topBar;
        internal static UISearchBar searchBar;
        private static UIButtonChoice sortButtons;
        internal static UITextPanel<LocalizedText> depositButton;
        private static UIElement topBar2;
        private static UIButtonChoice filterButtons;
        internal static UISearchBar searchBar2;

        private static UISlotZone slotZone = new UISlotZone(HoverItemSlot, GetItem, inventoryScale);
        private static int slotFocus = -1;
        private static int rightClickTimer = 0;
        private const int startMaxRightClickTimer = 20;
        private static int maxRightClickTimer = startMaxRightClickTimer;

        internal static UIScrollbar scrollBar = new UIScrollbar();
        private static bool scrollBarFocus = false;
        private static int scrollBarFocusMouseStart;
        private static float scrollBarFocusPositionStart;
        private static float scrollBarViewSize = 1f;
        private static float scrollBarMaxViewSize = 2f;

        private static List<Item> Items = new List<Item>();
        private static List<bool> didMatCheck = new List<bool>();
        private static int numRows;
        private static int displayRows;

        private static UIElement bottomBar = new UIElement();
        private static UIText capacityText;

        public static void Initialize()
        {
            InitLangStuff();
            float ItemSlotWidth = TextureAssets.InventoryBack.Width() * inventoryScale;
            float ItemSlotHeight = TextureAssets.InventoryBack.Height() * inventoryScale;

            panelTop = Main.instance.invBottom + 60;
            panelLeft = 20f;
            basePanel = new UIPanel();
            float innerPanelLeft = panelLeft + basePanel.PaddingLeft;
            float innerPanelWidth = numColumns * (ItemSlotWidth + padding) + 20f + padding;
            panelWidth = basePanel.PaddingLeft + innerPanelWidth + basePanel.PaddingRight;
            panelHeight = Main.screenHeight - panelTop - 40f;
            basePanel.Left.Set(panelLeft, 0f);
            basePanel.Top.Set(panelTop, 0f);
            basePanel.Width.Set(panelWidth, 0f);
            basePanel.Height.Set(panelHeight, 0f);
            basePanel.Recalculate();

            topBar = new UIElement();
            topBar.Width.Set(0f, 1f);
            topBar.Height.Set(32f, 0f);
            basePanel.Append(topBar);

            InitSortButtons();
            topBar.Append(sortButtons);

            depositButton.Left.Set(sortButtons.GetDimensions().Width + 2 * padding, 0f);
            depositButton.Width.Set(128f, 0f);
            depositButton.Height.Set(-2 * padding, 1f);
            depositButton.PaddingTop = 8f;
            depositButton.PaddingBottom = 8f;
            topBar.Append(depositButton);

            float depositButtonRight = sortButtons.GetDimensions().Width + 2 * padding + depositButton.GetDimensions().Width;
            searchBar.Left.Set(depositButtonRight + padding, 0f);
            searchBar.Width.Set(-depositButtonRight - 2 * padding, 1f);
            searchBar.Height.Set(0f, 1f);
            topBar.Append(searchBar);

            topBar2 = new UIElement();
            topBar2.Width.Set(0f, 1f);
            topBar2.Height.Set(32f, 0f);
            topBar2.Top.Set(36f, 0f);
            basePanel.Append(topBar2);

            InitFilterButtons();
            topBar2.Append(filterButtons);
            searchBar2.Left.Set(depositButtonRight + padding, 0f);
            searchBar2.Width.Set(-depositButtonRight - 2 * padding, 1f);
            searchBar2.Height.Set(0f, 1f);
            topBar2.Append(searchBar2);

            slotZone.Width.Set(0f, 1f);
            slotZone.Top.Set(76f, 0f);
            slotZone.Height.Set(-116f, 1f);
            basePanel.Append(slotZone);

            numRows = (Items.Count + numColumns - 1) / numColumns;
            displayRows = (int)slotZone.GetDimensions().Height / ((int)ItemSlotHeight + padding);
            slotZone.SetDimensions(numColumns, displayRows);
            int noDisplayRows = numRows - displayRows;
            if (noDisplayRows < 0)
            {
                noDisplayRows = 0;
            }
            scrollBarMaxViewSize = 1 + noDisplayRows;
            scrollBar.Height.Set(displayRows * (ItemSlotHeight + padding), 0f);
            scrollBar.Left.Set(-20f, 1f);
            scrollBar.SetView(scrollBarViewSize, scrollBarMaxViewSize);
            slotZone.Append(scrollBar);

            bottomBar.Width.Set(0f, 1f);
            bottomBar.Height.Set(32f, 0f);
            bottomBar.Top.Set(-32f, 1f);
            basePanel.Append(bottomBar);

            capacityText.Left.Set(6f, 0f);
            capacityText.Top.Set(6f, 0f);
            TEStorageHeart heart = GetHeart();
            int numItems = 0;
            int capacity = 0;
            if (heart != null)
            {
                foreach (TEAbstractStorageUnit abstractStorageUnit in heart.GetStorageUnits())
                {
                    if (abstractStorageUnit is TEStorageUnit)
                    {
                        TEStorageUnit storageUnit = (TEStorageUnit)abstractStorageUnit;
                        numItems += storageUnit.NumItems;
                        capacity += storageUnit.Capacity;
                    }
                }
            }
            capacityText.SetText(numItems + "/" + capacity + " Items");
            bottomBar.Append(capacityText);
        }

        private static void InitLangStuff()
        {
            if (depositButton == null)
            {
                depositButton = new UITextPanel<LocalizedText>(Language.GetText("Mods.MagicStorage.DepositAll"), 1f);
            }
            if (searchBar == null)
            {
                searchBar = new UISearchBar(Language.GetText("Mods.MagicStorage.SearchName"));
            }
            if (searchBar2 == null)
            {
                searchBar2 = new UISearchBar(Language.GetText("Mods.MagicStorage.SearchMod"));
            }
            if (capacityText == null)
            {
                capacityText = new UIText("Items");
            }
        }

        internal static void Unload()
        {
            sortButtons = null;
            filterButtons = null;
        }

        private static void InitSortButtons()
        {
            if (sortButtons == null)
            {
                sortButtons = new UIButtonChoice(new Texture2D[]
                {
                    TextureAssets.InventorySort[0].Value,
                    ModContent.GetTexture("MagicStorage/SortID").Value,
                    ModContent.GetTexture("MagicStorage/SortName").Value,
                    ModContent.GetTexture("MagicStorage/SortNumber").Value
                },
                new LocalizedText[]
                {
                    Language.GetText("Mods.MagicStorage.SortDefault"),
                    Language.GetText("Mods.MagicStorage.SortID"),
                    Language.GetText("Mods.MagicStorage.SortName"),
                    Language.GetText("Mods.MagicStorage.SortStack")
                });
            }
        }

        private static void InitFilterButtons()
        {
            if (filterButtons == null)
            {
                filterButtons = new UIButtonChoice(new Texture2D[]
                {
                    ModContent.GetTexture("MagicStorage/FilterAll").Value,
                    ModContent.GetTexture("MagicStorage/FilterMelee").Value,
                    ModContent.GetTexture("MagicStorage/FilterPickaxe").Value,
                    ModContent.GetTexture("MagicStorage/FilterArmor").Value,
                    ModContent.GetTexture("MagicStorage/FilterPotion").Value,
                    ModContent.GetTexture("MagicStorage/FilterTile").Value,
                    ModContent.GetTexture("MagicStorage/FilterMisc").Value,
                },
                new LocalizedText[]
                {
                    Language.GetText("Mods.MagicStorage.FilterAll"),
                    Language.GetText("Mods.MagicStorage.FilterWeapons"),
                    Language.GetText("Mods.MagicStorage.FilterTools"),
                    Language.GetText("Mods.MagicStorage.FilterEquips"),
                    Language.GetText("Mods.MagicStorage.FilterPotions"),
                    Language.GetText("Mods.MagicStorage.FilterTiles"),
                    Language.GetText("Mods.MagicStorage.FilterMisc")
                });
            }
        }

        public static void Update(GameTime gameTime)
        {
            oldMouse = curMouse;
            curMouse = Mouse.GetState();
            if (Main.playerInventory && Main.player[Main.myPlayer].GetModPlayer<StoragePlayer>().ViewingStorage().X >= 0 && !StoragePlayer.IsStorageCrafting())
            {
                if (StorageGUI.curMouse.RightButton == ButtonState.Released)
                {
                    ResetSlotFocus();
                }
                if(basePanel != null)
                    basePanel.Update(gameTime);
                UpdateScrollBar();
                UpdateDepositButton();
            }
            else
            {
                scrollBarFocus = false;
                scrollBar.ViewPosition = 0f;
                ResetSlotFocus();
            }
        }

        public static void Draw(TEStorageHeart heart)
        {
            Player player = Main.player[Main.myPlayer];
            StoragePlayer ModPlayer = player.GetModPlayer<StoragePlayer>();
            Initialize();
            if (Main.mouseX > panelLeft && Main.mouseX < panelLeft + panelWidth && Main.mouseY > panelTop && Main.mouseY < panelTop + panelHeight)
            {
                player.mouseInterface = true;
                player.cursorItemIconEnabled = false;
                Main.ItemIconCacheVerification();
            }
            basePanel.Draw(Main.spriteBatch);
            slotZone.DrawText();
            sortButtons.DrawText();
            filterButtons.DrawText();
        }

        private static Item GetItem(int slot, ref int context)
        {
            int index = slot + numColumns * (int)Math.Round(scrollBar.ViewPosition);
            Item Item = index < Items.Count ? Items[index] : new Item();
            if (!Item.IsAir && !didMatCheck[index])
            {
                Item.checkMat();
                didMatCheck[index] = true;
            }
            return Item;
        }

        private static void UpdateScrollBar()
        {
            if (slotFocus >= 0)
            {
                scrollBarFocus = false;
                return;
            }
            Rectangle dim = scrollBar.GetClippingRectangle(Main.spriteBatch);
            Vector2 boxPos = new Vector2(dim.X, dim.Y + dim.Height * (scrollBar.ViewPosition / scrollBarMaxViewSize));
            float boxWidth = 20f * Main.UIScale;
            float boxHeight = dim.Height * (scrollBarViewSize / scrollBarMaxViewSize);
            if (scrollBarFocus)
            {
                if (curMouse.LeftButton == ButtonState.Released)
                {
                    scrollBarFocus = false;
                }
                else
                {
                    int difference = curMouse.Y - scrollBarFocusMouseStart;
                    scrollBar.ViewPosition = scrollBarFocusPositionStart + (float)difference / boxHeight;
                }
            }
            else if (MouseClicked)
            {
                if (curMouse.X > boxPos.X && curMouse.X < boxPos.X + boxWidth && curMouse.Y > boxPos.Y - 3f && curMouse.Y < boxPos.Y + boxHeight + 4f)
                {
                    scrollBarFocus = true;
                    scrollBarFocusMouseStart = curMouse.Y;
                    scrollBarFocusPositionStart = scrollBar.ViewPosition;
                }
            }
            if (!scrollBarFocus)
            {
                int difference = oldMouse.ScrollWheelValue / 250 - curMouse.ScrollWheelValue / 250;
                scrollBar.ViewPosition += difference;
            }
        }

        private static TEStorageHeart GetHeart()
        {
            Player player = Main.player[Main.myPlayer];
            StoragePlayer ModPlayer = player.GetModPlayer<StoragePlayer>();
            return ModPlayer.GetStorageHeart();
        }

        public static void RefreshItems()
        {
            if (StoragePlayer.IsStorageCrafting())
            {
                CraftingGUI.RefreshItems();
                return;
            }
            Items.Clear();
            didMatCheck.Clear();
            TEStorageHeart heart = GetHeart();
            if (heart == null)
            {
                return;
            }
            InitLangStuff();
            InitSortButtons();
            InitFilterButtons();
            SortMode sortMode = (SortMode)sortButtons.Choice;
            FilterMode filterMode = (FilterMode)filterButtons.Choice;

            Items.AddRange(ItemSorter.SortAndFilter(heart.GetStoredItems(), sortMode, filterMode, searchBar2.Text, searchBar.Text));
            for (int k = 0; k < Items.Count; k++)
            {
                didMatCheck.Add(false);
            }
        }

        private static void UpdateDepositButton()
        {
            Rectangle dim = InterfaceHelper.GetFullRectangle(depositButton);
            if (curMouse.X > dim.X && curMouse.X < dim.X + dim.Width && curMouse.Y > dim.Y && curMouse.Y < dim.Y + dim.Height)
            {
                depositButton.BackgroundColor = new Color(73, 94, 171);
                if (MouseClicked)
                {
                    if (TryDepositAll())
                    {
                        RefreshItems();
                        Terraria.Audio.SoundEngine.PlaySound(7, -1, -1, 1);
                    }
                }
            }
            else
            {
                depositButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            }
        }

        private static void ResetSlotFocus()
        {
            slotFocus = -1;
            rightClickTimer = 0;
            maxRightClickTimer = startMaxRightClickTimer;
        }

        private static void HoverItemSlot(int slot, ref int hoverSlot)
        {
            Player player = Main.player[Main.myPlayer];
            int visualSlot = slot;
            slot += numColumns * (int)Math.Round(scrollBar.ViewPosition);
            if (MouseClicked)
            {
                bool changed = false;
                if (!Main.mouseItem.IsAir && (player.itemAnimation == 0 && player.itemTime == 0))
                {
                    if (TryDeposit(Main.mouseItem))
                    {
                        changed = true;
                    }
                }
                else if (Main.mouseItem.IsAir && slot < Items.Count && !Items[slot].IsAir)
                {
                    Item toWithdraw = Items[slot].Clone();
                    if (toWithdraw.stack > toWithdraw.maxStack)
                    {
                        toWithdraw.stack = toWithdraw.maxStack;
                    }
                    Main.mouseItem = DoWithdraw(toWithdraw, ItemSlot.ShiftInUse);
                    if (ItemSlot.ShiftInUse)
                    {
                        Main.mouseItem = player.GetItem(Main.myPlayer, Main.mouseItem, GetItemSettings.InventoryEntityToPlayerInventorySettings);
                    }
                    changed = true;
                }
                if (changed)
                {
                    RefreshItems();
                    Terraria.Audio.SoundEngine.PlaySound(7, -1, -1, 1);
                }
            }

            if (curMouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released && slot < Items.Count && (Main.mouseItem.IsAir || ItemData.Matches(Main.mouseItem, Items[slot]) && Main.mouseItem.stack < Main.mouseItem.maxStack))
            {
                slotFocus = slot;
            }
            
            if (slot < Items.Count && !Items[slot].IsAir)
            {
                hoverSlot = visualSlot;
            }

            if (slotFocus >= 0)
            {
                SlotFocusLogic();
            }
        }

        private static void SlotFocusLogic()
        {
            if (slotFocus >= Items.Count || (!Main.mouseItem.IsAir && (!ItemData.Matches(Main.mouseItem, Items[slotFocus]) || Main.mouseItem.stack >= Main.mouseItem.maxStack)))
            {
                ResetSlotFocus();
            }
            else
            {
                if (rightClickTimer <= 0)
                {
                    rightClickTimer = maxRightClickTimer;
                    maxRightClickTimer = maxRightClickTimer * 3 / 4;
                    if (maxRightClickTimer <= 0)
                    {
                        maxRightClickTimer = 1;
                    }
                    Item toWithdraw = Items[slotFocus].Clone();
                    toWithdraw.stack = 1;
                    Item result = DoWithdraw(toWithdraw);
                    if (Main.mouseItem.IsAir)
                    {
                        Main.mouseItem = result;
                    }
                    else
                    {
                        Main.mouseItem.stack += result.stack;
                    }
                    //Main.soundInstanceMenuTick.Stop();
                    //Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                    Terraria.Audio.SoundEngine.PlaySound(12, -1, -1, 1);
                    RefreshItems();
                }
                rightClickTimer--;
            }
        }

        private static bool TryDeposit(Item Item)
        {
            int oldStack = Item.stack;
            DoDeposit(Item);
            return oldStack != Item.stack;
        }

        private static void DoDeposit(Item Item)
        {
            TEStorageHeart heart = GetHeart();
            if (Main.netMode == 0)
            {
                heart.DepositItem(Item);
            }
            else
            {
                NetHelper.SendDeposit(heart.ID, Item);
                Item.SetDefaults(0, true);
            }
        }

        private static bool TryDepositAll()
        {
            Player player = Main.player[Main.myPlayer];
            TEStorageHeart heart = GetHeart();
            bool changed = false;
            if (Main.netMode == 0)
            {
                for (int k = 10; k < 50; k++)
                {
                    if (!player.inventory[k].IsAir && !player.inventory[k].favorited)
                    {
                        int oldStack = player.inventory[k].stack;
                        heart.DepositItem(player.inventory[k]);
                        if (oldStack != player.inventory[k].stack)
                        {
                            changed = true;
                        }
                    }
                }
            }
            else
            {
                List<Item> Items = new List<Item>();
                for (int k = 10; k < 50; k++)
                {
                    if (!player.inventory[k].IsAir && !player.inventory[k].favorited)
                    {
                        Items.Add(player.inventory[k]);
                    }
                }
                NetHelper.SendDepositAll(heart.ID, Items);
                foreach (Item Item in Items)
                {
                    Item.SetDefaults(0, true);
                }
                changed = true;
            }
            return changed;
        }

        private static Item DoWithdraw(Item Item, bool toInventory = false)
        {
            TEStorageHeart heart = GetHeart();
            if (Main.netMode == 0)
            {
                return heart.TryWithdraw(Item);
            }
            else
            {
                NetHelper.SendWithdraw(heart.ID, Item, toInventory);
                return new Item();
            }
        }
    }
}