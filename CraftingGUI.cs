using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using MagicStorage.Components;
using MagicStorage.Sorting;

namespace MagicStorage
{
    public static class CraftingGUI
    {
        private const int padding = 4;
        private const int numColumns = 10;
        private const int numColumns2 = 7;
        private const float inventoryScale = 0.85f;
        private const float smallScale = 0.7f;

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
        private static UIButtonChoice recipeButtons;
        private static UIElement topBar2;
        private static UIButtonChoice filterButtons;
        internal static UISearchBar searchBar2;

        private static UIText stationText;
        private static UISlotZone stationZone = new UISlotZone(HoverStation, GetStation, inventoryScale);
        private static UIText recipeText;
        private static UISlotZone recipeZone = new UISlotZone(HoverRecipe, GetRecipe, inventoryScale);

        internal static UIScrollbar scrollBar = new UIScrollbar();
        private static int scrollBarFocus = 0;
        private static int scrollBarFocusMouseStart;
        private static float scrollBarFocusPositionStart;
        private static float scrollBarViewSize = 1f;
        private static float scrollBarMaxViewSize = 2f;

        private static List<Item> Items = new List<Item>();
        private static Dictionary<int, int> ItemCounts = new Dictionary<int, int>();
        private static bool[] adjTiles = new bool[TileLoader.TileCount];
        private static bool adjWater = false;
        private static bool adjLava = false;
        private static bool adjHoney = false;
        private static bool zoneSnow = false;
        private static bool alchemyTable = false;
        private static List<Recipe> recipes = new List<Recipe>();
        private static List<bool> recipeAvailable = new List<bool>();
        private static Recipe selectedRecipe = null;
        private static int numRows;
        private static int displayRows;
        private static bool slotFocus = false;

        private static UIElement bottomBar = new UIElement();
        private static UIText capacityText;

        private static UIPanel recipePanel;
        private static float recipeTop;
        private static float recipeLeft;
        private static float recipeWidth;
        private static float recipeHeight;

        private static UIText recipePanelHeader;
        private static UIText ingredientText;
        private static UISlotZone ingredientZone = new UISlotZone(HoverItem, GetIngredient, smallScale);
        private static UIText reqObjText;
        private static UIText reqObjText2;
        private static UIText storedItemsText;

        private static UISlotZone storageZone = new UISlotZone(HoverStorage, GetStorage, smallScale);
        private static int numRows2;
        private static int displayRows2;
        private static List<Item> storageItems = new List<Item>();
        private static List<ItemData> blockStorageItems = new List<ItemData>();

        internal static UIScrollbar scrollBar2 = new UIScrollbar();
        private static float scrollBar2ViewSize = 1f;
        private static float scrollBar2MaxViewSize = 2f;

        internal static UITextPanel<LocalizedText> craftButton;
        private static Item result = null;
        private static UISlotZone resultZone = new UISlotZone(HoverResult, GetResult, inventoryScale);
        private static int craftTimer = 0;
        private const int startMaxCraftTimer = 20;
        private static int maxCraftTimer = startMaxCraftTimer;
        private static int rightClickTimer = 0;
        private const int startMaxRightClickTimer = 20;
        private static int maxRightClickTimer = startMaxRightClickTimer;

