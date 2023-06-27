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
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Nurse)
            {
                // Adding an item to a vanilla NPC is easy:
                // This item sells for the normal price.
                shop.Add<NurseWalkieTalkie>();
                shop.Add(ItemID.Mushroom);
                shop.Register();

                // We can use shopCustomPrice and shopSpecialCurrency to support custom prices and currency. Usually a shop sells an item for item.value.
                // Editing item.value in SetupShop is an incorrect approach.
                /*
                // This shop entry sells for 2 Defenders Medals.
                shop.Add(new Item(ModContent.ItemType<ExampleMountItem>())
                {
                    shopCustomPrice = 2,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals // omit this line if shopCustomPrice should be in regular coins.
                });

                // This shop entry sells for 3 of a custom currency added in our mod.
                shop.Add(new Item(ModContent.ItemType<ExampleMountItem>())
                {
                    shopCustomPrice = 2,
                    shopSpecialCurrency = ExampleMod.ExampleCustomCurrencyId
                });*/
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
