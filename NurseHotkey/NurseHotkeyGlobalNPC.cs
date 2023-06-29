using NurseHotkey.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace NurseHotkey.NPCs
{
    public class NurseHotkeyGlobalNPC : GlobalNPC
    {
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            // Calls listed items in NurseHotkeyUI's ModifyActiveShop method, 
            List<Item> customItems = NurseHotkeyUI.ModifyActiveShop();
            for (int i = 0; i < customItems.Count && i < items.Length; i++)
            {
                items[i] = customItems[i];
            }
        }

        // Adds Nurse component items to bosses listed
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrokenWalkieTalkie>(), 1, 1, 1));
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BioticRifle>(), 1, 1, 1));
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Thruster>(), 1, 1, 1));
            }
        }
    }
}
