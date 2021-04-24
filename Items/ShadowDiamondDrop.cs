using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Events;

namespace MagicStorage.NPCs
{
    public class ShadowDiamondDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) //TODO: make this more elegantly use the new drop rules
        {
            if (npc.type == NPCID.KingSlime && !StorageWorld.kingSlimeDiamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.kingSlimeDiamond = true;
            }
            else if (npc.type == NPCID.EyeofCthulhu && !StorageWorld.boss1Diamond)
            {
                DropDiamond(npc, Main.expertMode ? 2 : 1, npcLoot);
                StorageWorld.boss1Diamond = true;
            }
            else if ((npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.BrainofCthulhu) && !StorageWorld.boss2Diamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.boss2Diamond = true;
            }
            else if (npc.type == NPCID.SkeletronHead && !StorageWorld.boss3Diamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.boss3Diamond = true;
            }
            else if (npc.type == NPCID.QueenBee && !StorageWorld.queenBeeDiamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.queenBeeDiamond = true;
            }
            else if (npc.type == NPCID.WallofFlesh && !StorageWorld.hardModeDiamond)
            {
                DropDiamond(npc, 2, npcLoot);
                StorageWorld.hardModeDiamond = true;
            }
            else if (npc.type == NPCID.TheDestroyer && !StorageWorld.mechBoss1Diamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.mechBoss1Diamond = true;
            }
            else if ((npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism) && !StorageWorld.mechBoss2Diamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.mechBoss2Diamond = true;
            }
            else if (npc.type == NPCID.SkeletronPrime && !StorageWorld.mechBoss3Diamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.mechBoss3Diamond = true;
            }
            else if (npc.type == NPCID.Plantera && !StorageWorld.plantBossDiamond)
            {
                DropDiamond(npc, Main.expertMode ? 2 : 1, npcLoot);
                StorageWorld.plantBossDiamond = true;
            }
            else if (npc.type == NPCID.Golem && !StorageWorld.golemBossDiamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.golemBossDiamond = true;
            }
            else if (npc.type == NPCID.DukeFishron && !StorageWorld.fishronDiamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.fishronDiamond = true;
            }
            else if (npc.type == NPCID.CultistBoss && !StorageWorld.ancientCultistDiamond)
            {
                DropDiamond(npc, 1, npcLoot);
                StorageWorld.ancientCultistDiamond = true;
            }
            else if (npc.type == NPCID.MoonLordCore && !StorageWorld.moonlordDiamond)
            {
                DropDiamond(npc, Main.expertMode ? 3 : 2, npcLoot);
                StorageWorld.moonlordDiamond = true;
            }
        }

        private void DropDiamond(NPC npc, int stack, NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ShadowDiamond>(), 1, stack, stack));
        }
    }
}