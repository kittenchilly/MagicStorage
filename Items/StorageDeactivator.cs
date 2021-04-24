using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MagicStorage.Components;
using Terraria.Localization;

namespace MagicStorage.Items
{
    public class StorageDeactivator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storage Unit Wand");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Жезл Ячейки Хранилища");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Polish, "Różdżka jednostki magazynującej");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Baguette d'unité de stockage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Spanish, "Varita de unidad de almacenamiento");
            DisplayName.AddTranslation((int)GameCulture.CultureName.French, "Baguetter d'unité de stockage");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "存储单元魔杖");

            Tooltip.SetDefault("<right> Storage Unit to toggle between Active/Inactive");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "<right> на Ячейке Хранилища что бы активировать/деактивировать ее");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Polish, "<right> aby przełączyć Jednostkę Magazynującą (wł./wył.)");
            Tooltip.AddTranslation((int)GameCulture.CultureName.French, "<right> pour changer l'unité de stockage actif/inactif");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Spanish, "<right> para cambiar el unidad de almacenamiento activo/inactivo");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "<right>存储单元使其切换启用/禁用");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = 1;
            Item.tileBoost = 20;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 0, 40, 0);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemAnimation > 0 && player.itemTime == 0 && player.controlUseItem)
            {
                int i = Player.tileTargetX;
                int j = Player.tileTargetY;
                if (Main.tile[i, j].frameX % 36 == 18)
                {
                    i--;
                }
                if (Main.tile[i, j].frameY % 36 == 18)
                {
                    j--;
                }
                Point16 point = new Point16(i, j);
                if (TileEntity.ByPosition.ContainsKey(point) && TileEntity.ByPosition[point] is TEAbstractStorageUnit)
                {
                    TEAbstractStorageUnit storageUnit = (TEAbstractStorageUnit)TileEntity.ByPosition[point];
                    storageUnit.Inactive = !storageUnit.Inactive;
                    string activeText = storageUnit.Inactive ? "Deactivated" : "Activated";
                    Main.NewText("Storage Unit has been " + activeText);
                    NetHelper.ClientSendTEUpdate(storageUnit.ID);
                    if (storageUnit is TEStorageUnit)
                    {
                        ((TEStorageUnit)storageUnit).UpdateTileFrameWithNetSend();
                        if (Main.netMode == 0)
                        {
                            storageUnit.GetHeart().ResetCompactStage();
                        }
                    }
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ActuationRod)
            .AddIngredient(ModContent.ItemType<StorageComponent>())
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
