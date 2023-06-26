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
        public static void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Nurse)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<NurseWalkieTalkie>());
                nextSlot++;

                // We can use shopCustomPrice and shopSpecialCurrency to support custom prices and currency. Usually a shop sells an item for item.value. 
                // Editing item.value in SetupShop is an incorrect approach.
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<SurfaceTransponder>());
                shop.item[nextSlot].shopCustomPrice = 2;
                shop.item[nextSlot].shopSpecialCurrency = CustomCurrencyID.DefenderMedals; // omit this line if shopCustomPrice should be in regular coins. 
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModContent.ItemType<PlatinumInsurance>());
                shop.item[nextSlot].shopCustomPrice = 3;
                nextSlot++;
            }
        }
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
