using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using rail;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseHotkey;


public class NurseHotkeyPlayer : ModPlayer
{
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
        int[] excludedDebuffs = { BuffID.PotionSickness, BuffID.WaterCandle, BuffID.NoBuilding, BuffID.Werewolf };

        // Check if the given buffType is in the excludedDebuffs list
        return excludedDebuffs.Contains(buffType);
    }

    public static float GetHealCost(int healthMissing, Player player)
    {
        float difficultyFactor = 1; // Set the difficulty factor to 1 by default
        float baseCost = healthMissing;
        float debuffCount = GetDebuffCount(player); // Get the count of debuffs for the player

        if (Main.expertMode)
        {
            difficultyFactor = 2; // If the game is in Expert mode, set the difficulty factor to 2
        }
        if (Main.masterMode)
        {
            difficultyFactor = 2; // If the game is in Master mode, set the difficulty factor to 3
        }

        float preCost = (baseCost * difficultyFactor); // Calculate the total cost of the heal based on the difficulty factor

        if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
        {
            preCost += debuffCount * 200; // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
        }

        else
        {
            preCost += debuffCount * 100; // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
        }

        float multipliedCost = 0;

        if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && debuffCount > 0 || healthMissing > 0) //increases base cost by set amount based on https://calamitymod.fandom.com/wiki/Town_NPCs/Nurse_Price_Scaling
                                                                                                          //NOTE: Not sure if the prices are inaacurate on that page or if the scaling is different, but values derived below are hand tested on a max happiness Nurse and accurate for these 4 tests:
                                                                                                          //1. Max health 1 debuff 2. Missing 10 health 3. Missing 490 health (or whatever other large number your mod uses) 4. Missing somewhere in between those two number to test float accuracy
        {
            if (NPC.downedBoss1) // Eye of Cthulhu defeated
            {
                multipliedCost += 300; // Increased price (3 silver base). Note this differs from what is on the Calamity wiki for whatever reason. Base prices from here are hand calculated
            }


            if (NPC.downedBoss3) // Skeletron defeated 
            {
                multipliedCost += 600; // Increased price (9 silver base)

                if (!NPC.downedBoss1) //these nested ifs check if the player has skipped bosses then applies additional cost 
                {
                    multipliedCost += 300;
                }
            }

            if (Main.hardMode)
            {
                multipliedCost += 1200; // 21 silver base

                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if(!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }

            if (NPC.downedMechBossAny) // At least one Mechanical Boss defeated
            {
                multipliedCost += 1600; // 37 silver base

                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }

            if (NPC.downedPlantBoss || BossChecklistIntegration.isCalamitasCloneDefeated()) // Plantera defeated
            {
                multipliedCost += 2000; // 57 silver base
                
                if(!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }

            if (NPC.downedGolemBoss) // Golem defeated
            {
                multipliedCost += 3000; // 87 silver base
                if (!NPC.downedPlantBoss)
                {
                    multipliedCost += 2000;
                }
                if (!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }

            if (NPC.downedFishron || BossChecklistIntegration.isPlaguebringerDefeated() || BossChecklistIntegration.isRavagerDefeated()) // Duke Fishron/Plaguebringer Goliath/ Ravager defeated

            {
                multipliedCost += 3000; // 1 gold 17 silver base
                if(!NPC.downedGolemBoss)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedPlantBoss)
                {
                    multipliedCost += 2000;
                }
                if (!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }

            if (NPC.downedMoonlord) // Moon Lord defeated
            {
                multipliedCost += 8000; // 1 gold 97 silver base

                if(!NPC.downedFishron)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedGolemBoss)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedPlantBoss)
                {
                    multipliedCost += 2000;
                }
                if (!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }

            if (BossChecklistIntegration.isProvidenceDefeated()) //Providence defeated
            {
                multipliedCost += 12000; //3 gold 20 silver base
                if(!NPC.downedMoonlord)
                {
                    multipliedCost += 8000;
                }
                if (!NPC.downedFishron)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedGolemBoss)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedPlantBoss)
                {
                    multipliedCost += 2000;
                }
                if (!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }
            if (BossChecklistIntegration.isDevourerDefeated())
            {
                multipliedCost += 28000; //5.97

                if(!BossChecklistIntegration.isProvidenceDefeated())
                {
                    multipliedCost += 12000;
                }

                if (!NPC.downedMoonlord)
                {
                    multipliedCost += 8000;
                }
                if (!NPC.downedFishron)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedGolemBoss)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedPlantBoss)
                {
                    multipliedCost += 2000;
                }
                if (!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }
            if (BossChecklistIntegration.isYharonDefeated())
            {
                multipliedCost += 30000; //897
                if(!BossChecklistIntegration.isRavagerDefeated())
                {
                    multipliedCost += 28000;
                }
                if (!BossChecklistIntegration.isProvidenceDefeated())
                {
                    multipliedCost += 12000;
                }

                if (!NPC.downedMoonlord)
                {
                    multipliedCost += 8000;
                }
                if (!NPC.downedFishron)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedGolemBoss)
                {
                    multipliedCost += 3000;
                }
                if (!NPC.downedPlantBoss)
                {
                    multipliedCost += 2000;
                }
                if (!NPC.downedMechBossAny)
                {
                    multipliedCost += 1600;
                }
                if (!Main.hardMode)
                {
                    multipliedCost += 1200;
                }
                if (!NPC.downedBoss1)
                {
                    multipliedCost += 300;
                }
                if (!NPC.downedBoss3)
                {
                    multipliedCost += 600;
                }
            }
        }

        if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
        {
            multipliedCost += preCost * GetCalamityBossMultiplier();
        }
        else
        {
            multipliedCost += preCost * GetBossMultiplier(); // Multiply the total cost by the boss defeat multiplier
        }

        return multipliedCost;
    }

    public bool CombatCheck()
    {
        if (Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<NurseHotkeyPlayer>().IsPlayerFightingBoss())
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerFightingBoss()
    {
        return NPC.AnyNPCs(NPCID.KingSlime) ||
               NPC.AnyNPCs(NPCID.EyeofCthulhu) ||
               NPC.AnyNPCs(NPCID.EaterofWorldsHead) ||
               NPC.AnyNPCs(NPCID.BrainofCthulhu) ||
               NPC.AnyNPCs(NPCID.QueenBee) ||
               NPC.AnyNPCs(NPCID.SkeletronHead) ||
               NPC.AnyNPCs(NPCID.WallofFlesh) ||
               NPC.AnyNPCs(NPCID.TheDestroyer) ||
               NPC.AnyNPCs(NPCID.SkeletronPrime) ||
               NPC.AnyNPCs(NPCID.Retinazer) ||
               NPC.AnyNPCs(NPCID.Spazmatism) ||
               NPC.AnyNPCs(NPCID.Plantera) ||
               NPC.AnyNPCs(NPCID.Golem) ||
               NPC.AnyNPCs(NPCID.DukeFishron) ||
               NPC.AnyNPCs(NPCID.CultistBoss) ||
               NPC.AnyNPCs(NPCID.MoonLordCore);
        // Add more boss NPCs as needed
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

    private static float GetCalamityBossMultiplier()
    {
        float multiplier = .94999f;

        if (NPC.downedBoss1) // Eye of Cthulhu
        {
            multiplier = 2.8499f;
        }

        if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
        {
            multiplier = 9.4999f;
        }

        if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
        {
            multiplier = 23.7499f;
        }

        if (Main.hardMode) // Wall of Flesh
        {
            multiplier = 56.9999f;
        }

        if (NPC.downedMechBossAny) //Any mech boss
        {
            multiplier = 94.9999f;
        }

        if (NPC.downedPlantBoss) //Plantera or Calamitas clone
        {
            multiplier = 142.4999f;
        }

        if(BossChecklistIntegration.isCalamitasCloneDefeated())
        {
            multiplier = 142.4999f;

            if (!NPC.downedBoss1 && !NPC.downedBoss2 && !NPC.downedBoss3 && !Main.hardMode && !NPC.downedMechBossAny && !NPC.downedPlantBoss)
            {
                multiplier = .94999f;
            }

            /*
            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier = 2.8499f;
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier = 9.4999f;
            }

            if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier = 23.7499f;
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier = 56.9999f;
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier = 94.9999f;
            }
            */
        }

        if (NPC.downedGolemBoss) // Golem
        {
            multiplier = 189.9999f;
        }

        if (NPC.downedFishron || BossChecklistIntegration.isPlaguebringerDefeated() || BossChecklistIntegration.isRavagerDefeated())
        {
            multiplier = 189.9999f;

            if(!NPC.downedBoss1 && !NPC.downedBoss2 && !NPC.downedBoss3 && !Main.hardMode && !NPC.downedMechBossAny && !NPC.downedPlantBoss && !NPC.downedGolemBoss)
            {
                multiplier = .94999f;
            }

            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier = 2.8499f;
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier = 9.4999f;
            }

            if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier = 23.7499f;
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier = 56.9999f;
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier = 94.9999f;
            }

            if (NPC.downedPlantBoss || BossChecklistIntegration.isCalamitasCloneDefeated()) //Plantera or Calamitas clone
            {
                multiplier = 142.4999f;
            }

            if (NPC.downedGolemBoss) // Golem
            {
                multiplier = 189.9999f;
            }
            }

        if (NPC.downedMoonlord)
        {
            multiplier = 189.9999f;

            if (!NPC.downedBoss1 && !NPC.downedBoss2 && !NPC.downedBoss3 && !Main.hardMode && !NPC.downedMechBossAny && !NPC.downedPlantBoss && !NPC.downedGolemBoss) // && !NPC.downedFishron || !BossChecklistIntegration.isPlaguebringerDefeated() || !BossChecklistIntegration.isRavagerDefeated())
            {
                multiplier = .94999f;
            }

            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier = 2.8499f;
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier = 9.4999f;
            }

            if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier = 23.7499f;
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier = 56.9999f;
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier = 94.9999f;
            }

            if (NPC.downedPlantBoss || BossChecklistIntegration.isCalamitasCloneDefeated()) //Plantera or Calamitas clone
            {
                multiplier = 142.4999f;
            }

            if (NPC.downedGolemBoss) // Golem
            {
                multiplier = 189.9999f;
            }
        }
            if (BossChecklistIntegration.isProvidenceDefeated())
            {
                multiplier = 189.9999f;
            if (!NPC.downedBoss1 && !NPC.downedBoss2 && !NPC.downedBoss3 && !Main.hardMode && !NPC.downedMechBossAny && !NPC.downedPlantBoss && !NPC.downedGolemBoss && !NPC.downedFishron || !BossChecklistIntegration.isPlaguebringerDefeated() || !BossChecklistIntegration.isRavagerDefeated() && !NPC.downedMoonlord)
            {
                multiplier = .94999f;
            }

            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier = 2.8499f;
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier = 9.4999f;
            }

            if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier = 23.7499f;
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier = 56.9999f;
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier = 94.9999f;
            }

            if (NPC.downedPlantBoss || BossChecklistIntegration.isCalamitasCloneDefeated()) //Plantera or Calamitas clone
            {
                multiplier = 142.4999f;
            }

            if (NPC.downedGolemBoss) // Golem
            {
                multiplier = 189.9999f;
            }
        }

            if (BossChecklistIntegration.isDevourerDefeated())
            {
                multiplier = 189.9999f;
            if (!NPC.downedBoss1 && !NPC.downedBoss2 && !NPC.downedBoss3 && !Main.hardMode && !NPC.downedMechBossAny && !NPC.downedPlantBoss && !NPC.downedGolemBoss && !NPC.downedFishron || !BossChecklistIntegration.isPlaguebringerDefeated() 
                || !BossChecklistIntegration.isRavagerDefeated() && !NPC.downedMoonlord && !BossChecklistIntegration.isProvidenceDefeated())
            {
                multiplier = .94999f;
            }

            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier = 2.8499f;
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier = 9.4999f;
            }

            if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier = 23.7499f;
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier = 56.9999f;
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier = 94.9999f;
            }

            if (NPC.downedPlantBoss || BossChecklistIntegration.isCalamitasCloneDefeated()) //Plantera or Calamitas clone
            {
                multiplier = 142.4999f;
            }

            if (NPC.downedGolemBoss) // Golem
            {
                multiplier = 189.9999f;
            }
        }

            if (BossChecklistIntegration.isYharonDefeated())
            {
                multiplier = 189.9995f;
            if (!NPC.downedBoss1 && !NPC.downedBoss2 && !NPC.downedBoss3 && !Main.hardMode && !NPC.downedMechBossAny && !NPC.downedPlantBoss && !NPC.downedGolemBoss && !NPC.downedFishron || !BossChecklistIntegration.isPlaguebringerDefeated()
                || !BossChecklistIntegration.isRavagerDefeated() && !NPC.downedMoonlord && !BossChecklistIntegration.isProvidenceDefeated() && BossChecklistIntegration.isDevourerDefeated())
            {
                multiplier = .94999f;
            }

            if (NPC.downedBoss1) // Eye of Cthulhu
            {
                multiplier = 2.8499f;
            }

            if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
            {
                multiplier = 9.4999f;
            }

            if (NPC.downedBoss3 || NPC.downedQueenBee) // Skeletron or Queen Bee
            {
                multiplier = 23.7499f;
            }

            if (Main.hardMode) // Wall of Flesh
            {
                multiplier = 56.9999f;
            }

            if (NPC.downedMechBossAny) //Any mech boss
            {
                multiplier = 94.9999f;
            }

            if (NPC.downedPlantBoss || BossChecklistIntegration.isCalamitasCloneDefeated()) //Plantera or Calamitas clone
            {
                multiplier = 142.4999f;
            }

            if (NPC.downedGolemBoss) // Golem
            {
                multiplier = 189.9999f;
            }
        }

            return multiplier;
    }
       

    private static float GetBossMultiplier()
    {
        float multiplier = .84f; // NEED TO FIX EVERYTHING

        if (NPC.downedBoss1) // Eye of Cthulhu
        {
            multiplier = 1.8499f;
        }
        if (NPC.downedBoss2) // Eater of Worlds/Brain of Cthulhu
        {
            multiplier = 8.4999f;
        }
        if (NPC.downedBoss3) // Skeletron
        {
            multiplier = 22.7499f;
        }
        if (Main.hardMode)
        {
            multiplier = 55.9999f;
        }
        if (NPC.downedMechBossAny)
        {
            multiplier = 93.9999f;
        }
        if (NPC.downedPlantBoss)
        {
            multiplier = 141.4999f;
        }
        if (NPC.downedGolemBoss)
        {
            multiplier = 188.9999f;
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



    public void NurseHeal()
    {
        NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];
        Player player = Main.LocalPlayer;

        if (nurse != null && Vector2.Distance(Player.Center, nurse.Center) <= 300)
        {
            int healthMissing = Player.statLifeMax2 - Player.statLife;
            float cost = GetHealCost(healthMissing, Player);
            float totalMoney = 0;
            float debuffCount = GetDebuffCount(player); // Get the count of debuffs for the player

            int GetPlayerTotalMoney(int playerIndex)
            {
                Player player = Main.player[playerIndex];

                // Access the player's inventory
                Item[] inventory = player.inventory;

                // Calculate the total money from inventory
                int totalMoney = CalculateMoneyFromItems(inventory);

                // Check if remaining cost is greater than zero
                float remainingCost = cost - totalMoney;
                if (remainingCost > 0)
                {
                    // Access the player's Piggy Bank items
                    Item[] piggyBankItems = player.bank.item;

                    // Calculate the total money from Piggy Bank
                    totalMoney += CalculateMoneyFromItems(piggyBankItems);

                    // Deduct the remaining cost from Piggy Bank if needed
                    if (totalMoney >= remainingCost)
                    {
                        int deductedMoney = DeductMoneyFromItems(piggyBankItems, (int)remainingCost);
                        totalMoney -= deductedMoney;
                    }
                }

                return totalMoney;
            }

            // Helper method to deduct money from items
            int DeductMoneyFromItems(Item[] items, int amount)
            {
                int deductedMoney = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    Item item = items[i];

                    if (item.type == ItemID.CopperCoin)
                    {
                        if (item.stack >= amount)
                        {
                            item.stack -= amount;
                            deductedMoney += amount;
                            break;
                        }
                        else
                        {
                            amount -= item.stack;
                            deductedMoney += item.stack;
                            item.stack = 0;
                        }
                    }
                    else if (item.type == ItemID.SilverCoin)
                    {
                        int silverValue = item.stack * 100;

                        if (silverValue >= amount)
                        {
                            int deductedSilver = amount / 100;
                            item.stack -= deductedSilver;
                            deductedMoney += deductedSilver * 100;
                            break;
                        }
                        else
                        {
                            amount -= silverValue;
                            deductedMoney += item.stack * 100;
                            item.stack = 0;
                        }
                    }
                    else if (item.type == ItemID.GoldCoin)
                    {
                        int goldValue = item.stack * 10000;

                        if (goldValue >= amount)
                        {
                            int deductedGold = amount / 10000;
                            item.stack -= deductedGold;
                            deductedMoney += deductedGold * 10000;
                            break;
                        }
                        else
                        {
                            amount -= goldValue;
                            deductedMoney += item.stack * 10000;
                            item.stack = 0;
                        }
                    }
                    else if (item.type == ItemID.PlatinumCoin)
                    {
                        int platinumValue = item.stack * 1000000;

                        if (platinumValue >= amount)
                        {
                            int deductedPlatinum = amount / 1000000;
                            item.stack -= deductedPlatinum;
                            deductedMoney += deductedPlatinum * 1000000;
                            break;
                        }
                        else
                        {
                            amount -= platinumValue;
                            deductedMoney += item.stack * 1000000;
                            item.stack = 0;
                        }
                    }
                }

                return deductedMoney;
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
            int intCost = Convert.ToInt32(cost);

            if (Wallet >= cost && debuffCount > 0 | healthMissing > 0)
            {
                CureAllDebuffs(Player); //debuffs destroyed
                Player.BuyItem(intCost); //pay up 
                int healAmount = healthMissing; //ok how much this mothasucka need
                Player.statLife += healAmount; //puts the item in the bag

                if (healAmount > 0) //needed for healing debuffs and no health (i.e. stink potion)
                {
                    Player.HealEffect(healAmount); //"ok here u go sir, have a nice day :)"
                }

                //all this shit just calculates your money then gives you a message 
                int remainingMoney = (int)cost;
                int platRemaining = remainingMoney / 1000000;
                int goldRemaining = (remainingMoney % 1000000) / 10000;
                int silverRemaining = (remainingMoney % 10000) / 100;
                int copperRemaining = remainingMoney % 100;

                if (platRemaining > 0)
                {
                    Main.NewText($"You just spent {platRemaining} platinum {goldRemaining} gold {silverRemaining} silver and {copperRemaining} copper on quick healing.");
                }
                if (goldRemaining > 0 && platRemaining == 0)
                {
                    Main.NewText($"You just spent {goldRemaining} gold {silverRemaining} silver and {copperRemaining} copper for healing.");
                }
                if (silverRemaining > 0 && platRemaining == 0 && goldRemaining == 0)
                {
                    Main.NewText($"You just spent {silverRemaining} silver and {copperRemaining} copper on quick healing.");
                }
                if (cost > 0 && silverRemaining == 0 && platRemaining == 0 && goldRemaining == 0)
                {
                    Main.NewText($"You just spent {remainingMoney} copper on quick healing.");
                }
            }

            else if (Wallet < cost)
            {
                Main.NewText("You don't have enough money to pay for a quick heal.");
            }

            else if (Wallet > cost && healthMissing == 0 && debuffCount == 0)
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