        private static Object threadLock = new Object();
        private static Object recipeLock = new Object();
        private static Object ItemsLock = new Object();
        private static bool threadRunning = false;
        internal static bool threadNeedsRestart = false;
        private static SortMode threadSortMode;
        private static FilterMode threadFilterMode;
        private static List<Recipe> threadRecipes = new List<Recipe>();
        private static List<bool> threadRecipeAvailable = new List<bool>();
        private static List<Recipe> nextRecipes = new List<Recipe>();
        private static List<bool> nextRecipeAvailable = new List<bool>();
        public static void Initialize()
        {
            lock (recipeLock)
            {
                recipes = nextRecipes;
                recipeAvailable = nextRecipeAvailable;
            }

            InitLangStuff();
            float ItemSlotWidth = TextureAssets.InventoryBack.Width() * inventoryScale;
            float ItemSlotHeight = TextureAssets.InventoryBack.Height() * inventoryScale;
            float smallSlotWidth = TextureAssets.InventoryBack.Width() * smallScale;
            float smallSlotHeight = TextureAssets.InventoryBack.Height() * smallScale;

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

            recipePanel = new UIPanel();
            recipeTop = panelTop;
            recipeLeft = panelLeft + panelWidth;
            recipeWidth = numColumns2 * (smallSlotWidth + padding) + 20f + padding;
            recipeWidth += recipePanel.PaddingLeft + recipePanel.PaddingRight;
            recipeHeight = panelHeight;
            recipePanel.Left.Set(recipeLeft, 0f);
            recipePanel.Top.Set(recipeTop, 0f);
            recipePanel.Width.Set(recipeWidth, 0f);
            recipePanel.Height.Set(recipeHeight, 0f);
            recipePanel.Recalculate();

            topBar = new UIElement();
            topBar.Width.Set(0f, 1f);
            topBar.Height.Set(32f, 0f);
            basePanel.Append(topBar);

            InitSortButtons();
            topBar.Append(sortButtons);
            float sortButtonsRight = sortButtons.GetDimensions().Width + padding;
            InitRecipeButtons();
            float recipeButtonsLeft = sortButtonsRight + 32f + 3 * padding;
            recipeButtons.Left.Set(recipeButtonsLeft, 0f);
            topBar.Append(recipeButtons);
            float recipeButtonsRight = recipeButtonsLeft + recipeButtons.GetDimensions().Width + padding;

            searchBar.Left.Set(recipeButtonsRight + padding, 0f);
            searchBar.Width.Set(-recipeButtonsRight - 2 * padding, 1f);
            searchBar.Height.Set(0f, 1f);
            topBar.Append(searchBar);

            topBar2 = new UIElement();
            topBar2.Width.Set(0f, 1f);
            topBar2.Height.Set(32f, 0f);
            topBar2.Top.Set(36f, 0f);
            basePanel.Append(topBar2);

            InitFilterButtons();
            float filterButtonsRight = filterButtons.GetDimensions().Width + padding;
            topBar2.Append(filterButtons);
            searchBar2.Left.Set(filterButtonsRight + padding, 0f);
            searchBar2.Width.Set(-filterButtonsRight - 2 * padding, 1f);
            searchBar2.Height.Set(0f, 1f);
            topBar2.Append(searchBar2);

            stationText.Top.Set(76f, 0f);
            basePanel.Append(stationText);

            stationZone.Width.Set(0f, 1f);
            stationZone.Top.Set(100f, 0f);
            stationZone.Height.Set(70f, 0f);
            stationZone.SetDimensions(numColumns, 1);
            basePanel.Append(stationZone);

            recipeText.Top.Set(152f, 0f);
            basePanel.Append(recipeText);

            recipeZone.Width.Set(0f, 1f);
            recipeZone.Top.Set(176f, 0f);
            recipeZone.Height.Set(-216f, 1f);
            basePanel.Append(recipeZone);

            numRows = (recipes.Count + numColumns - 1) / numColumns;
            displayRows = (int)recipeZone.GetDimensions().Height / ((int)ItemSlotHeight + padding);
            recipeZone.SetDimensions(numColumns, displayRows);
            int noDisplayRows = numRows - displayRows;
            if (noDisplayRows < 0)
            {
                noDisplayRows = 0;
            }
            scrollBarMaxViewSize = 1 + noDisplayRows;
            scrollBar.Height.Set(displayRows * (ItemSlotHeight + padding), 0f);
            scrollBar.Left.Set(-20f, 1f);
            scrollBar.SetView(scrollBarViewSize, scrollBarMaxViewSize);
            recipeZone.Append(scrollBar);

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

            recipePanel.Append(recipePanelHeader);
            ingredientText.Top.Set(30f, 0f);
            recipePanel.Append(ingredientText);

            ingredientZone.SetDimensions(numColumns2, 2);
            ingredientZone.Top.Set(54f, 0f);
            ingredientZone.Width.Set(0f, 1f);
            ingredientZone.Height.Set(60f, 0f);
            recipePanel.Append(ingredientZone);

            reqObjText.Top.Set(136f, 0f);
            recipePanel.Append(reqObjText);
            reqObjText2.Top.Set(160f, 0f);
            recipePanel.Append(reqObjText2);
            storedItemsText.Top.Set(190f, 0f);
            recipePanel.Append(storedItemsText);

            storageZone.Top.Set(214f, 0f);
            storageZone.Width.Set(0f, 1f);
            storageZone.Height.Set(-214f - 36, 1f);
            recipePanel.Append(storageZone);
            numRows2 = (storageItems.Count + numColumns2 - 1) / numColumns2;
            displayRows2 = (int)storageZone.GetDimensions().Height / ((int)smallSlotHeight + padding);
            storageZone.SetDimensions(numColumns2, displayRows2);
            int noDisplayRows2 = numRows2 - displayRows2;
            if (noDisplayRows2 < 0)
            {
                noDisplayRows2 = 0;
            }
            scrollBar2MaxViewSize = 1 + noDisplayRows2;
            scrollBar2.Height.Set(displayRows2 * (smallSlotHeight + padding), 0f);
            scrollBar2.Left.Set(-20f, 1f);
            scrollBar2.SetView(scrollBar2ViewSize, scrollBar2MaxViewSize);
            storageZone.Append(scrollBar2);

            craftButton.Top.Set(-32f, 1f);
            craftButton.Width.Set(100f, 0f);
            craftButton.Height.Set(24f, 0f);
            craftButton.PaddingTop = 8f;
            craftButton.PaddingBottom = 8f;
            recipePanel.Append(craftButton);

            resultZone.SetDimensions(1, 1);
            resultZone.Left.Set(-ItemSlotWidth, 1f);
            resultZone.Top.Set(-ItemSlotHeight, 1f);
            resultZone.Width.Set(ItemSlotWidth, 0f);
            resultZone.Height.Set(ItemSlotHeight, 0f);
            recipePanel.Append(resultZone);
        }

        private static void InitLangStuff()
        {
            if (searchBar == null)
            {
                searchBar = new UISearchBar(Language.GetText("Mods.MagicStorage.SearchName"));
            }
            if (searchBar2 == null)
            {
                searchBar2 = new UISearchBar(Language.GetText("Mods.MagicStorage.SearchMod"));
            }
            if (stationText == null)
            {
                stationText = new UIText(Language.GetText("Mods.MagicStorage.CraftingStations"));
            }
            if (recipeText == null)
            {
                recipeText = new UIText(Language.GetText("Mods.MagicStorage.Recipes"));
            }
            if (capacityText == null)
            {
                capacityText = new UIText("Items");
            }
            if (recipePanelHeader == null)
            {
                recipePanelHeader = new UIText(Language.GetText("Mods.MagicStorage.SelectedRecipe"));
            }
            if (ingredientText == null)
            {
                ingredientText = new UIText(Language.GetText("Mods.MagicStorage.Ingredients"));
            }
            if (reqObjText == null)
            {
                reqObjText = new UIText(Language.GetText("LegacyInterface.22"));
            }
            if (reqObjText2 == null)
            {
                reqObjText2 = new UIText("");
            }
            if (storedItemsText == null)
            {
                storedItemsText = new UIText(Language.GetText("Mods.MagicStorage.StoredItems"));
            }
            if (craftButton == null)
            {
                craftButton = new UITextPanel<LocalizedText>(Language.GetText("LegacyMisc.72"), 1f);
            }
        }

