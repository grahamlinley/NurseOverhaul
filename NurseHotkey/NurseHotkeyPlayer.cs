using Microsoft.Xna.Framework;
using NurseHotkey.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseHotkey
{
    public class NurseHotkeyPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            Player player = Main.LocalPlayer;
            if (PlayerIsInRangeOfNurse() && PlayerHasTransponder())
            {
                player.AddBuff(ModContent.BuffType<Buffs.NurseInRange>(), 2);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (NurseHotkey.NurseHealHotkey.JustPressed)
            {
                NurseHeal();
            }
        }

        public static bool ExcludedDebuff(int buffType)
        {
            // List of debuffs to be excluded
            int[] excludedDebuffs = { BuffID.PotionSickness, BuffID.WaterCandle, BuffID.NoBuilding, BuffID.Werewolf, BuffID.DryadsWard, BuffID.HeartLamp, BuffID.PeaceCandle,
            BuffID.Honey, BuffID.StarInBottle, BuffID.CatBast, BuffID.Sunflower, BuffID.Merfolk, BuffID.Campfire};

            // Check if the given buffType is in the excludedDebuffs list
            return excludedDebuffs.Contains(buffType);
        }

        public static float GetHealCost(int healthMissing, Player player)
        {
            int nurseNPCId = NPC.FindFirstNPC(NPCID.Nurse);
            NPC nurseNPC = Main.npc[nurseNPCId];
            var currentShoppingSettings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, nurseNPC);
            float nurseHappinessAdjustment = (float)currentShoppingSettings.PriceAdjustment;
            float difficultyFactor = 1; // Set the difficulty factor to 1 by default
            float baseCost = healthMissing;
            float debuffCount = GetDebuffCount(player); // Get the count of debuffs for the player
            float bossRange = 6400f; // Range of 6400 units
            bool isBossInRange = bossCombatCheck(bossRange);


            if (Main.expertMode)
            {
                difficultyFactor = 2; // If the game is in Expert mode, set the difficulty factor to 2
            }
            if (Main.masterMode)
            {
                difficultyFactor = 2; // If the game is in Master mode, set the difficulty factor to 3
            }

            float preCost = (baseCost * difficultyFactor); // Calculate the total cost of the heal based on the difficulty factor


            if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) == true && debuffCount - 1 > 0)
            {
                preCost += (debuffCount - 1) * 200;
            }

            else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
            {
                preCost += debuffCount * 200; // Add the debuff cost to the total cost, multiplied by 2 silver per debuff
            }

            else
            {
                preCost += debuffCount * 100; // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
            }

            float calamityBaseCost = 0;

            if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && debuffCount > 0 || healthMissing > 0) //increases base cost by set amount based on https://calamitymod.fandom.com/wiki/Town_NPCs/Nurse_Price_Scaling
                                                                                                              //NOTE: Not sure if the prices are inaacurate on that page or if the scaling is different, but values derived below are hand tested on a max happiness Nurse and accurate for these 4 tests:
                                                                                                              //1. Max health 1 debuff 2. Missing 10 health 3. Missing 490 health (or whatever other large number your mod uses) 4. Missing somewhere in between those two number to test float accuracy
            {
                if (BossChecklistIntegration.isYharonDefeated())
                {
                    calamityBaseCost = 89700; //897
                }
                else if (BossChecklistIntegration.isDevourerDefeated())
                {
                    calamityBaseCost = 59700; //5.97
                }
                else if (BossChecklistIntegration.isProvidenceDefeated()) //Providence defeated
                {
                    calamityBaseCost = 31700; //3 gold 20 silver base
                }
                else if (NPC.downedMoonlord) // Moon Lord defeated
                {
                    calamityBaseCost = 19700; // 1 gold 97 silver base
                }
                else if (NPC.downedFishron | BossChecklistIntegration.isPlaguebringerDefeated() | BossChecklistIntegration.isRavagerDefeated()) // Duke Fishron/Plaguebringer Goliath/ Ravager defeated
                {
                    calamityBaseCost = 11700; // 1 gold 17 silver base
                }
                else if (NPC.downedGolemBoss) // Golem defeated
                {
                    calamityBaseCost = 8700; // 87 silver base
                }
                else if (NPC.downedPlantBoss | BossChecklistIntegration.isCalamitasCloneDefeated()) // Plantera defeated
                {
                    calamityBaseCost = 5700; // 57 silver base
                }
                else if (NPC.downedMechBossAny) // At least one Mechanical Boss defeated
                {
                    calamityBaseCost = 3700; // 37 silver base
                }
                else if (Main.hardMode)
                {
                    calamityBaseCost = 2100; // 21 silver base
                }
                else if (NPC.downedBoss3) // Skeletron defeated 
                {
                    calamityBaseCost = 900; // Increased price (9 silver base)
                }
                else if (NPC.downedBoss1) // Eye of Cthulhu defeated
                {
                    calamityBaseCost = 300; // Increased price (3 silver base). Note this differs from what is on the Calamity wiki for whatever reason. Base prices from here are hand calculated
                }
            }

            float finalCost;

            if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) == true && debuffCount > 1)
            {
                calamityBaseCost += preCost * GetMultiplier() * nurseHappinessAdjustment;
                finalCost = calamityBaseCost * 5;
            }
            else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
            {
                finalCost = calamityBaseCost + (preCost * GetMultiplier() * nurseHappinessAdjustment);
            }
            else
            {
                finalCost = preCost * GetMultiplier() * nurseHappinessAdjustment; // Multiply the total cost by the boss defeat multiplier
            }

            return finalCost;
        }

        public static bool bossCombatCheck(float range)
        {
            Player player = Main.LocalPlayer;

            foreach (NPC npc in Main.npc)
            {
                // Check if the NPC is a boss and if it's still active
                if (npc.active && npc.boss && npc.life > 0)
                {
                    // Calculate the distance between the boss and the player
                    float distance = Vector2.Distance(npc.position, player.position);

                    // Check if the boss is within the specified range
                    if (distance <= range)
                    {
                        // Boss found within range
                        return true;
                    }
                }
            }

            // No boss found within range
            return false;
        }
        public static int GetDebuffCount(Player player)
        {
            int debuffCount = 0;
            for (int i = 0; i < player.buffType.Length; i++)
            {
                int buffType = player.buffType[i];

                if (buffType > 0 && Main.debuff[buffType] && !ExcludedDebuff(buffType))
                {
                    debuffCount++;
                }
            }
            return debuffCount;
        }

        public bool IsBossAlive(string bossName)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.boss && npc.FullName == bossName)
                {
                    return true;
                }
            }

            return false;
        }

        private static float GetMultiplier()
        {
            int nurseNPCId = NPC.FindFirstNPC(NPCID.Nurse);
            NPC nurseNPC = Main.npc[nurseNPCId];
            var currentShoppingSettings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, nurseNPC);
            float nurseHappinessAdjustment = (float)currentShoppingSettings.PriceAdjustment;
            float multiplier = 0;
            Main.NewText($"happiness {nurseHappinessAdjustment}");

            if (nurseHappinessAdjustment < 1.07 && nurseHappinessAdjustment != 1) //
            {
                multiplier += .9995f;
            }

            if (nurseHappinessAdjustment >= 1.07 | nurseHappinessAdjustment == 1)
            {
                multiplier += 1f;
            }

            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier += 2f; //2.8499f
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier += 7f; //9.4999f or 6.6491 if 6.65 wrong
                if (!NPC.downedBoss1)
                {
                    multiplier += 2f;
                }
            }

            if (NPC.downedBoss3 | NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier += 15f; //23.7499f

                if (!NPC.downedBoss1)
                {
                    multiplier += 2f;
                }
                if (!NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
                {
                    multiplier += 7f; //9.4999f or 6.6491 if 6.65 wrong
                }
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier += 35f; //28f fixed

                if (!NPC.downedBoss1)
                {
                    multiplier += 2f;
                }
                if (!NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
                {
                    multiplier += 7f; //9.4999f or 6.6491 if 6.65 wrong
                }
                if (!NPC.downedBoss3 && !NPC.downedQueenBee)
                {
                    multiplier += 15f;
                }
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier += 40f; //94.9999f

                if (!Main.hardMode)
                {
                    multiplier += 35f;
                }
                if (!NPC.downedBoss1)
                {
                    multiplier += 2f;
                }
                if (!NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
                {
                    multiplier += 7f; //9.4999f or 6.6491 if 6.65 wrong
                }
                if (!NPC.downedBoss3 && !NPC.downedQueenBee)
                {
                    multiplier += 15f;
                }
            }

            if (NPC.downedPlantBoss) //Plantera or Calamitas clone
            {
                multiplier += 50f; //142.4999f

                if (!NPC.downedMechBossAny)
                {
                    multiplier += 40f;
                }
                if (!Main.hardMode)
                {
                    multiplier += 35f;
                }
                if (!NPC.downedBoss1)
                {
                    multiplier += 2f;
                }
                if (!NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
                {
                    multiplier += 7f; //9.4999f or 6.6491 if 6.65 wrong
                }
                if (!NPC.downedBoss3 && !NPC.downedQueenBee)
                {
                    multiplier += 15f;
                }
            }

            if (NPC.downedGolemBoss) // Golem
            {
                multiplier += 50f; //

                if (!NPC.downedPlantBoss)
                {
                    multiplier += 50f;
                }
                if (!NPC.downedMechBossAny)
                {
                    multiplier += 40f;
                }
                if (!Main.hardMode)
                {
                    multiplier += 35f;
                }
                if (!NPC.downedBoss1)
                {
                    multiplier += 2f;
                }
                if (!NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
                {
                    multiplier += 7f; //9.4999f or 6.6491 if 6.65 wrong
                }
                if (!NPC.downedBoss3 && !NPC.downedQueenBee)
                {
                    multiplier += 15f;
                }
            }
            return multiplier;
        }


        public static void CureAllDebuffs(Player player)
        {
            for (int i = 0; i < player.buffType.Length; i++)
            {
                int buffType = player.buffType[i];

                if (buffType > 0 && Main.debuff[buffType] && !ExcludedDebuff(buffType))
                {
                    player.DelBuff(i);
                    i--; // Decrement i to account for the removed buff
                }
            }
        }
        private bool PlayerHasItem(Player player, int itemType)
        {
            return player.inventory.Any(i => i.type == itemType);
        }
        private int GetRangeForItem(int itemType)
        {
            if (itemType == ModContent.ItemType<NurseVIPBadge>())
                return 320;
            if (itemType == ModContent.ItemType<LocalTransponder>())
                return 3200;
            if (itemType == ModContent.ItemType<SurfaceTransponder>())
                return 32000;
            if (itemType == ModContent.ItemType<GlobalTransponder>())
                return 320000;
            return 0;
        }

        private bool PlayerIsInRangeOfNurse()
        {
            Player player = Main.LocalPlayer;
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];
            int highestRange = GetHighestRangeForItems(player);
            return nurse != null && Vector2.Distance(player.Center, nurse.Center) <= highestRange;
        }

        private bool PlayerHasTransponder()
        {
            Player player = Main.LocalPlayer;
            int[] itemsTypesToCheck = new int[]
            {
                ModContent.ItemType<NurseVIPBadge>(),
                ModContent.ItemType<LocalTransponder>(),
                ModContent.ItemType<SurfaceTransponder>(),
                ModContent.ItemType<GlobalTransponder>()
            };
            return itemsTypesToCheck.Any(itemType => PlayerHasItem(player, itemType));
        }

        private int GetHighestRangeForItems(Player player)
        {
            int[] itemsTypesToCheck = new int[]
            {
                ModContent.ItemType<NurseVIPBadge>(),
                ModContent.ItemType<LocalTransponder>(),
                ModContent.ItemType<SurfaceTransponder>(),
                ModContent.ItemType<GlobalTransponder>()
            };
            int highestRange = 3200; // base range
            int rangeForItem;

            foreach (int itemType in itemsTypesToCheck)
            {
                if (PlayerHasItem(player, itemType))
                {
                    rangeForItem = GetRangeForItem(itemType);
                    highestRange = Math.Max(highestRange, rangeForItem);
                }
            }

            return highestRange;
        }

        public void NurseHeal()
        {
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];
            Player player = Main.LocalPlayer;
            Item[] GetAllItems(int playerIndex)
            {
                Player player = Main.player[playerIndex];
                List<Item> allItems = new List<Item>();
                Item[] inventory = player.inventory;

                // Add all bank items to the list
                allItems.AddRange(inventory);
                allItems.AddRange(player.bank.item);
                allItems.AddRange(player.bank2.item);
                allItems.AddRange(player.bank3.item);
                allItems.AddRange(player.bank4.item);

                return allItems.ToArray();
            }

            int[] itemsTypesToCheck = new int[]
            {
                    ModContent.ItemType<NurseVIPBadge>(),
                    ModContent.ItemType<LocalTransponder>(),
                    ModContent.ItemType<SurfaceTransponder>(),
                    ModContent.ItemType<GlobalTransponder>()
            };
                        int highestRange = 320; // base range
                        int rangeForItem;

                        foreach (int itemType in itemsTypesToCheck)
                        {
                            if (PlayerHasItem(player, itemType))
                            {
                                rangeForItem = GetRangeForItem(itemType);
                                highestRange = Math.Max(highestRange, rangeForItem);
                            }
                        }

            if (nurse != null && Vector2.Distance(Player.Center, nurse.Center) <= highestRange)
            {
                int healthMissing = Player.statLifeMax2 - Player.statLife;
                float cost = GetHealCost(healthMissing, Player);
                float totalMoney = 0;
                float debuffCount = GetDebuffCount(player); // Get the count of debuffs for the 
                int GetPlayerTotalMoney(int playerIndex)
                {
                    Player player = Main.player[playerIndex];

                    // Access the player's inventory
                    //Item[] inventory = player.inventory;

                    // Access all the player's Bank items
                    Item[] bankItems = GetAllItems(playerIndex);

                    // Calculate the total money from inventory
                    //int totalMoney = CalculateMoneyFromItems(inventory);

                    // Calculate the total money from all Banks
                    int totalMoney = CalculateMoneyFromItems(bankItems);

                    // Check if remaining cost is greater than zero
                    float remainingCost = cost - totalMoney;
                    return totalMoney;
                }

                // Helper method to calculate money from items
                int CalculateMoneyFromItems(Item[] items)
                {
                    for (int i = 0; i < items.Length; i++) // Change the loop condition here
                    {
                        Item item = items[i];

                        if (item.type == ItemID.CopperCoin)
                        {
                            totalMoney += item.stack;
                        }
                        else if (item.type == ItemID.SilverCoin)
                        {
                            totalMoney += item.stack * 100;
                        }
                        else if (item.type == ItemID.GoldCoin)
                        {
                            totalMoney += item.stack * 10000;
                        }
                        else if (item.type == ItemID.PlatinumCoin)
                        {
                            totalMoney += item.stack * 1000000;
                        }
                    }

                    return (int)totalMoney;
                }

                int Wallet = GetPlayerTotalMoney(Main.myPlayer);

                void HealAndSpend(float cost, Player player, int healthMissing)
                {
                    CureAllDebuffs(player); //debuffs destroyed
                    float flooredFloatCost = (float)Math.Floor(cost);
                    int intCost = Convert.ToInt32(flooredFloatCost);
                    player.BuyItem(intCost); //pay up 
                    int healAmount = healthMissing; //ok how much this mothasucka need
                    player.statLife += healAmount; //puts the item in the bag
                    SoundEngine.PlaySound(SoundID.Item4);

                    if (healAmount > 0) //needed for healing debuffs and no health (i.e. stink potion)
                    {
                        player.HealEffect(healAmount); //"ok here u go sir, have a nice day :)"
                    }

                    CalculateAndPrintSpending(intCost);
                }

                void CalculateAndPrintSpending(float cost)
                {
                    //all this shit just calculates your money then gives you a message 
                    int remainingMoney = (int)cost;
                    int platRemaining = remainingMoney / 1000000;
                    int goldRemaining = (remainingMoney % 1000000) / 10000;
                    int silverRemaining = (remainingMoney % 10000) / 100;
                    int copperRemaining = remainingMoney % 100;

                    if (platRemaining > 0)
                    {
                        string message = $"You just spent {platRemaining} platinum";

                        if (goldRemaining == 0 && silverRemaining == 0 && copperRemaining == 0)
                        {
                            message += $" on quick healing";
                        }
                        if (goldRemaining > 0 && (silverRemaining > 0 | copperRemaining > 0))
                        {
                            message += $" {goldRemaining} gold";
                        }
                        if (goldRemaining > 0 && silverRemaining == 0 && copperRemaining == 0)
                        {
                            message += $" and {goldRemaining} gold on quick healing.";
                        }
                        if (silverRemaining > 0 && copperRemaining > 0)
                        {
                            message += $" {silverRemaining} silver";
                        }
                        if (silverRemaining > 0 && copperRemaining == 0)
                        {
                            message += $" and {silverRemaining} silver on quick healing.";
                        }
                        if (copperRemaining > 0)
                        {
                            message += $" and {copperRemaining} copper on quick healing.";
                        }

                        Main.NewText(message);
                    }

                    if (goldRemaining > 0 && platRemaining == 0)
                    {
                        string message = $"You just spent {goldRemaining} gold";

                        if (silverRemaining == 0 && copperRemaining == 0)
                        {
                            message += $" on quick healing.";
                        }
                        if (silverRemaining > 0 && copperRemaining > 0)
                        {
                            message += $" {silverRemaining} silver";
                        }
                        if (silverRemaining > 0 && copperRemaining == 0)
                        {
                            message += $" and {silverRemaining} silver on quick healing.";
                        }
                        if (copperRemaining > 0)
                        {
                            message += $" and {copperRemaining} copper on quick healing.";
                        }
                        Main.NewText(message);
                    }

                    if (silverRemaining > 0 && platRemaining == 0 && goldRemaining == 0)
                    {
                        string message = $"You just spent {silverRemaining} silver";
                        if (copperRemaining == 0)
                        {
                            message += " on quick healing.";
                        }
                        if (copperRemaining > 0)
                        {
                            message += $" and {copperRemaining} copper on quick healing.";
                        }
                        Main.NewText(message);
                    }
                    if (cost > 0 && silverRemaining == 0 && platRemaining == 0 && goldRemaining == 0)
                    {
                        Main.NewText($"You just spent {remainingMoney} copper on quick healing.");
                    }
                }


                if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && Wallet >= cost && debuffCount > 1 | healthMissing > 0)
                {
                    HealAndSpend(cost, Player, healthMissing);
                }

                else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && !bossCombatCheck(6400f) && Wallet >= cost && debuffCount > 0 | healthMissing > 0)
                {
                    HealAndSpend(cost, Player, healthMissing);
                }

                else if (Wallet >= cost && !ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && debuffCount > 0 | healthMissing > 0)
                {
                    HealAndSpend(cost, Player, healthMissing);
                }
                else if (Wallet < cost)
                {
                    Main.NewText("You don't have enough money to pay for a quick heal.");
                }

                else if (Wallet > cost && healthMissing == 0 && debuffCount == 0)
                {
                    Main.NewText("Health full.");
                }

                else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) == true && debuffCount == 1 && healthMissing == 0)
                {
                    Main.NewText("Health full.");
                }
                else
                {
                    Main.NewText("Couldn't quick heal.");
                }
            }
        }
    }
}