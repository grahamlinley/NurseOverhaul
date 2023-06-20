using NurseHotkey.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;

namespace NurseHotkey.NPCs
{
    public class NurseHotkeyGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                // This is where we add item drop rules for VampireBat, here is a simple example:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SingleBandTransceiver>(), 1, 1, 1));
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                // This is where we add item drop rules for VampireBat, here is a simple example:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BioticRifle>(), 1, 1, 1));
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                // This is where we add item drop rules for VampireBat, here is a simple example:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Thruster>(), 1, 1, 1));
            }
        }
    }
}
