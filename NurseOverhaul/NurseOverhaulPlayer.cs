using Microsoft.Xna.Framework;
using NurseOverhaul.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseOverhaul
{
    public class NurseOverhaulPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            Player player = Main.LocalPlayer;
            int nurseNPCID = NPC.FindFirstNPC(NPCID.Nurse);

            if (nurseNPCID != -1)
            {
                if (PlayerIsInRangeOfNurse() && PlayerHasTransponder())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.NurseInRange>(), 2); // Applies our custom buff if the player is in range of the nurse and has a transponder item
                }

                if (PlayerHasItem(player, ModContent.ItemType<NurseWalkieTalkie>()) && PlayerIsInSmallSweetSpot())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.NurseSweetSpot>(), 2);
                }
                if (PlayerHasItem(player, ModContent.ItemType<SurfaceTransponder>()) && PlayerIsInMediumSweetSpot())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.NurseSweetSpot>(), 2);
                }
                if (PlayerHasItem(player, ModContent.ItemType<PlatinumInsurance>()) && PlayerIsInLargeSweetSpot())
                {
                    player.AddBuff(ModContent.BuffType<Buffs.NurseSweetSpot>(), 2);
                }
            }
        }


        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            int nurseNPCID = NPC.FindFirstNPC(NPCID.Nurse);

            if (nurseNPCID != -1)
            {
                if (NurseOverhaul.NurseHealHotkey.JustPressed)
                {
                    NurseHeal(); // If the Nurse exists, and the player presses the designated hotkey, calls our NurseHeal method
                }
            }
        }


        public static bool ExcludedDebuff(int buffType)
        {
            // List of debuffs to be excluded from being cured or from being counted in price
            // matched against source code
            int[] excludedDebuffs = { BuffID.Werewolf, BuffID.Merfolk, BuffID.Campfire, BuffID.HeartLamp, BuffID.PotionSickness, BuffID.WaterCandle, BuffID.NoBuilding, BuffID.DryadsWard, BuffID.PeaceCandle,
            BuffID.Honey, BuffID.StarInBottle, BuffID.CatBast, BuffID.Sunflower, BuffID.NeutralHunger, BuffID.Hunger, BuffID.Starving, BuffID.ShadowCandle, BuffID.MonsterBanner};

            // Check if the given buffType is in the excludedDebuffs list
            return excludedDebuffs.Contains(buffType);
        }

        // gets us a cost for our NurseHeal method later based on health missing, debuffs, mods installed, boss combat status, happiness values, and game difficulty
        // also sets a threshhold for healing with items, and adds custom prices to vanilla based on if the player is fighting a boss/in an event
        public static float GetHealCost(int healthMissing, Player player)
        {
            int nurseNPCId = NPC.FindFirstNPC(NPCID.Nurse);
            NPC nurseNPC = Main.npc[nurseNPCId];
            var currentShoppingSettings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, nurseNPC); // Accesses an actual number representing how the Nurse feels towards you
            float nurseHappinessAdjustment = (float)currentShoppingSettings.PriceAdjustment; // Floated number for Nurse's happiness
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
                difficultyFactor = 2; // If the game is in Master mode, also set the difficulty factor to 2
            }

            float preCost = (baseCost * difficultyFactor); // First step, we need to multiply the raw health number with the difficulty factor. This establishes our "preCost"


            if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) == true && debuffCount - 1 > 0) //This specific check is important for Calamity as a pseudo weak reference to
                                                                                                                               //Calamity's Boss Debuff. Because Calamity bosses give a debuff when you're
                                                                                                                               //within a certain range and our mod cures debuffs and charges people for it,
                                                                                                                               //you charge people everytime for 1 debuff worth of healing even if they are
                                                                                                                               //at full health without this check.
            {
                preCost += (debuffCount - 1) * (100 * difficultyFactor); // READ NOTE BELOW AND APPLY TO THIS TOO
                                                                         // Logic for next 3 if statements are similar. Takes debuff count, multiplies that by 100 or 200 if master/expert.
                                                                         // This is because the cost of healing 1 debuff is the same as 100 health in normal mode
            }

            else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod")) // If not in combat, debuffs counted differently. Will delete Calamity specific check if the same as normal (which it should be)
            {
                preCost += debuffCount * (100 * difficultyFactor); // !!!!!!!!!!!!!!!!!!!!!!CHECK THIS WHEN CALAMITY RELEASES 1.4.4. CHECK IF NON-EXPERT,
                                                                   // NON-MASTER IS 200. IF NOT, JUST DO THE SAME AS BELOW AND MULTIPLY BY DIFFICULTY FACTOR
            }

            else
            {
                preCost += debuffCount * (100 * difficultyFactor); // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
            }

            float calamityBaseCost = 0;
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);

            if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && debuffCount > 0 | healthMissing > 0) //increases base cost by set amount based on https://calamitymod.fandom.com/wiki/Town_NPCs/Nurse_Price_Scaling
                                                                                                              //NOTE: Not sure if the prices are inaacurate on that page or if the scaling is different,
                                                                                                              //but values derived below are hand tested in multiple scenarios with these 4 tests:
                                                                                                              //1. Max health 1 debuff 2. Missing 10 health 3. Missing 490 health (for decimal accuracy)
                                                                                                              //4. Missing somewhere in between those two number to test general accuracy
            {
                if ((bool)Calamity.Call("Downed", "yharon")) // Start at last boss then else if for everything else descending below. This way, will check if you killed last boss first, then apply pricing.
                {
                    calamityBaseCost = 89700; //8 gold 97 silver base
                }
                else if ((bool)Calamity.Call("Downed", "dog"))
                {
                    calamityBaseCost = 59700; //5 gold 97 silver base
                }
                else if ((bool)Calamity.Call("Downed", "providence"))
                {
                    calamityBaseCost = 31700; //3 gold 17 silver base
                }
                else if (NPC.downedMoonlord) // Moon Lord defeated
                {
                    calamityBaseCost = 19700; // 1 gold 97 silver base
                }
                else if (NPC.downedFishron | ((bool)Calamity.Call("Downed", "plaguebringer goliath")) | ((bool)Calamity.Call("Downed", "ravager"))) // Duke Fishron ir Plaguebringer Goliath or Ravager defeated
                {
                    calamityBaseCost = 11700; // 1 gold 17 silver base
                }
                else if (NPC.downedGolemBoss)
                {
                    calamityBaseCost = 8700; // 87 silver base
                }
                else if (NPC.downedPlantBoss | ((bool)Calamity.Call("Downed", "calamitas doppelganger"))) // Plantera defeated
                {
                    calamityBaseCost = 5700; // 57 silver base
                }
                else if (NPC.downedMechBossAny) // At least one Mechanical Boss defeated
                {
                    calamityBaseCost = 3700; // 37 silver base
                }
                else if (Main.hardMode) // Wall of Flesh defeated
                {
                    calamityBaseCost = 2100; // 21 silver base
                }
                else if (NPC.downedBoss3) // Skeletron defeated 
                {
                    calamityBaseCost = 900; // 9 silver base
                }
                else if (NPC.downedBoss1) // Eye of Cthulhu defeated
                {
                    calamityBaseCost = 300; // 3 silver base
                }
            }

            float nurseDisplayCost;

            if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) == true && debuffCount > 1) // Another weak reference check for Calamity boss debuff
            {
                calamityBaseCost += preCost * GetMultiplier() * nurseHappinessAdjustment; // Gets the boss multiplier from below, multiplies that with our preCost above and the
                                                                                          // numerical value of happiness which modifies prices and gets our cost we will use to
                                                                                          // heal or modify based on conditions like the ones set out below. Also adds it to Calamity's base price
                nurseDisplayCost = calamityBaseCost * 5; // Manually calculating 5 times price increase of Calamity
            }
            else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod")) // Condition needed because Calamity's base cost system works differently from Vanilla
            {
                nurseDisplayCost = calamityBaseCost + (preCost * GetMultiplier() * nurseHappinessAdjustment);
            }
            else
            {
                nurseDisplayCost = preCost * GetMultiplier() * nurseHappinessAdjustment; // Multiply the total cost by the boss defeat multiplier
            }

            // Custom pricing to prevent spam abuse
            float finalCost;
            float flooredDisplayCost = (float)Math.Floor(nurseDisplayCost);

            if (PlayerHasItem(player, ModContent.ItemType<NurseWalkieTalkie>()) && !PlayerIsInSmallSweetSpot())
            {
                finalCost = flooredDisplayCost * 3;
            }
            else if (PlayerHasItem(player, ModContent.ItemType<SurfaceTransponder>()) && !PlayerIsInMediumSweetSpot())
            {
                finalCost = flooredDisplayCost * 3;
            }
            else if (PlayerHasItem(player, ModContent.ItemType<PlatinumInsurance>()) && !PlayerIsInLargeSweetSpot())
            {
                finalCost = flooredDisplayCost * 3;
            }
            else
                finalCost = flooredDisplayCost;
            return finalCost;
        }

        public static bool bossCombatCheck(float range) // Checks if a boss is alive in a certain range
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

        //Check for if the player is in an event
        public static bool IsEventActive(Player player)
        {

            if (Main.eclipse) return true;  // Solar Eclipse
            if (Main.bloodMoon) return true;  // Blood Moon
            if (Main.slimeRain) return true;  // Slime Rain
            if (Main.pumpkinMoon) return true;  // Pumpkin Moon
            if (Main.snowMoon) return true;  // Frost Moon
            if (Main.invasionType > 0 && Main.invasionSize > 0) return true; // Goblin Army, Martian Madness, Pirate Invasion, etc

            // No event is active
            return false;
        }

        public static int GetDebuffCount(Player player) // How many debuffs we got boss
        {
            int debuffCount = 0; // None
            for (int i = 0; i < player.buffType.Length; i++) // Loops as long as we have debuffs, keep increasing the value of i
            {
                int buffType = player.buffType[i]; // This line takes the current buff type from the array based on the loop's iteration.

                if (buffType > 0 && Main.debuff[buffType] && !ExcludedDebuff(buffType)) // If we have debuffs and the buff is a debuff, and it's not in our list of excluded debuffs
                {
                    debuffCount++; // Increment that ish home boy
                }
            }
            return debuffCount;
        }

        public static void CureAllDebuffs(Player player)
        {
            for (int i = 0; i < player.buffType.Length; i++) // Same as GetDebuffCount
            {
                int buffType = player.buffType[i];

                if (buffType > 0 && Main.debuff[buffType] && !ExcludedDebuff(buffType))
                {
                    player.DelBuff(i); // Bye bye debuff, everyone say bye bye!
                    i--; // Decrement i to account for the removed buff
                }
            }
        }

        public static bool IsBossAlive(string bossName)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.boss && npc.FullName == bossName) // If it's a boss, then yes. If not a boss, then no.
                {
                    return true;
                }
            }
            return false;
        }

        // My finest work
        private static float GetMultiplier()
        {
            int nurseNPCId = NPC.FindFirstNPC(NPCID.Nurse);
            NPC nurseNPC = Main.npc[nurseNPCId]; // IS IT A NURSE?
            var currentShoppingSettings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, nurseNPC); // HOW HAPPY
            float nurseHappinessAdjustment = (float)currentShoppingSettings.PriceAdjustment; // HOW HAPPY, BUT IN A FLOAT PLEASE
            float multiplier = 0; // Initializing at 0 so we can add specific multipliers to our float based on the happiness value. You might think I'm crazy, but this is what I got from my tests

            if (nurseHappinessAdjustment < 1.07 && nurseHappinessAdjustment != 1) // If the Nurse is at 1.07 happiness or below, all the way to 1 (but specifically NOT 1), the base multiplier is a
                                                                                  // a decimal roughly equal to the one below (.9995f). This specific value will yield us the most accurate results,
                                                                                  // as long as we do some more math later
            {
                multiplier += .9995f;
            }

            if (nurseHappinessAdjustment >= 1.07 | nurseHappinessAdjustment == 1) // IF the happiness is above 1.07 OR if it's exactly 1, we have a base multiplier of 1f
            {
                multiplier += 1f;
            }

            if (NPC.downedGolemBoss) // We do an else if check starting from the last boss that affects multipliers for the same reason as Calamity's base cost check. These specific bosses give a 
                                     // specific multiplier. Since we need to add up to the Nurse happiness base cost, we can't have multipliers from each boss interacting in weird ways
            {
                multiplier += 199f;
            }
            else if (NPC.downedPlantBoss)
            {
                multiplier += 149f;
            }
            else if (NPC.downedMechBossAny)
            {
                multiplier += 99f;
            }
            else if (Main.hardMode)
            {
                multiplier += 59f;
            }
            else if (NPC.downedBoss3 | NPC.downedQueenBee) // skellietron or qb
            {
                multiplier += 24f;
            }
            else if (NPC.downedBoss2) // EoW//BoC
            {
                multiplier += 9f;
            }
            else if (NPC.downedBoss1) // EoC
            {
                multiplier += 2f;
            }
            return multiplier;
        }

        private static bool PlayerHasItem(Player player, int itemType) // helper method that will determine if the player has a Nurse healing item
        {
            return player.inventory.Any(i => i.type == itemType);
        }
        private static (int horizontal, int vertical) GetRangeForItem(int itemType)
        {
            double horizontalScaleFactor = (double)Main.maxTilesX / 8400; // Scaling for smaller worlds
            double verticalScaleFactor = (double)Main.maxTilesY / 2400;

            // Initialize with default range of 0. Makes it so you can't heal without an item
            int horizontalRange = 0;
            int verticalRange = 0;

            if (itemType == ModContent.ItemType<NurseVIPBadge>()) // If you have an item, you're range is this
            {
                horizontalRange = 280; // Fixed regardless of world type
                verticalRange = 280;
            }
            if (itemType == ModContent.ItemType<NurseWalkieTalkie>())
            {
                horizontalRange = 1280;
                verticalRange = 1280;
            }
            if (itemType == ModContent.ItemType<SurfaceTransponder>())
            {
                horizontalRange = (int)(80000 * horizontalScaleFactor); // No longer fixed, scales to world size for every equation below
                verticalRange = (int)(4000 * verticalScaleFactor);
            }
            if (itemType == ModContent.ItemType<PlatinumInsurance>())
            {
                horizontalRange = (int)(134400 * horizontalScaleFactor);
                verticalRange = (int)(38400 * verticalScaleFactor);
            }

            return (horizontalRange, verticalRange);
        }

        private static (int horizontal, int vertical) GetHighestRangeForItems(Player player) //Checks highest item range, for if you have multiple items in your inventory
        {
            int[] itemsTypesToCheck = new int[]
            {
                ModContent.ItemType<NurseVIPBadge>(),
                ModContent.ItemType<NurseWalkieTalkie>(),
                ModContent.ItemType<SurfaceTransponder>(),
                ModContent.ItemType<PlatinumInsurance>()
            };

            int highestHorizontalRange = 0;
            int highestVerticalRange = 0;

            foreach (int itemType in itemsTypesToCheck) //Checking all your items
            {
                if (PlayerHasItem(player, itemType)) // If you have an item from above in your inventory
                {
                    var (horizontalRange, verticalRange) = GetRangeForItem(itemType); // Getting range for the item found in inventory
                    highestHorizontalRange = Math.Max(highestHorizontalRange, horizontalRange); // Since we're doing foreach, need to reference highestHorizontalRange to make sure each item being checked in foreach is included 
                    highestVerticalRange = Math.Max(highestVerticalRange, verticalRange);
                }
            }

            return (highestHorizontalRange, highestVerticalRange);
        }
        private static bool PlayerIsInSmallSweetSpot()
        {
            Player player = Main.LocalPlayer;
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

            bool inHorizontalRange = Math.Abs(player.Center.X - nurse.Center.X) <= 320; // Takes the absolute value regardless of axis of the player from the Nurse, and determines
                                                                                        // if it is less than or equal to the highest range allowed by the Nurse items to see if it's true
            bool inVerticalRange = Math.Abs(player.Center.Y - nurse.Center.Y) <= 320;

            return inHorizontalRange && inVerticalRange;
        }
        private static bool PlayerIsInMediumSweetSpot()
        {
            Player player = Main.LocalPlayer;
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

            bool inHorizontalRange = Math.Abs(player.Center.X - nurse.Center.X) <= 640; // Takes the absolute value regardless of axis of the player from the Nurse, and determines
                                                                                        // if it is less than or equal to the highest range allowed by the Nurse items to see if it's true
            bool inVerticalRange = Math.Abs(player.Center.Y - nurse.Center.Y) <= 640;

            return inHorizontalRange && inVerticalRange;
        }
        private static bool PlayerIsInLargeSweetSpot()
        {
            Player player = Main.LocalPlayer;
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

            bool inHorizontalRange = Math.Abs(player.Center.X - nurse.Center.X) <= 1280; // Takes the absolute value regardless of axis of the player from the Nurse, and determines
                                                                                         // if it is less than or equal to the highest range allowed by the Nurse items to see if it's true
            bool inVerticalRange = Math.Abs(player.Center.Y - nurse.Center.Y) <= 1280;

            return inHorizontalRange && inVerticalRange;
        }

        private static bool PlayerIsInRangeOfNurse()
        {
            Player player = Main.LocalPlayer;
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

            var (highestHorizontalRange, highestVerticalRange) = GetHighestRangeForItems(player); // Getting highest range with previous helper method, storing it locally in new variable

            bool inHorizontalRange = Math.Abs(player.Center.X - nurse.Center.X) <= highestHorizontalRange; // Takes the absolute value regardless of axis of the player from the Nurse, and determines
                                                                                                           // if it is less than or equal to the highest range allowed by the Nurse items to see if it's true
            bool inVerticalRange = Math.Abs(player.Center.Y - nurse.Center.Y) <= highestVerticalRange;

            return inHorizontalRange && inVerticalRange;
        }

        public static bool PlayerCanShopAtNurse()
        {
            Player player = Main.LocalPlayer;
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

            var (highestHorizontalRange, highestVerticalRange) = (64, 64);

            bool inHorizontalRange = Math.Abs(player.Center.X - nurse.Center.X) <= highestHorizontalRange; // Takes the absolute value regardless of axis of the player from the Nurse, and determines
                                                                                                           // if it is less than or equal to the highest range allowed by the Nurse items to see if it's true
            bool inVerticalRange = Math.Abs(player.Center.Y - nurse.Center.Y) <= highestVerticalRange;

            return inHorizontalRange && inVerticalRange;
        }


        private static bool PlayerHasTransponder() // Checks if you have any nurse item in your inventory
        {
            Player player = Main.LocalPlayer;
            int[] itemsTypesToCheck = new int[]
            {
                ModContent.ItemType<NurseVIPBadge>(),
                ModContent.ItemType<NurseWalkieTalkie>(),
                ModContent.ItemType<SurfaceTransponder>(),
                ModContent.ItemType<PlatinumInsurance>()
            };
            return itemsTypesToCheck.Any(itemType => PlayerHasItem(player, itemType));
        }

        // Main course
        public void NurseHeal()
        {
            NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];
            if (nurse != null) // Only will execute if the Nurse exists
            {
                if (Player.statLife > 0) // Won't execute if dead
                {
                    Player player = Main.LocalPlayer;
                    Item[] GetAllItems(int playerIndex) // Getting every item the player owns and adding it to an array
                    {
                        List<Item> allItems = new List<Item>();
                        Item[] inventory = player.inventory;

                        // Add coins from coin slots (Copper, Silver, Gold, Platinum)
                        allItems.Add(inventory[58]); // Copper Coin slot
                        allItems.Add(inventory[57]); // Silver Coin slot
                        allItems.Add(inventory[56]); // Gold Coin slot
                        allItems.Add(inventory[55]); // Platinum Coin slot

                        // Add all other items from the player's inventory
                        for (int i = 0; i < inventory.Length - 4; i++) // Excluding coin slots
                        {
                            allItems.Add(inventory[i]);
                        }

                        // Add all bank items to the list
                        allItems.AddRange(player.bank.item);
                        allItems.AddRange(player.bank2.item);
                        allItems.AddRange(player.bank3.item);
                        allItems.AddRange(player.bank4.item);

                        return allItems.ToArray();
                    }

                    if (PlayerIsInRangeOfNurse() == true) // Checking if the player meets vertical and horizontal range requirements
                    {
                        int healthMissing = Player.statLifeMax2 - Player.statLife; // Max Life - Current life = a new locally stored integer
                        int debuffCount = GetDebuffCount(player); // Always have to check debuffs
                        float cost = GetHealCost(healthMissing, Player); // Plugging this number, derived from the player, into our method with multipliers
                                                                         // and base costs to get a number we're going to use for a lot of things
                        float totalMoney = 0; // Initialized at 0 because it's going to go through some stuff
                        int GetPlayerTotalMoney(int playerIndex) // Might be able to delete palyerIndex from everything. Could be an online compatability thing?
                        {

                            // Access all the player's items
                            Item[] allItems = GetAllItems(playerIndex);


                            // Calculate the total money from all Banks
                            int totalMoney = CalculateMoneyFromItems(allItems);

                            // Check if remaining cost is greater than zero
                            float remainingCost = cost - totalMoney;
                            return totalMoney;
                        }

                        // Helper method to calculate money from items
                        int CalculateMoneyFromItems(Item[] items)
                        {
                            for (int i = 0; i < items.Length; i++)
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

                        int Wallet = GetPlayerTotalMoney(Main.myPlayer); // Get's the player's money from all their items

                        void HealAndSpend(float cost, Player player, int healthMissing) //We'll be calling this method in all conditions where the player can heal
                        {
                            CureAllDebuffs(player); //debuffs destroyed
                            float flooredFloatCost = (float)Math.Floor(cost);
                            int intCost = Convert.ToInt32(flooredFloatCost);
                            player.BuyItem(intCost); //pay up 

                            if (healthMissing > 0) //needed for healing debuffs and no health (i.e. stink potion)
                            {
                                player.statLife += healthMissing; //puts the item in the bag
                                SoundEngine.PlaySound(SoundID.Item4);
                                player.HealEffect(healthMissing); //"ok here u go sir, have a nice day :)"
                            }

                            CalculateAndPrintSpending(intCost);
                        }

                        void CalculateAndPrintSpending(int cost)
                        {
                            // Calculates your momney and outputs a message in the main chat depending on the cost
                            int remainingMoney = cost;
                            int platRemaining = remainingMoney / 1000000;
                            int goldRemaining = (remainingMoney % 1000000) / 10000;
                            int silverRemaining = (remainingMoney % 10000) / 100;
                            int copperRemaining = remainingMoney % 100;
                            int calamityBossDebuffCount = debuffCount - 1;

                            if (platRemaining > 0)
                            {
                                string message = $"You just spent {platRemaining} platinum";

                                if (goldRemaining == 0 && silverRemaining == 0 && copperRemaining == 0)
                                {
                                    message += $" on quick healing, ";
                                }
                                if (goldRemaining > 0 && (silverRemaining > 0 | copperRemaining > 0))
                                {
                                    message += $" {goldRemaining} gold";
                                }
                                if (goldRemaining > 0 && silverRemaining == 0 && copperRemaining == 0)
                                {
                                    message += $" and {goldRemaining} gold on quick healing, ";
                                }
                                if (silverRemaining > 0 && copperRemaining > 0)
                                {
                                    message += $" {silverRemaining} silver";
                                }
                                if (silverRemaining > 0 && copperRemaining == 0)
                                {
                                    message += $" and {silverRemaining} silver on quick healing, ";
                                }
                                if (copperRemaining > 0)
                                {
                                    message += $" and {copperRemaining} copper on quick healing, ";
                                }
                                if (healthMissing > 0)
                                {
                                    message += $"restoring {healthMissing} health";
                                }

                                if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount > 3)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuffs";
                                }
                                else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount == 2)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuff";
                                }
                                else if (debuffCount > 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuffs";
                                }
                                else if (debuffCount == 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuff";
                                }
                                message += ".";
                                Main.NewText(message); // Outputs actual message
                            }

                            if (goldRemaining > 0 && platRemaining == 0)
                            {
                                string message = $"You just spent {goldRemaining} gold";

                                if (silverRemaining == 0 && copperRemaining == 0)
                                {
                                    message += $" on quick healing, ";
                                }
                                if (silverRemaining > 0 && copperRemaining > 0)
                                {
                                    message += $" {silverRemaining} silver";
                                }
                                if (silverRemaining > 0 && copperRemaining == 0)
                                {
                                    message += $" and {silverRemaining} silver on quick healing, ";
                                }
                                if (copperRemaining > 0)
                                {
                                    message += $" and {copperRemaining} copper on quick healing, ";
                                }
                                if (healthMissing > 0)
                                {
                                    message += $"restoring {healthMissing} health";
                                }

                                if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount > 3)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuffs";
                                }
                                else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount == 2)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuff";
                                }
                                else if (debuffCount > 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuffs";
                                }
                                else if (debuffCount == 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuff";
                                }
                                message += ".";
                                Main.NewText(message);
                            }

                            if (silverRemaining > 0 && platRemaining == 0 && goldRemaining == 0)
                            {
                                string message = $"You just spent {silverRemaining} silver";
                                if (copperRemaining == 0)
                                {
                                    message += " on quick healing, ";
                                }
                                if (copperRemaining > 0)
                                {
                                    message += $" and {copperRemaining} copper on quick healing, ";
                                }
                                if (healthMissing > 0)
                                {
                                    message += $"restoring {healthMissing} health";
                                }

                                if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount > 3)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuffs";
                                }
                                else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount == 2)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuff";
                                }
                                else if (debuffCount > 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuffs";
                                }
                                else if (debuffCount == 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuff";
                                }
                                message += ".";
                                Main.NewText(message);
                            }
                            if (cost > 0 && silverRemaining == 0 && platRemaining == 0 && goldRemaining == 0)
                            {
                                string message = $"You just spent {remainingMoney} copper on quick healing, ";
                                if (healthMissing > 0)
                                {
                                    message += $"restoring {healthMissing} health";
                                }

                                if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount > 3)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuffs";
                                }
                                else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && debuffCount == 2)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {calamityBossDebuffCount} debuff";
                                }
                                else if (debuffCount > 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuffs";
                                }
                                else if (debuffCount == 1)
                                {
                                    if (healthMissing > 0)
                                    {
                                        message += " and ";
                                    }
                                    message += $"curing {debuffCount} debuff";
                                }
                                message += ".";
                                Main.NewText(message);
                            }
                        }


                        if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && bossCombatCheck(6400f) && (Wallet >= cost | Wallet == -2147483648) && debuffCount > 1 | healthMissing > 0) // Pesudo weak reference to Calamity's boss debuff again,
                                                                                                                                                                     // outlining general healing conditions besides that
                        {
                            HealAndSpend(cost, Player, healthMissing);
                        }

                        else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && !bossCombatCheck(6400f) && (Wallet >= cost | Wallet == -2147483648) && debuffCount > 0 | healthMissing > 0) // Non-boss combat check healing conditions
                        {
                            HealAndSpend(cost, Player, healthMissing);
                        }

                        else if ((Wallet >= cost | Wallet == -2147483648) && debuffCount > 0 | healthMissing > 0) // Vanilla healing conditions
                        {
                            HealAndSpend(cost, Player, healthMissing);
                        }

                        else if (Wallet < cost && Wallet !=-2147483648) // Message displayed if money found in all inventory slots checked is less than the cost
                        {
                            Main.NewText("You don't have enough money to pay for a quick heal.");
                        }

                        else if ((Wallet >= cost | Wallet == -2147483648) && healthMissing == 0 && debuffCount == 0) // If your health is full
                        {
                            Main.NewText("Health full.");
                        }

                        else if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod") && (Wallet >= cost | Wallet == -2147483648) && bossCombatCheck(6400f) == true && debuffCount == 1 && healthMissing == 0) // If your health is full and you're fighting a boss in Calamity
                        {
                            Main.NewText("Health full.");
                        }
                        else // Something goes terribly, terribly, terribly, and I mean terribly wrong. This should not be possible
                        {
                            Main.NewText("Couldn't quick heal.");
                        }
                    }
                }
            }
        }
    }
}
