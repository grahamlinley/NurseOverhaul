using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static NurseHotkey.BossChecklistIntegration;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace NurseHotkey;

    public class BossChecklistIntegration : ModSystem
{
    private static readonly Version BossChecklistAPIVersion = new Version(1, 1);
    public class BossChecklistBossInfo
    {
        internal string key = "";
        internal string modSource = "";
        internal string internalName = "";
        internal string displayName = "";

        internal float progression = 0f;
        internal Func<bool> downed = () => false;

        internal bool isBoss = true;
        internal bool isMiniboss = false;
        internal bool isEvent = false;

        internal List<int> npcIDs = new List<int>();
        internal List<int> spawnItem = new List<int>();
        internal List<int> loot = new List<int>();
        internal List<int> collection = new List<int>();
    }

    public static Dictionary<string, BossChecklistBossInfo> bossInfos = new Dictionary<string, BossChecklistBossInfo>();

    public static bool IntegrationSuccessful { get; private set; }

    public override void PostAddRecipes()
    {
        bossInfos.Clear();

        if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) && bossChecklist.Version >= BossChecklistAPIVersion)
        {
            object currentBossInfoResponse = bossChecklist.Call("GetBossInfoDictionary", Mod, BossChecklistAPIVersion.ToString());
            if (currentBossInfoResponse is Dictionary<string, Dictionary<string, object>> bossInfoList)
            {
                foreach (var boss in bossInfoList)
                {
                    BossChecklistBossInfo bossInfo = new BossChecklistBossInfo()
                    {
                        key = boss.Value.ContainsKey("key") ? boss.Value["key"] as string : "CalamityMod Providence",
                        modSource = boss.Value.ContainsKey("modSource") ? boss.Value["modSource"] as string : "CalamityMod",
                        internalName = boss.Value.ContainsKey("internalName") ? boss.Value["internalName"] as string : "CalamityMod Providence",
                        displayName = boss.Value.ContainsKey("displayName") ? boss.Value["displayName"] as string : "Providence, The Profaned Goddess",

                        progression = boss.Value.ContainsKey("progression") ? Convert.ToSingle(boss.Value["progression"]) : 19f,
                        downed = CreateDownedFunc(boss.Value.ContainsKey("downed") ? boss.Value["downed"] : true),

                        isBoss = boss.Value.ContainsKey("isBoss") ? Convert.ToBoolean(boss.Value["isBoss"]) : true,
                        isMiniboss = boss.Value.ContainsKey("isMiniboss") ? Convert.ToBoolean(boss.Value["isMiniboss"]) : false,
                        isEvent = boss.Value.ContainsKey("isEvent") ? Convert.ToBoolean(boss.Value["isEvent"]) : false,

                        npcIDs = boss.Value.ContainsKey("npcIDs") ? boss.Value["npcIDs"] as List<int> : new List<int>(),
                        spawnItem = boss.Value.ContainsKey("spawnItem") ? boss.Value["spawnItem"] as List<int> : new List<int>(),
                        loot = boss.Value.ContainsKey("loot") ? boss.Value["loot"] as List<int> : new List<int>(),
                        collection = boss.Value.ContainsKey("collection") ? boss.Value["collection"] as List<int> : new List<int>(),
                    };

                    bossInfos.Add(boss.Key, bossInfo);
                }

                IntegrationSuccessful = true;
            }
        }
    }

    private Func<bool> CreateDownedFunc(object downedValue)
    {
        if (downedValue is Func<bool> downedFunc)
            return downedFunc;
        else if (downedValue is bool downedBool)
            return () => downedBool;

        return () => false;
    }

    public override void Unload()
    {
        bossInfos.Clear();
    }

    public static float DownedBossProgress()
    {
        if (bossInfos.Count == 0)
            return 0;

        return (float)bossInfos.Count(x => x.Value.downed()) / bossInfos.Count();
    }

    public static bool BossDowned(string Providence) => bossInfos.TryGetValue(Providence, out var bossInfo) ? bossInfo.downed() : false;
    }