        internal static void Unload()
        {
            sortButtons = null;
            filterButtons = null;
            recipeButtons = null;
        }

        private static void InitSortButtons()
        {
            if (sortButtons == null)
            {
                sortButtons = new UIButtonChoice(new Texture2D[]
                {
                    TextureAssets.InventorySort[0].Value,
                    ModContent.GetTexture("MagicStorage/SortID").Value,
                    ModContent.GetTexture("MagicStorage/SortName").Value
                },
                new LocalizedText[]
                {
                    Language.GetText("Mods.MagicStorage.SortDefault"),
                    Language.GetText("Mods.MagicStorage.SortID"),
                    Language.GetText("Mods.MagicStorage.SortName")
                });
            }
        }

        private static void InitRecipeButtons()
        {
            if (recipeButtons == null)
            {
                recipeButtons = new UIButtonChoice(new Texture2D[]
                {
                    ModContent.GetTexture("MagicStorage/RecipeAvailable").Value,
                    ModContent.GetTexture("MagicStorage/RecipeAll").Value
                },
                new LocalizedText[]
                {
                    Language.GetText("Mods.MagicStorage.RecipeAvailable"),
                    Language.GetText("Mods.MagicStorage.RecipeAll")
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
        {try{
            oldMouse = StorageGUI.oldMouse;
            curMouse = StorageGUI.curMouse;
            if (Main.playerInventory && Main.player[Main.myPlayer].GetModPlayer<StoragePlayer>().ViewingStorage().X >= 0 && StoragePlayer.IsStorageCrafting())
            {
                if (curMouse.RightButton == ButtonState.Released)
                {
                    ResetSlotFocus();
                }
                if (basePanel != null)
                {
                    basePanel.Update(gameTime);
                }
                if (recipePanel != null)
                {
                    recipePanel.Update(gameTime);
                }
                UpdateRecipeText();
                UpdateScrollBar();
                UpdateCraftButton();
            }
            else
            {
                scrollBarFocus = 0;
                selectedRecipe = null;
                craftTimer = 0;
                maxCraftTimer = startMaxCraftTimer;
                ResetSlotFocus();
            }}catch(Exception e){Main.NewTextMultiline(e.ToString());}
        }

        public static void Draw(TEStorageHeart heart)
        {try{
            Player player = Main.player[Main.myPlayer];
            StoragePlayer ModPlayer = player.GetModPlayer<StoragePlayer>();
            Initialize();
            if (Main.mouseX > panelLeft && Main.mouseX < recipeLeft + panelWidth && Main.mouseY > panelTop && Main.mouseY < panelTop + panelHeight)
            {
                player.mouseInterface = true;
                player.cursorItemIconEnabled = false;
                    Main.ItemIconCacheVerification();
            }
            basePanel.Draw(Main.spriteBatch);
            recipePanel.Draw(Main.spriteBatch);
            Vector2 pos = recipeZone.GetDimensions().Position();
            if (threadRunning)
            {
                Utils.DrawBorderString(Main.spriteBatch, "Loading", pos + new Vector2(8f, 8f), Color.White);
            }
            stationZone.DrawText();
            recipeZone.DrawText();
            ingredientZone.DrawText();
            storageZone.DrawText();
            resultZone.DrawText();
            sortButtons.DrawText();
            recipeButtons.DrawText();
            filterButtons.DrawText();}catch(Exception e){Main.NewTextMultiline(e.ToString());}
        }

        private static Item GetStation(int slot, ref int context)
        {
            Item[] stations = GetCraftingStations();
            if (stations == null || slot >= stations.Length)
            {
                return new Item();
            }
            return stations[slot];
        }

        private static Item GetRecipe(int slot, ref int context)
        {
            if (threadRunning)
            {
                return new Item();
            }
            int index = slot + numColumns * (int)Math.Round(scrollBar.ViewPosition);
            Item Item = index < recipes.Count ? recipes[index].createItem : new Item();
            if (!Item.IsAir && recipes[index] == selectedRecipe)
            {
                context = 6;
            }
            if (!Item.IsAir && !recipeAvailable[index])
            {
                context = recipes[index] == selectedRecipe ? 4 : 3;
            }
            return Item;
        }

        private static Item GetIngredient(int slot, ref int context)
        {
            if (selectedRecipe == null || slot >= selectedRecipe.requiredItem.Count)
            {
                return new Item();
            }
            Item Item = selectedRecipe.requiredItem[slot].Clone();
            if (selectedRecipe.HasRecipeGroup(RecipeGroupID.Wood) && Item.type == ItemID.Wood)
            {
                Item.SetNameOverride(Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.Wood));
            }
            if (selectedRecipe.HasRecipeGroup(RecipeGroupID.Sand) && Item.type == ItemID.SandBlock)
            {
                Item.SetNameOverride(Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.SandBlock));
            }
            if (selectedRecipe.HasRecipeGroup(RecipeGroupID.IronBar) && Item.type == ItemID.IronBar)
            {
                Item.SetNameOverride(Lang.misc[37].Value + " " + Lang.GetItemNameValue(ItemID.IronBar));
            }
            if (selectedRecipe.HasRecipeGroup(RecipeGroupID.Fragment) && Item.type == ItemID.FragmentSolar)
            {
                Item.SetNameOverride(Lang.misc[37].Value + " " + Lang.misc[51].Value);
            }
            if (selectedRecipe.HasRecipeGroup(RecipeGroupID.PressurePlate) && Item.type == ItemID.GrayPressurePlate)
            {
                Item.SetNameOverride(Lang.misc[37].Value + " " + Lang.misc[38].Value);
            }
            string nameOverride;
            if (ProcessGroupsForText(Item.type, out nameOverride))
            {
                Item.SetNameOverride(nameOverride);
            }
            return Item;
        }

        internal static bool ProcessGroupsForText(int type, out string theText)
        {
            foreach (int num in selectedRecipe.acceptedGroups)
            {
                if (RecipeGroup.recipeGroups[num].ContainsItem(type))
                {
                    theText = RecipeGroup.recipeGroups[num].GetText();
                    return true;
                }
            }

            theText = "";
            return false;
        }

        private static Item GetStorage(int slot, ref int context)
        {
            int index = slot + numColumns2 * (int)Math.Round(scrollBar2.ViewPosition);
            Item Item = index < storageItems.Count ? storageItems[index] : new Item();
            if (blockStorageItems.Contains(new ItemData(Item)))
            {
                context = 3;
            }
            return Item;
        }

        private static Item GetResult(int slot, ref int context)
        {
            return slot == 0 && result != null ? result : new Item();
        }

        private static void UpdateRecipeText()
        {
            if (selectedRecipe == null)
            {
                reqObjText2.SetText("");
            }
            else
            {
                bool isEmpty = true;
                string text = "";
                for (int k = 0; k < selectedRecipe.requiredTile.Count; k++)
                {
                    if (selectedRecipe.requiredTile[k] == -1)
                    {
                        break;
                    }
                    if (!isEmpty)
                    {
                        text += ", ";
                    }
                    text += Lang.GetMapObjectName(MapHelper.TileToLookup(selectedRecipe.requiredTile[k], 0));
                    isEmpty = false;
                }
                if (selectedRecipe.HasCondition(Recipe.Condition.NearWater))
                {
                    if (!isEmpty)
                    {
                        text += ", ";
                    }
                    text += Language.GetTextValue("LegacyInterface.53");
                    isEmpty = false;
                }
                if (selectedRecipe.HasCondition(Recipe.Condition.NearHoney))
                {
                    if (!isEmpty)
                    {
                        text += ", ";
                    }
                    text += Language.GetTextValue("LegacyInterface.58");
                    isEmpty = false;
                }
                if (selectedRecipe.HasCondition(Recipe.Condition.NearLava))
                {
                    if (!isEmpty)
                    {
                        text += ", ";
                    }
                    text += Language.GetTextValue("LegacyInterface.56");
                    isEmpty = false;
                }
                if (selectedRecipe.HasCondition(Recipe.Condition.InSnow))
                {
                    if (!isEmpty)
                    {
                        text += ", ";
                    }
                    text += Language.GetTextValue("LegacyInterface.123");
                    isEmpty = false;
                }
                if (isEmpty)
                {
                    text = Language.GetTextValue("LegacyInterface.23");
                }
                reqObjText2.SetText(text);
            }
        }

        private static void UpdateScrollBar()
        {
            if (slotFocus)
            {
                scrollBarFocus = 0;
                return;
            }
            Rectangle dim = scrollBar.GetClippingRectangle(Main.spriteBatch);
            Vector2 boxPos = new Vector2(dim.X, dim.Y + dim.Height * (scrollBar.ViewPosition / scrollBarMaxViewSize));
            float boxWidth = 20f * Main.UIScale;
            float boxHeight = dim.Height * (scrollBarViewSize / scrollBarMaxViewSize);
            Rectangle dim2 = scrollBar2.GetClippingRectangle(Main.spriteBatch);
            Vector2 box2Pos = new Vector2(dim2.X, dim2.Y + dim2.Height * (scrollBar2.ViewPosition / scrollBar2MaxViewSize));
            float box2Height = dim2.Height * (scrollBar2ViewSize / scrollBar2MaxViewSize);
            if (scrollBarFocus > 0)
            {
                if (curMouse.LeftButton == ButtonState.Released)
                {
                    scrollBarFocus = 0;
                }
                else
                {
                    int difference = curMouse.Y - scrollBarFocusMouseStart;
                    if (scrollBarFocus == 1)
                    {
                        scrollBar.ViewPosition = scrollBarFocusPositionStart + (float)difference / boxHeight;
                    }
                    else if (scrollBarFocus == 2)
                    {
                        scrollBar2.ViewPosition = scrollBarFocusPositionStart + (float)difference / box2Height;
                    }
                }
            }
            else if (MouseClicked)
            {
                if (curMouse.X > boxPos.X && curMouse.X < boxPos.X + boxWidth && curMouse.Y > boxPos.Y - 3f && curMouse.Y < boxPos.Y + boxHeight + 4f)
                {
                    scrollBarFocus = 1;
                    scrollBarFocusMouseStart = curMouse.Y;
                    scrollBarFocusPositionStart = scrollBar.ViewPosition;
                }
                else if (curMouse.X > box2Pos.X && curMouse.X < box2Pos.X + boxWidth && curMouse.Y > box2Pos.Y - 3f && curMouse.Y < box2Pos.Y + box2Height + 4f)
                {
                    scrollBarFocus = 2;
                    scrollBarFocusMouseStart = curMouse.Y;
                    scrollBarFocusPositionStart = scrollBar2.ViewPosition;
                }
            }
            if (scrollBarFocus == 0)
            {
                int difference = oldMouse.ScrollWheelValue / 250 - curMouse.ScrollWheelValue / 250;
                scrollBar.ViewPosition += difference;
            }
        }

        private static void UpdateCraftButton()
        {
            Rectangle dim = InterfaceHelper.GetFullRectangle(craftButton);
            bool flag = false;
            if (curMouse.X > dim.X && curMouse.X < dim.X + dim.Width && curMouse.Y > dim.Y && curMouse.Y < dim.Y + dim.Height)
            {
                craftButton.BackgroundColor = new Color(73, 94, 171);
                if (curMouse.LeftButton == ButtonState.Pressed)
                {
                    if (selectedRecipe != null && IsAvailable(selectedRecipe) && PassesBlock(selectedRecipe))
                    {
                        if (craftTimer <= 0)
                        {
                            craftTimer = maxCraftTimer;
                            maxCraftTimer = maxCraftTimer * 3 / 4;
                            if (maxCraftTimer <= 0)
                            {
                                maxCraftTimer = 1;
                            }
                            TryCraft();
                            RefreshItems();
                            Terraria.Audio.SoundEngine.PlaySound(7, -1, -1, 1);
                        }
                        craftTimer--;
                        flag = true;
                    }
                }
            }
            else
            {
                craftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            }
            if (selectedRecipe == null || !IsAvailable(selectedRecipe) || !PassesBlock(selectedRecipe))
            {
                craftButton.BackgroundColor = new Color(30, 40, 100) * 0.7f;
            }
            if (!flag)
            {
                craftTimer = 0;
                maxCraftTimer = startMaxCraftTimer;
            }
        }

        private static TEStorageHeart GetHeart()
        {
            Player player = Main.player[Main.myPlayer];
            StoragePlayer ModPlayer = player.GetModPlayer<StoragePlayer>();
            return ModPlayer.GetStorageHeart();
        }

        private static TECraftingAccess GetCraftingEntity()
        {
            Player player = Main.player[Main.myPlayer];
            StoragePlayer ModPlayer = player.GetModPlayer<StoragePlayer>();
            return ModPlayer.GetCraftingAccess();
        }

        private static Item[] GetCraftingStations()
        {
            TECraftingAccess ent = GetCraftingEntity();
            return ent == null ? null : ent.stations;
        }

        public static void RefreshItems()
        {
            Items.Clear();
            TEStorageHeart heart = GetHeart();
            if (heart == null)
            {
                return;
            }
            Items.AddRange(ItemSorter.SortAndFilter(heart.GetStoredItems(), SortMode.Id, FilterMode.All, "", ""));
            AnalyzeIngredients();
            InitLangStuff();
            InitSortButtons();
            InitRecipeButtons();
            InitFilterButtons();
            SortMode sortMode;
            switch (sortButtons.Choice)
            {
            case 0:
                sortMode = SortMode.Default;
                break;
            case 1:
                sortMode = SortMode.Id;
                break;
            case 2:
                sortMode = SortMode.Name;
                break;
            default:
                sortMode = SortMode.Default;
                break;
            }
            FilterMode filterMode;
            switch (filterButtons.Choice)
            {
            case 0:
                filterMode = FilterMode.All;
                break;
            case 1:
                filterMode = FilterMode.Weapons;
                break;
            case 2:
                filterMode = FilterMode.Tools;
                break;
            case 3:
                filterMode = FilterMode.Equipment;
                break;
            case 4:
                filterMode = FilterMode.Potions;
                break;
            case 5:
                filterMode = FilterMode.Placeables;
                break;
            case 6:
                filterMode = FilterMode.Misc;
                break;
            default:
                filterMode = FilterMode.All;
                break;
            }
            RefreshStorageItems();
            lock (threadLock)
            {
                threadNeedsRestart = true;
                threadSortMode = sortMode;
                threadFilterMode = filterMode;
                if (!threadRunning)
                {
                    threadRunning = true;
                    Thread thread = new Thread(RefreshRecipes);
                    thread.Start();
                }
            }
        }

        private static void RefreshRecipes()
        {
            while (true)
            {try{
                SortMode sortMode;
                FilterMode filterMode;
                lock (threadLock)
                {
                    threadNeedsRestart = false;
                    sortMode = threadSortMode;
                    filterMode = threadFilterMode;
                }
                var temp = ItemSorter.GetRecipes(sortMode, filterMode, searchBar2.Text, searchBar.Text);
                threadRecipes.Clear();
                threadRecipeAvailable.Clear();
                try
                {
                    if (recipeButtons.Choice == 0)
                    {
                        threadRecipes.AddRange(temp.Where(recipe => IsAvailable(recipe)));
                        threadRecipeAvailable.AddRange(threadRecipes.Select(recipe => true));
                    }
                    else
                    {
                        threadRecipes.AddRange(temp);
                        threadRecipeAvailable.AddRange(threadRecipes.Select(recipe => IsAvailable(recipe)));
                    }
                }
                catch (InvalidOperationException)
                {
                }
                catch (KeyNotFoundException)
                {
                }
                lock (recipeLock)
                {
                    nextRecipes = new List<Recipe>();
                    nextRecipeAvailable = new List<bool>();
                    nextRecipes.AddRange(threadRecipes);
                    nextRecipeAvailable.AddRange(threadRecipeAvailable);
                    
                }
                lock (threadLock)
                {
                    if (!threadNeedsRestart)
                    {
                        threadRunning = false;
                        return;
                    }
                }}catch(Exception e){Main.NewTextMultiline(e.ToString());}
            }
        }

        private static void AnalyzeIngredients()
        {
            Player player = Main.player[Main.myPlayer];
            ItemCounts.Clear();
            if (adjTiles.Length != player.adjTile.Length)
            {
                Array.Resize(ref adjTiles, player.adjTile.Length);
            }
            for (int k = 0; k < adjTiles.Length; k++)
            {
                adjTiles[k] = false;
            }
            adjWater = false;
            adjLava = false;
            adjHoney = false;
            zoneSnow = false;
            alchemyTable = false;

            foreach (Item Item in Items)
            {
                if (ItemCounts.ContainsKey(Item.netID))
                {
                    ItemCounts[Item.netID] += Item.stack;
                }
                else
                {
                    ItemCounts[Item.netID] = Item.stack;
                }
            }
            foreach (Item Item in GetCraftingStations())
            {
                if (Item.createTile >= 0)
                {
                    adjTiles[Item.createTile] = true;
                    if (Item.createTile == TileID.GlassKiln || Item.createTile == TileID.Hellforge || Item.createTile == TileID.AdamantiteForge)
                    {
                        adjTiles[TileID.Furnaces] = true;
                    }
                    if (Item.createTile == TileID.AdamantiteForge)
                    {
                        adjTiles[TileID.Hellforge] = true;
                    }
                    if (Item.createTile == TileID.MythrilAnvil)
                    {
                        adjTiles[TileID.Anvils] = true;
                    }
                    if (Item.createTile == TileID.BewitchingTable || Item.createTile == TileID.Tables2)
                    {
                        adjTiles[TileID.Tables] = true;
                    }
                    if (Item.createTile == TileID.AlchemyTable)
                    {
                        adjTiles[TileID.Bottles] = true;
                        adjTiles[TileID.Tables] = true;
                        alchemyTable = true;
                    }
                    bool[] oldAdjTile = player.adjTile;
                    bool oldAdjWater = adjWater;
                    bool oldAdjLava = adjLava;
                    bool oldAdjHoney = adjHoney;
                    bool oldAlchemyTable = alchemyTable;
                    player.adjTile = adjTiles;
                    player.adjWater = false;
                    player.adjLava = false;
                    player.adjHoney = false;
                    player.alchemyTable = false;
                    TileLoader.AdjTiles(player, Item.createTile);
                    if (player.adjWater)
                    {
                        adjWater = true;
                    }
                    if (player.adjLava)
                    {
                        adjLava = true;
                    }
                    if (player.adjHoney)
                    {
                        adjHoney = true;
                    }
                    if (player.alchemyTable)
                    {
                        alchemyTable = true;
                    }
                    player.adjTile = oldAdjTile;
                    player.adjWater = oldAdjWater;
                    player.adjLava = oldAdjLava;
                    player.adjHoney = oldAdjHoney;
                    player.alchemyTable = oldAlchemyTable;
                }
                if (Item.type == ItemID.WaterBucket || Item.type == ItemID.BottomlessBucket)
                {
                    adjWater = true;
                }
                if (Item.type == ItemID.LavaBucket)
                {
                    adjLava = true;
                }
                if (Item.type == ItemID.HoneyBucket)
                {
                    adjHoney = true;
                }
                if (Item.type == ModContent.ItemType<Items.SnowBiomeEmulator>())
                {
                    zoneSnow = true;
                }
            }
            adjTiles[ModContent.TileType<CraftingAccess>()] = true;
        }

        private static bool IsAvailable(Recipe recipe)
        {
            foreach (int tile in recipe.requiredTile)
            {
                if (tile == -1)
                {
                    break;
                }
                if (!adjTiles[tile])
                {
                    return false;
                }
            }
            foreach (Item ingredient in recipe.requiredItem)
            {
                if (ingredient.type == 0)
                {
                    break;
                }
                int stack = ingredient.stack;
                bool useRecipeGroup = false;
                foreach (int type in ItemCounts.Keys)
                {
                    if (RecipeGroupMatch(recipe, type, ingredient.type))
                    {
                        stack -= ItemCounts[type];
                        useRecipeGroup = true;
                    }
                }
                if (!useRecipeGroup && ItemCounts.ContainsKey(ingredient.netID))
                {
                    stack -= ItemCounts[ingredient.netID];
                }
                if (stack > 0)
                {
                    return false;
                }
            }
            if (recipe.HasCondition(Recipe.Condition.NearWater) && !adjWater && !adjTiles[TileID.Sinks])
            {
                return false;
            }
            if (recipe.HasCondition(Recipe.Condition.NearLava) && !adjLava)
            {
                return false;
            }
            if (recipe.HasCondition(Recipe.Condition.NearHoney) && !adjHoney)
            {
                return false;
            }
            if (recipe.HasCondition(Recipe.Condition.InSnow) && !zoneSnow)
            {
                return false;
            }
            try
            {
                BlockRecipes.active = false;
                if (!RecipeLoader.RecipeAvailable(recipe))
                {
                    return false;
                }
            }
            finally
            {
                BlockRecipes.active = true;
            }
            return true;
        }

        private static bool PassesBlock(Recipe recipe)
        {
            foreach (Item ingredient in recipe.requiredItem)
            {
                if (ingredient.type == 0)
                {
                    break;
                }
                int stack = ingredient.stack;
                bool useRecipeGroup = false;
                foreach (Item Item in storageItems)
                {
                    ItemData data = new ItemData(Item);
                    if (!blockStorageItems.Contains(data) && RecipeGroupMatch(recipe, Item.netID, ingredient.type))
                    {
                        stack -= Item.stack;
                        useRecipeGroup = true;
                    }
                }
                if (!useRecipeGroup)
                {
                    foreach (Item Item in storageItems)
                    {
                        ItemData data = new ItemData(Item);
                        if (!blockStorageItems.Contains(data) && Item.netID == ingredient.netID)
                        {
                            stack -= Item.stack;
                        }
                    }
                }
                if (stack > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static void RefreshStorageItems()
        {
            storageItems.Clear();
            result = null;
            if (selectedRecipe != null)
            {
                foreach (Item Item in Items)
                {
                    for (int k = 0; k < selectedRecipe.requiredItem.Count; k++)
                    {
                        if (selectedRecipe.requiredItem[k].type == 0)
                        {
                            break;
                        }
                        if (Item.type == selectedRecipe.requiredItem[k].type || RecipeGroupMatch(selectedRecipe, selectedRecipe.requiredItem[k].type, Item.type))
                        {
                            storageItems.Add(Item);
                        }
                    }
                    if (Item.type == selectedRecipe.createItem.type)
                    {
                        result = Item;
                    }
                }
                if (result == null)
                {
                    result = new Item();
                    result.SetDefaults(selectedRecipe.createItem.type);
                    result.stack = 0;
                }
            }
        }

        private static bool RecipeGroupMatch(Recipe recipe, int type1, int type2)
        {
            return (recipe.HasRecipeGroup(RecipeGroupID.Wood) || recipe.HasRecipeGroup(RecipeGroupID.Sand) || recipe.HasRecipeGroup(RecipeGroupID.IronBar) || recipe.HasRecipeGroup(RecipeGroupID.Fragment) || recipe.acceptedGroups.Contains(type2) || recipe.HasRecipeGroup(RecipeGroupID.PressurePlate)) && type1 == type2;
        }

        private static void HoverStation(int slot, ref int hoverSlot)
        {
            TECraftingAccess ent = GetCraftingEntity();
            if (ent == null || slot >= ent.stations.Length)
            {
                return;
            }

            Player player = Main.player[Main.myPlayer];
            if (MouseClicked)
            {
                bool changed = false;
                if (!ent.stations[slot].IsAir && ItemSlot.ShiftInUse)
                {
                    Item result = player.GetItem(Main.myPlayer, DoWithdraw(slot), GetItemSettings.InventoryEntityToPlayerInventorySettings);
                    if (!result.IsAir && Main.mouseItem.IsAir)
                    {
                        Main.mouseItem = result;
                        result = new Item();
                    }
                    if (!result.IsAir && Main.mouseItem.type == result.type && Main.mouseItem.stack < Main.mouseItem.maxStack)
                    {
                        Main.mouseItem.stack += result.stack;
                        result = new Item();
                    }
                    if (!result.IsAir)
                    {
                        player.QuickSpawnClonedItem(result);
                    }
                    changed = true;
                }
                else if (player.itemAnimation == 0 && player.itemTime == 0)
                {
                    int oldType = Main.mouseItem.type;
                    int oldStack = Main.mouseItem.stack;
                    Main.mouseItem = DoStationSwap(Main.mouseItem, slot);
                    if (oldType != Main.mouseItem.type || oldStack != Main.mouseItem.stack)
                    {
                        changed = true;
                    }
                }
                if (changed)
                {
                    RefreshItems();
                    Terraria.Audio.SoundEngine.PlaySound(7, -1, -1, 1);
                }
            }

            hoverSlot = slot;
        }

        private static void HoverRecipe(int slot, ref int hoverSlot)
        {
            int visualSlot = slot;
            slot += numColumns * (int)Math.Round(scrollBar.ViewPosition);
            if (slot < recipes.Count)
            {
                if (MouseClicked)
                {
                    selectedRecipe = recipes[slot];
                    RefreshStorageItems();
                    blockStorageItems.Clear();
                }
                hoverSlot = visualSlot;
            }
        }

        private static void HoverItem(int slot, ref int hoverSlot)
        {
            hoverSlot = slot;
        }

        private static void HoverStorage(int slot, ref int hoverSlot)
        {
            int visualSlot = slot;
            slot += numColumns2 * (int)Math.Round(scrollBar2.ViewPosition);
            if (slot < storageItems.Count)
            {
                if (MouseClicked)
                {
                    ItemData data = new ItemData(storageItems[slot]);
                    if (blockStorageItems.Contains(data))
                    {
                        blockStorageItems.Remove(data);
                    }
                    else
                    {
                        blockStorageItems.Add(data);
                    }
                }
                hoverSlot = visualSlot;
            }
        }

        private static void HoverResult(int slot, ref int hoverSlot)
        {
            if (slot != 0)
            {
                return;
            }

            Player player = Main.player[Main.myPlayer];
            if (MouseClicked)
            {
                bool changed = false;
                if (!Main.mouseItem.IsAir && player.itemAnimation == 0 && player.itemTime == 0 && result != null && Main.mouseItem.type == result.type)
                {
                    if (TryDepositResult(Main.mouseItem))
                    {
                        changed = true;
                    }
                }
                else if (Main.mouseItem.IsAir && result != null && !result.IsAir)
                {
                    Item toWithdraw = result.Clone();
                    if (toWithdraw.stack > toWithdraw.maxStack)
                    {
                        toWithdraw.stack = toWithdraw.maxStack;
                    }
                    Main.mouseItem = DoWithdrawResult(toWithdraw, ItemSlot.ShiftInUse);
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

            if (curMouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released && result != null && !result.IsAir && (Main.mouseItem.IsAir || ItemData.Matches(Main.mouseItem, Items[slot]) && Main.mouseItem.stack < Main.mouseItem.maxStack))
            {
                slotFocus = true;
            }

            hoverSlot = slot;

            if (slotFocus)
            {
                SlotFocusLogic();
            }
        }

        private static void SlotFocusLogic()
        {
            if (result == null || result.IsAir || (!Main.mouseItem.IsAir && (!ItemData.Matches(Main.mouseItem, result) || Main.mouseItem.stack >= Main.mouseItem.maxStack)))
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
                    Item toWithdraw = result.Clone();
                    toWithdraw.stack = 1;
                    Item withdrawn = DoWithdrawResult(toWithdraw);
                    if (Main.mouseItem.IsAir)
                    {
                        Main.mouseItem = withdrawn;
                    }
                    else
                    {
                        Main.mouseItem.stack += withdrawn.stack;
                    }
                    //Main.soundInstanceMenuTick.Stop();
                    //Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                    Terraria.Audio.SoundEngine.PlaySound(12, -1, -1, 1);
                    RefreshItems();
                }
                rightClickTimer--;
            }
        }

        private static void ResetSlotFocus()
        {
            slotFocus = false;
            rightClickTimer = 0;
            maxRightClickTimer = startMaxRightClickTimer;
        }

        private static Item DoWithdraw(int slot)
        {
            TECraftingAccess access = GetCraftingEntity();
            if (Main.netMode == 0)
            {
                Item result = access.TryWithdrawStation(slot);
                RefreshItems();
                return result;
            }
            else
            {
                NetHelper.SendWithdrawStation(access.ID, slot);
                return new Item();
            }
        }

        private static Item DoStationSwap(Item Item, int slot)
        {
            TECraftingAccess access = GetCraftingEntity();
            if (Main.netMode == 0)
            {
                Item result = access.DoStationSwap(Item, slot);
                RefreshItems();
                return result;
            }
            else
            {
                NetHelper.SendStationSlotClick(access.ID, Item, slot);
                return new Item();
            }
        }

        private static void TryCraft()
        {
            List<Item> availableItems = new List<Item>(storageItems.Where(Item => !blockStorageItems.Contains(new ItemData(Item))).Select(Item => Item.Clone()));
            List<Item> toWithdraw = new List<Item>();
            for (int k = 0; k < selectedRecipe.requiredItem.Count; k++)
            {
                Item Item = selectedRecipe.requiredItem[k];
                if (Item.type == 0)
                {
                    break;
                }
                int stack = Item.stack;
                Recipe ModRecipe = selectedRecipe as Recipe;
                if (ModRecipe != null)
                {
                    Recipe.ConsumeItemCallback consumeItemCallback = (Recipe recipe, int type, ref int amount) => {
                        stack = amount;
                    };
                    ModRecipe.AddConsumeItemCallback(consumeItemCallback);
                }
                if (selectedRecipe.HasTile(TileID.AlchemyTable) && alchemyTable)
                {
                    int save = 0;
                    for (int j = 0; j < stack; j++)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            save++;
                        }
                    }
                    stack -= save;
                }
                if (stack > 0)
                {
                    foreach (Item tryItem in availableItems)
                    {
                        if (Item.type == tryItem.type || RecipeGroupMatch(selectedRecipe, Item.type, tryItem.type))
                        {
                            if (tryItem.stack > stack)
                            {
                                Item temp = tryItem.Clone();
                                temp.stack = stack;
                                toWithdraw.Add(temp);
                                tryItem.stack -= stack;
                                stack = 0;
                            }
                            else
                            {
                                toWithdraw.Add(tryItem.Clone());
                                stack -= tryItem.stack;
                                tryItem.stack = 0;
                                tryItem.type = 0;
                            }
                        }
                    }
                }
            }
            Item resultItem = selectedRecipe.createItem.Clone();
            resultItem.Prefix(-1);

            RecipeLoader.OnCraft(resultItem, selectedRecipe);

            if (Main.netMode == 0)
            {
                foreach (Item Item in DoCraft(GetHeart(), toWithdraw, resultItem))
                {
                    Main.player[Main.myPlayer].QuickSpawnClonedItem(Item, Item.stack);
                }
            }
            else if (Main.netMode == 1)
            {
                NetHelper.SendCraftRequest(GetHeart().ID, toWithdraw, resultItem);
            }
        }

        internal static List<Item> DoCraft(TEStorageHeart heart, List<Item> toWithdraw, Item result)
        {
            List<Item> Items = new List<Item>();
            foreach (Item tryWithdraw in toWithdraw)
            {
                Item withdrawn = heart.TryWithdraw(tryWithdraw);
                if (!withdrawn.IsAir)
                {
                    Items.Add(withdrawn);
                }
                if (withdrawn.stack < tryWithdraw.stack)
                {
                    for (int k = 0; k < Items.Count; k++)
                    {
                        heart.DepositItem(Items[k]);
                        if (Items[k].IsAir)
                        {
                            Items.RemoveAt(k);
                            k--;
                        }
                    }
                    return Items;
                }
            }
            Items.Clear();
            heart.DepositItem(result);
            if (!result.IsAir)
            {
                Items.Add(result);
            }
            return Items;
        }

        private static bool TryDepositResult(Item Item)
        {
            int oldStack = Item.stack;
            DoDepositResult(Item);
            return oldStack != Item.stack;
        }

        private static void DoDepositResult(Item Item)
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

        private static Item DoWithdrawResult(Item Item, bool toInventory = false)
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