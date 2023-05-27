using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;


namespace NurseHotkey;

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
            bool calamitasDowned = BossChecklistIntegration.BossDowned("CalamityMod:Calamitas");
            bool plagueBringerDowned = BossChecklistIntegration.BossDowned("CalamityMod:PlaguebringerGoliath");
            bool ravagerDowned = BossChecklistIntegration.BossDowned("CalamityMod:Ravager");
            bool providenceDowned = BossChecklistIntegration.BossDowned("CalamityMod:Providence");
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

                if (NPC.downedPlantBoss | calamitasDowned == true) // Plantera defeated
                {
                    multipliedCost += 2000; // 57 silver base
                }

                if (NPC.downedGolemBoss) // Golem defeated
                {
                    multipliedCost += 3000; // 87 silver base
                }

                if (NPC.downedFishron || plagueBringerDowned == true || ravagerDowned == true) // Duke Fishron/Plaguebringer Goliath/ Ravager defeated
                {
                    multipliedCost += 3000; // 1 gold 17 silver base
                }

                if (NPC.downedMoonlord) // Moon Lord defeated
                {
                    multipliedCost += 8000; // 1 gold 97 silver base
                }

                if (providenceDowned == true) //Providence defeated
                {
                    multipliedCost += 12000; //3 gold 20 silver base
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
            if (NPC.downedBoss3) // Skeletron
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
            if (NPC.downedPlantBoss) //Plantera or Calamitas
            {
                multiplier = 142.4999f;
            }
            if (NPC.downedGolemBoss) // Golem
            {
                multiplier = 189.9999f;
            }
            if (NPC.downedMoonlord) // Moon Lord
            {
                multiplier = 204.9999f; //203.9999f at 8000
            }
            if (NPC.downedAncientCultist)
            {
                multiplier = 3000;
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
            int totalMoney = 0;
            float debuffCount = GetDebuffCount(player); // Get the count of debuffs for the player

            int GetPlayerTotalMoney(int playerIndex)
            {
                Player player = Main.player[playerIndex];


                // Access the player's inventory
                Item[] inventory = player.inventory;

                // Calculate the total money from inventory
                int totalMoney = CalculateMoneyFromItems(inventory);

                // Access the player's Piggy Bank items
                Item[] piggyBankItems = player.bank.item;

                // Calculate the total money from Piggy Bank
                totalMoney += CalculateMoneyFromItems(piggyBankItems);

                return totalMoney;
            }

            // Helper method to calculate money from items
            int CalculateMoneyFromItems(Item[] items)
            {
                for (int i = 0; i < Player.inventory.Length; i++)
                {
                    Item item = Player.inventory[i];

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

                return totalMoney;
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







/*
public class ModGlobalNPC : GlobalNPC
{
    public override void SetDefaults(NPC npc)
    {

    }
    public override void AI(NPC npc)
    {
        base.AI(npc);
        if (npc.type == NPCID.Nurse)
        {
            Player modPlayer = Main.LocalPlayer; //First getting who is the player
            if (Vector2.Distance(npc.Center, modPlayer.Center) < 300) //Checking for the distance between the NPC and the player and if true
            {
                //Main.LocalPlayer.BuyItem();
                Main.LocalPlayer.Heal(9999);
            }
        }
    }
}

*/

/*
    public int CalculateHealCost(int healthMissing, Player player)
    {
        float baseCost = 1f; // Calculate the base cost of the heal based on the player's maximum health
        float difficultyFactor = 1f; // Set the difficulty factor to 1 by default

        if (Main.expertMode)
        {
            difficultyFactor = 2f; // If the game is in Expert mode, set the difficulty factor to 2
        }
        else if (Main.masterMode)
        {
            difficultyFactor = 3f; // If the game is in Master mode, set the difficulty factor to 3
        }

        int totalCost = (int)(baseCost * difficultyFactor); // Calculate the total cost of the heal based on the difficulty factor

        // Apply any additional modifiers or events that may affect the cost of the heal
        totalCost += GetDebuffCost() * 100; // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
        /* totalCost *= (int)nurse.happiness * 10; // Multiply the total cost by the nurse's current happiness adjustment 
        totalCost = (int)(totalCost * GetBossMultiplier()); // Multiply the total cost by the boss defeat multiplier

        return totalCost; // Return the total cost of the heal
    }

*/



// need to figure out happiness call
/*
    private float GetHappinessModifier()
    {
        NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

        if (nurse != null)
        {
            float happiness = nurse.GetGlobalNPC<Nurse>().happiness;

            if (happiness > 0)
            {
                return 1f + happiness / 100f;
            }
            else
            {
                return 1f / (1f - happiness / 100f);
            }
        }

        return 1f;
    }
*/




//Main.NewText($"You spent {cost} coins for healing. Total spent: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} coins.")


/*        private void NurseHeal()
    {
        NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];

        if (nurse != null && Vector2.Distance(Player.Center, nurse.Center) <= 300)
        {
            int healthMissing = Player.statLifeMax2 - Player.statLife;
int cost = GetHealCost(healthMissing, Player);
int totalMoney = 0;
int silverCost = cost / 100;
int goldCost = cost / 10000;
int platCost = cost / 1000000;

            for (int i = 0; i<Player.inventory.Length; i++)
            {
                Item item = Player.inventory[i];

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
            //int money = Player.inventory[58].stack + Player.inventory[59].stack + Player.inventory[60].stack + Player.inventory[61].stack;
            Player player = Main.LocalPlayer;

if (totalMoney >= cost)
{
    Player.BuyItem(cost);
    int healAmount = healthMissing;
    Player.statLife += healAmount;
    Player.HealEffect(healAmount);

    if (cost > 100 && cost < 9999)
    {
        Main.NewText($"You spent {silverCost} silver coins on healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
    }

    if (cost > 10000 && cost < 1000000)
    {
        Main.NewText($"You spent {goldCost} gold coins for healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
    }
    if (cost > 1000000)
    {
        Main.NewText($"You spent {platCost} platinum coins for healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
    }
    // track money spent and print to chat
    Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent += cost;
}
else if (totalMoney < cost)
{
    Main.NewText("You don't have enough money to pay for the heal.");
}
        }

    }

*/

/*
                 if (platCost > 0)
                {
                    Main.NewText($"You just spent {platRemaining} platinum {goldRemaining} gold {silverRemaining} silver and {copperRemaining} copper on quick healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
                }
                if (goldCost > 0 && platCost == 0)
                {
                    Main.NewText($"You just spent {goldRemaining} gold {silverRemaining} silver and {copperRemaining} copper for healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
                }
                if (silverCost> 0 && platCost == 0 && goldCost == 0)
                {
                    Main.NewText($"You just spent {silverRemaining} silver and {copperRemaining} copper on quick healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
                }
                if (cost > 0 && silverCost == 0 && platCost == 0 && goldCost == 0)
                {
                    Main.NewText($"You just spent {cost} copper on quick healing. Total spent this session through NurseHotkey: {Main.LocalPlayer.GetModPlayer<poopy>().MoneySpent} copper coins.");
                }




            if (totalMoney > cost && healthMissing > 0)
            {
                if (ModLoader.GetMod("CalamityMod") != null)
                {
                    if (NPC.downedBoss1) // Eye of Cthulhu defeated
                    {
                        totalCost += 300; // Increased price
                    }

                    if (NPC.downedBoss3) // Skeletron defeated
                    {
                        totalCost += 900; // Increased price
                    }

                    if (Main.hardMode)
                    {
                        if (NPC.downedMechBossAny) // At least one Mechanical Boss defeated
                        {
                            totalCost += 1200; // Increased price
                        }
                    }

                    if (NPC.downedPlantBoss) // Plantera defeated
                    {
                        totalCost += 2400; // Increased price
                    }

                    if (NPC.downedGolemBoss) // Golem defeated
                    {
                        totalCost += 4000; // Increased price
                    }

                    if (NPC.downedFishron) // Duke Fishron defeated
                    {
                        totalCost += 6000; // Increased price
                    }

                    if (NPC.downedMoonlord) // Moon Lord defeated
                    {
                        totalCost += 9000; // Increased price
                    }
                }
                else 
                {
                    totalCost += 1;
                }
            }


                int goldNumber;
                int silverNumber;
                int copperNumber;

                if (goldRemaining == 0 && platCost >0)
                {
                    goldNumber = 
                }


    public static float GetHealCost(int healthMissing, Player player)
    {
        float difficultyFactor = 1; // Set the difficulty factor to 1 by default
        float baseCost = healthMissing;

        if (Main.expertMode)
        {
            difficultyFactor = 2; // If the game is in Expert mode, set the difficulty factor to 2
        }
        if (Main.masterMode)
        {
            difficultyFactor = 2; // If the game is in Master mode, set the difficulty factor to 3
        }
        float totalCost = (baseCost * difficultyFactor); // Calculate the total cost of the heal based on the difficulty factor
        totalCost += debuffco * 100; // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
        totalCost += (totalCost * GetBossMultiplier()); // Multiply the total cost by the boss defeat multiplier

        if (totalCost < 1)
        {
            totalCost = 0;
        }
        return totalCost;
    }


    public int GetDebuffCost(int debuffCost, Player player)
    {
        debuffCost = 0;
        for (int i = 0; i < Player.buffType.Length; i++)
        {
            if (Player.buffType[i] > 0 && Main.debuff[Player.buffType[i]])
            {
                debuffCost++;
            }
        }
        return debuffCost;
    }




                    // Check if the player has Betsy's Curse
                    if (player.HasBuff(BuffID.BetsysCurse))
                    {
                        // Remove Betsy's Curse
                        player.DelBuff(player.FindBuffIndex(BuffID.BetsysCurse));
                    }

                    // Check if the player is bleeding
                    if (player.HasBuff(BuffID.Bleeding))
                    {
                        // Remove the bleeding debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Bleeding));
                    }

                    // Check if the player has broken armor
                    if (player.HasBuff(BuffID.BrokenArmor))
                    {
                        // Remove the broken armor debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.BrokenArmor));
                    }

                    // Check if the player is on fire
                    if (player.HasBuff(BuffID.Burning))
                    {
                        // Remove the burning debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Burning));
                    }

                    // Check if the player is chilled
                    if (player.HasBuff(BuffID.Chilled))
                    {
                        // Remove the chilled debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Chilled));
                    }

                    // Check if the player is confused
                    if (player.HasBuff(BuffID.Confused))
                    {
                        // Remove the confused debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Confused));
                    }

                    // Check if the player is cursed
                    if (player.HasBuff(BuffID.Cursed))
                    {
                        // Remove the cursed debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Cursed));
                    }

                    // Check if the player is in darkness
                    if (player.HasBuff(BuffID.Darkness))
                    {
                        // Remove the darkness debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Darkness));
                    }

                    // Check if the player is electrified
                    if (player.HasBuff(BuffID.Electrified))
                    {
                        // Remove the electrified debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Electrified));
                    }

                    // Check if the player is frostburned
                    if (player.HasBuff(BuffID.Frostburn))
                    {
                        // Remove the frostburn debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Frostburn));
                    }

                    // Check if the player has the gravitation debuff
                    if (player.HasBuff(BuffID.Gravitation))
                    {
                        // Remove the gravitation debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Gravitation));
                    }

                    // Check if the player has the ichor debuff
                    if (player.HasBuff(BuffID.Ichor))
                    {
                        // Remove the ichor debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Ichor));
                    }

                    // Check if the player is lovestruck
                    if (player.HasBuff(BuffID.Lovestruck))
                    {
                        // Remove the lovestruck debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Lovestruck));
                    }

                    // Check if the player has the moon bite debuff
                    if (player.HasBuff(BuffID.MoonLeech))
                    {
                        // Remove the moon bite debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.MoonLeech));
                    }

                    // Check if the player has the ogre spit debuff
                    if (player.HasBuff(BuffID.OgreSpit))
                    {
                        // Remove the ogre spit debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.OgreSpit));
                    }

                    // Check if the player is poisoned
                    if (player.HasBuff(BuffID.Poisoned))
                    {
                        // Remove the poisoned debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Poisoned));
                    }
                    // Check if the player has the shadow dodge buff
                    if (player.HasBuff(BuffID.ShadowDodge))
                    {
                        // Remove the shadow dodge buff
                        player.DelBuff(player.FindBuffIndex(BuffID.ShadowDodge));
                    }

                    // Check if the player is on fire
                    if (player.HasBuff(BuffID.OnFire))
                    {
                        // Remove the on fire debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.OnFire));
                    }

                    // Check if the player has the thorns debuff
                    if (player.HasBuff(BuffID.Thorns))
                    {
                        // Remove the thorns debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Thorns));
                    }

                    // Check if the player is venom
                    if (player.HasBuff(BuffID.Venom))
                    {
                        // Remove the venom debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Venom));
                    }

                    // Check if the player has the water candle debuff
                    if (player.HasBuff(BuffID.WaterCandle))
                    {
                        // Remove the water candle debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.WaterCandle));
                    }

                    // Check if the player is wet
                    if (player.HasBuff(BuffID.Wet))
                    {
                        // Remove the wet debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Wet));
                    }

                    // Check if the player has the winded debuff
                    if (player.HasBuff(BuffID.WindPushed))
                    {
                        // Remove the winded debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.WindPushed));
                    }

                    // Check if the player is webbed
                    if (player.HasBuff(BuffID.Webbed))
                    {
                        // Remove the webbed debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Webbed));
                    }

                    // Check if the player is stoned
                    if (player.HasBuff(BuffID.Stoned))
                    {
                        // Remove the stoned debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Stoned));
                    }

                    // Check if the player has the obnoxious debuff
                    if (player.HasBuff(BuffID.Obstructed))
                    {
                        // Remove the obnoxious debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Obstructed));
                    }

                    // Check if the player has the dazed debuff
                    if (player.HasBuff(BuffID.Dazed))
                    {
                        // Remove the dazed debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Dazed));
                    }

                    // Check if the player has the frozen debuff
                    if (player.HasBuff(BuffID.Frozen))
                    {
                        // Remove the frozen debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Frozen));
                    }

                    // Check if the player has the cursed inferno debuff
                    if (player.HasBuff(BuffID.CursedInferno))
                    {
                        // Remove the cursed inferno debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.CursedInferno));
                    }

                    // Check if the player has the blacked out debuff
                    if (player.HasBuff(BuffID.Blackout))
                    {
                        // Remove the blacked out debuff
                        player.DelBuff(player.FindBuffIndex(BuffID.Blackout));
                    }



            var currentShoppingSettings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, nurse);
            int currentPriceAdjustment = (int)currentShoppingSettings.PriceAdjustment;


        public void Happiness()
    {
        // Initialize the readonly dictionary with some NPC happiness levels
        var dictionary = new Dictionary<int, AffectionLevel>()
        {
            { NPCID.Nurse, AffectionLevel.Love },
            { NPCID.Nurse, AffectionLevel.Hate },
            { NPCID.Nurse, AffectionLevel.Like },
            { NPCID.Nurse, AffectionLevel.Dislike }
            // Add more NPCs and their respective happiness levels as needed
        };
    }



    private readonly ReadOnlyDictionary<int, AffectionLevel> npcHappinessLevels;
    private int healCost = 1; // Set the default heal cost to 1c per point of health




public class poopy : ModPlayer
{
    public int MoneySpent { get; set; }
}

using Terraria.ModLoader.Config;
using Terraria.UI;
using Color = System.Drawing.Color;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria.Localization;
using NurseHotkey;


    public static void CureAllDebuffs(Player player)
    {
        for (int i = 0; i < player.buffType.Length; i++)
        {
            int buffType = player.buffType[i];

            if (buffType > 0 && Main.debuff[buffType] && !ExcludedDebuff(buffType))
            {
                player.ClearBuff(buffType);
            }
        }
    }
*/

/*
for (int i = 0; i < Player.inventory.Length; i++)
    {
        Item item = Player.inventory[i];

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

        float totalCost = (baseCost * difficultyFactor); // Calculate the total cost of the heal based on the difficulty factor

        if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
        {
            totalCost += debuffCount * 200;
        }
        else
        {
            totalCost += debuffCount * 100; // Add the debuff cost to the total cost, multiplied by 1 silver per debuff
        }

        if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
        {
            totalCost += totalCost * GetCalamityBossMultiplier();
        }
        else
        {
            totalCost += totalCost * GetBossMultiplier(); // Multiply the total cost by the boss defeat multiplier
        }

        return totalCost;
    }


private void NurseHeal()
    {
        NPC nurse = Main.npc[NPC.FindFirstNPC(NPCID.Nurse)];
        Player player = Main.LocalPlayer;

        if (nurse != null && Vector2.Distance(Player.Center, nurse.Center) <= 300)
        {
            int healthMissing = Player.statLifeMax2 - Player.statLife;
            float cost = GetHealCost(healthMissing, Player);
            int totalMoney = 0;
            float debuffCount = GetDebuffCount(player); // Get the count of debuffs for the player

            int GetPlayerTotalMoney(int playerIndex)
            {
                Player player = Main.player[playerIndex];


                // Access the player's inventory
                Item[] inventory = player.inventory;

                // Calculate the total money from inventory
                int totalMoney = CalculateMoneyFromItems(inventory);

                // Access the player's Piggy Bank items
                Item[] piggyBankItems = player.bank.item;

                // Calculate the total money from Piggy Bank
                totalMoney += CalculateMoneyFromItems(piggyBankItems);

                return totalMoney;
            }

            // Helper method to calculate money from items
            int CalculateMoneyFromItems(Item[] items)
            {
                for (int i = 0; i < Player.inventory.Length; i++)
                {
                    Item item = Player.inventory[i];

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

                return totalMoney;
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
*/