using NurseOverhaul.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using NurseOverhual;

namespace NurseOverhaul
{
    public class NurseOverhaulGlobalNPC : GlobalNPC
    {
        
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            // KEEP debugging KEEP ALL
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
                for (int i = 0; i < Chest.maxItems; i++)
                {
                    items[i] = NurseShopItems()[i];
                }
            }
        }
        public static Item[] NurseShopItems()
        {
            List<(int id, int price)> items = new List<(int id, int price)>
            {
                (ItemID.Mushroom, 250),
                (ItemID.BottledWater, 200),
                (ItemID.BottledHoney, 400),
                (ItemID.LesserHealingPotion, 300)
            };

            if (NPC.downedSlimeKing)
            {
                items.Add((ItemID.HealingPotion, 10000));
            }

            if (NPC.downedBoss1)
            {
                items.Add((ItemID.RestorationPotion, 15000));
            }

            if (Main.hardMode)
            {
                items.Add((ItemID.LifeforcePotion, 10000));
                items.Add((ItemID.GreaterHealingPotion, 50000));
            }


            if (NPC.downedAncientCultist)
            {
                items.Add((ItemID.SuperHealingPotion, 150000));
            }


            int supremeHealingPotionIndex = -1;
            int omegaHealingPotionIndex = -1;

            // Calamity weak references, not to worried about name changes. If they do it should just not populate anyway
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);

            if (Calamity != null && NPC.downedMoonlord && Calamity.TryFind<ModItem>("SupremeHealingPotion", out ModItem supremeHealingPotion))
            {
                supremeHealingPotionIndex = items.FindIndex(item => item.id == ItemID.SuperHealingPotion);

                items.Add((supremeHealingPotion.Item.type, 500000));


            }

            if (Calamity != null && (bool)Calamity.Call("Downed", "dog") && Calamity.TryFind<ModItem>("OmegaHealingPotion", out ModItem omegaHealingPotion))
            {
                omegaHealingPotionIndex = items.FindIndex(item => item.id == ItemID.SuperHealingPotion);

                items.Add((omegaHealingPotion.Item.type, 1000000));

            }

            if (ModContent.GetInstance<NurseOverhaulConfig>().NurseVIPBadgeEnabled)
            {
                items.Add((ModContent.ItemType<NurseVIPBadge>(), 5000));
            }

            if (NPC.downedBoss2)
            {
                if (ModContent.GetInstance<NurseOverhaulConfig>().NursesWalkieTalkieEnabled)
                {
                    items.Add((ModContent.ItemType<NurseWalkieTalkie>(), 250000));
                }
            }

            if (NPC.downedBoss3)
            {
                if (ModContent.GetInstance<NurseOverhaulConfig>().NursesPaintedShirtEnabled)
                {
                    items.Add((ModContent.ItemType<SurfaceTransponder>(), 1000000));
                }
            }

            if (Main.hardMode)
            {
                if (ModContent.GetInstance<NurseOverhaulConfig>().NurseNourishmentDiamondEnabled)
                {
                    items.Add((ModContent.ItemType<PlatinumInsurance>(), 4000000));
                }
            }


            if (NPC.downedBoss1)
            {
                if (ModContent.GetInstance<NurseOverhaulConfig>().LifeCrystalInShop)
                {
                    items.Add((ItemID.LifeCrystal, 1000000));
                }
            }

            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                if (ModContent.GetInstance<NurseOverhaulConfig>().LifeFruitInShop)
                {
                    items.Add((ItemID.LifeFruit, 2000000));
                }
            }

            Item[] newItem = new Item[40];
            for (int i = 0; i < items.Count; i++) // very important for making shop items actually shop items, at least it was in 1.4.3. With 1.4.4 shop changes might not be important but it works so I'm keeping it
            {
                newItem[i] = new Item(items[i].id);
                newItem[i].shopCustomPrice = items[i].price;
                newItem[i].isAShopItem = true;
            }
            return newItem;
        }


        // Adds Nurse component items to bosses listed
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime && (ModContent.GetInstance<NurseOverhaulConfig>().NursesWalkieTalkieEnabled))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrokenWalkieTalkie>(), 1, 1, 1));
            }
            if (npc.type == NPCID.EyeofCthulhu && (ModContent.GetInstance<NurseOverhaulConfig>().NursesPaintedShirtEnabled))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BioticRifle>(), 1, 1, 1));
            }
            if (npc.type == NPCID.SkeletronHead && (ModContent.GetInstance<NurseOverhaulConfig>().NurseNourishmentDiamondEnabled))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Thruster>(), 1, 1, 1));
            }
        }
    }
}
