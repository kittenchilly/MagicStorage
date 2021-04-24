using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicStorage.Components
{
    public class CreativeStorageUnit : StorageComponent
    {
        public override ModTileEntity GetTileEntity()
        {
            return (ModTileEntity)TileEntity.manager.GetTileEntity<TECreativeStorageUnit>(ModContent.TileEntityType<TECreativeStorageUnit>());
        }

        public override int ItemType(int frameX, int frameY)
        {
            return ModContent.ItemType<Items.CreativeStorageUnit>();
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPos = zero + 16f * new Vector2(i, j) - Main.screenPosition;
            Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
            Color lightColor = Lighting.GetColor(i, j, Color.White);
            Color color = Color.Lerp(Color.White, lightColor, 0.5f);
            spriteBatch.Draw(ModContent.GetTexture("MagicStorage/Components/CreativeStorageUnit_Glow").Value, drawPos, frame, color);
        }
    }
}