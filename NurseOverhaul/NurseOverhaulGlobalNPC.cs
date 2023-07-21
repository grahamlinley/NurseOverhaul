using NurseOverhaul.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using NurseOverhual;

namespace NurseOverhaul.NPCs
{
    public class NurseOverhaulGlobalNPC : GlobalNPC
    {
        
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            // debugging
            /*
            static string GetShopName(int npcType, string shopName = "Shop")
            {
                return $"{(npcType < NPCID.Count ? $"Terraria/{NPCID.Search.GetName(npcType)}" : NPCLoader.GetNPC(npcType).FullName)}/{shopName}";
            }
            */
            // Checks what shop you're interacting with (debugging)
            //Main.NewText($"{GetShopName(Main.npcShop)})");
            //Main.NewText($"Interacted with NPC type: {npc.type}");
            //Main.NewText($"{Main.npcShop == npc.whoAmI}");
            if (npc.type == NPCID.Nurse)
            {
                // Confirms you're in the right shop (debugging)
               // Main.NewText("The shop is being called correctly for the Nurse.");
                //Main.NewText($"{Main.npcShop == npc.whoAmI}");

                // Calls listed items in NurseHotkeyUI's ModifyActiveShop method
                List<Item> customItems = NurseOverhaulUIState.NurseShopItems();
                for (int i = 0; i < customItems.Count && i < items.Length; i++)
                {
                    items[i] = customItems[i];
                }
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
