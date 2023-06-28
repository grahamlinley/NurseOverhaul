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
using System.Security.Cryptography;

namespace NurseHotkey.NPCs
{
    public class NurseHotkeyGlobalNPC : GlobalNPC
    {
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            // Your custom code here
            // For example, you can call your ModifyActiveShop method:
            List<Item> customItems = NurseHotkeyUI.ModifyActiveShop();
            for (int i = 0; i < customItems.Count && i < items.Length; i++)
            {
                items[i] = customItems[i];
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                // This is where we add item drop rules for VampireBat, here is a simple example:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrokenWalkieTalkie>(), 1, 1, 1));
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