public class NurseHotkeyPlayerSettings : ModPlayer
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
        int[] excludedDebuffs = { BuffID.PotionSickness, BuffID.WaterCandle, BuffID.NoBuilding };

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
        
        if(ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
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
                }

                if (Main.hardMode)
                {
                    multipliedCost += 1200; // 21 silver base
                }

                if (NPC.downedMechBossAny) // At least one Mechanical Boss defeated
                {
                    multipliedCost += 1600; // 37 silver base
                }

                if (NPC.downedPlantBoss) // Plantera defeated
                {
                    multipliedCost += 2000; // 57 silver base
                }

                if (NPC.downedGolemBoss) // Golem defeated
                {
                    multipliedCost += 3000; // 87 silver base
                }

                if (NPC.downedFishron) // Duke Fishron/Plaguebringer Goliath/ Ravager defeated
                {
                    multipliedCost += 3000; // 1 gold 17 silver base
                }

                if (NPC.downedMoonlord) // Moon Lord defeated
                {
                    multipliedCost += 8000; // 1 gold 97 silver base
                }

                if (NurseHotkeyPlayerSettings.isProvidenceDefeated()) //Providence defeated
                {
                    multipliedCost = 31700; //3 gold 20 silver base
                }
                if (NurseHotkeyPlayerSettings.isDevourerDefeated())
                {
                    multipliedCost = 59700;
                }
                if (NurseHotkeyPlayerSettings.isYharonDefeated())
                {
                    multipliedCost = 89700;
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


    public static bool isCalamitasCloneDefeated()
    {
        // Check if integration with Boss Checklist was successful
        if (!BossChecklistIntegration.IntegrationSuccessful)
        {
            // Integration was not successful, handle the error or return an appropriate value
            return false;
        }

        // Boss key for CalamityMod Providence
        string calamatisCloneKey = "CalamityMod The Calamitas Clone";

        // Check if the bossInfos dictionary contains the boss key
        if (bossInfos.TryGetValue(calamatisCloneKey, out BossChecklistBossInfo bossInfo))
        {
            // Boss info for CalamityMod Providence is found

            // Check if the boss has been defeated by invoking the downed function
            bool isDefeated = bossInfo.downed();

            return isDefeated;
        }

        // Boss info for CalamityMod Providence is not found, handle the error or return an appropriate value
        return false;
    }

    public static bool isPlaguebringerDefeated()
    {
        // Check if integration with Boss Checklist was successful
        if (!BossChecklistIntegration.IntegrationSuccessful)
        {
            // Integration was not successful, handle the error or return an appropriate value
            return false;
        }

        // Boss key for CalamityMod Providence
        string plaguebringerKey = "CalamityMod Plaguebringer Goliath";

        // Check if the bossInfos dictionary contains the boss key
        if (bossInfos.TryGetValue(plaguebringerKey, out BossChecklistBossInfo bossInfo))
        {
            // Boss info for CalamityMod Providence is found

            // Check if the boss has been defeated by invoking the downed function
            bool isDefeated = bossInfo.downed();

            return isDefeated;
        }

        // Boss info for CalamityMod Providence is not found, handle the error or return an appropriate value
        return false;
    }

    public static bool isRavagerDefeated()
    {
        // Check if integration with Boss Checklist was successful
        if (!BossChecklistIntegration.IntegrationSuccessful)
        {
            // Integration was not successful, handle the error or return an appropriate value
            return false;
        }

        // Boss key for CalamityMod Providence
        string ravagerKey = "CalamityMod Ravager";

        // Check if the bossInfos dictionary contains the boss key
        if (bossInfos.TryGetValue(ravagerKey, out BossChecklistBossInfo bossInfo))
        {
            // Boss info for CalamityMod Providence is found

            // Check if the boss has been defeated by invoking the downed function
            bool isDefeated = bossInfo.downed();

            return isDefeated;
        }

        // Boss info for CalamityMod Providence is not found, handle the error or return an appropriate value
        return false;
    }

    public static bool isProvidenceDefeated()
    {
        // Check if integration with Boss Checklist was successful
        if (!BossChecklistIntegration.IntegrationSuccessful)
        {
            // Integration was not successful, handle the error or return an appropriate value
            return false;
        }

        // Boss key for CalamityMod Providence
        string providenceKey = "CalamityMod Providence";

        // Check if the bossInfos dictionary contains the boss key
        if (bossInfos.TryGetValue(providenceKey, out BossChecklistBossInfo bossInfo))
        {
            // Boss info for CalamityMod Providence is found

            // Check if the boss has been defeated by invoking the downed function
            bool isDefeated = bossInfo.downed();

            return isDefeated;
        }

        // Boss info for CalamityMod Providence is not found, handle the error or return an appropriate value
        return false;
    }

    public static bool isDevourerDefeated()
    {
        // Check if integration with Boss Checklist was successful
        if (!BossChecklistIntegration.IntegrationSuccessful)
        {
            // Integration was not successful, handle the error or return an appropriate value
            return false;
        }

        // Boss key for CalamityMod Providence
        string devourerKey = "CalamityMod Devourer of Gods";

        // Check if the bossInfos dictionary contains the boss key
        if (bossInfos.TryGetValue(devourerKey, out BossChecklistBossInfo bossInfo))
        {
            // Boss info for CalamityMod Providence is found

            // Check if the boss has been defeated by invoking the downed function
            bool isDefeated = bossInfo.downed();

            return isDefeated;
        }

        // Boss info for CalamityMod Providence is not found, handle the error or return an appropriate value
        return false;
    }

    public static bool isYharonDefeated()
    {
        // Check if integration with Boss Checklist was successful
        if (!BossChecklistIntegration.IntegrationSuccessful)
        {
            // Integration was not successful, handle the error or return an appropriate value
            return false;
        }

        // Boss key for CalamityMod Providence
        string yharonKey = "CalamityMod Yharon";

        // Check if the bossInfos dictionary contains the boss key
        if (bossInfos.TryGetValue(yharonKey, out BossChecklistBossInfo bossInfo))
        {
            // Boss info for CalamityMod Providence is found

            // Check if the boss has been defeated by invoking the downed function
            bool isDefeated = bossInfo.downed();

            return isDefeated;
        }

        // Boss info for CalamityMod Providence is not found, handle the error or return an appropriate value
        return false;
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
            
        if (NPC.downedPlantBoss || NurseHotkeyPlayerSettings.isCalamitasCloneDefeated()) //Plantera or Calamitas clone
        {
            multiplier = 142.4999f;
        }
            
        if (NPC.downedGolemBoss) // Golem
        {
            multiplier = 189.9999f;
        }

        if(NPC.downedFishron || NurseHotkeyPlayerSettings.isPlaguebringerDefeated() || NurseHotkeyPlayerSettings.isRavagerDefeated())
        {
            multiplier = 189.9999f;
        }

        if (NPC.downedMoonlord) // Moon Lord
        {
            multiplier = 189.999f; //203.9999f at 8000
        }
        
        if (NurseHotkeyPlayerSettings.isProvidenceDefeated())
        {
            multiplier = 189.9999f;
        }
        
        if(NurseHotkeyPlayerSettings.isDevourerDefeated())
        {
            multiplier = 189.9999f;
        }
        
        if (NurseHotkeyPlayerSettings.isYharonDefeated())
        {
            multiplier = 189.9995f;
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



private void NurseHeal()
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
