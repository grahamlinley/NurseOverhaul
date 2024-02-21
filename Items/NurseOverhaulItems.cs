// Nurse VIP Badge, Local Transponder, Surface Transponder, Global Transponder 
// other ideas: walkie talkie, Nurse's Platinum Insurance, Eye of Horus /the nurse, Holy Word: Salvation/nurse, Goro Goro no Mi, Chiyu Chiyu no Mi
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseOverhaul.Items
{
    public class NurseVIPBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nurse VIP Badge");

            string tooltipText = "The Nurse will be watching for your signal. " +
                "\nWhen you're close to her, and you give her the signal (default key G when bound)," +
                "\nshe will drop everything she's doing to heal you." +
                "\nThe Nurse will only be able to track you as long as you keep this item in your inventory." +
                "\nIf you lose it, she'll sell you another at her \"discounted rate.\"";

            // Check if Calamity Mod is enabled.
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);
            if (Calamity != null)
            {
                tooltipText += "\nCALAMITY PLAYERS: Be aware Calamity bosses increases the price of the Nurse by 5 times what it normally costs to heal if you are within 400 tiles of them.";
            }

            // Tooltip.SetDefault(tooltipText);
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NurseVIPBadgeEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 15, 0); // Set the item's value
            Item.rare = ItemRarityID.Green; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }
    }

    public class BrokenWalkieTalkie : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Single Band Transceiver");
            // Tooltip.SetDefault("You get the feeling this could somehow be combined with your Nurse badge...");
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesWalkieTalkieEnabled) 
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 5, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Orange; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }
    }


    public class NurseWalkieTalkie : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nurse's Walkie Talkie");
            string tooltipText = "Using the technology you discovered, the Nurse has developed a way to remain in constant contact with you, and with a press of a button (default G) she can track you and heal you. " +
                "\nShe tells you she can hit you as long as you stay within a little over one" +
                "\n\"screen's worth\" of her. Whatever that means." +
                "\nThe Nurse will only be able to track you as long as you keep this item in your inventory.";

            // Check if Calamity Mod is enabled.
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);
            if (Calamity != null)
            {
                tooltipText += "\nCALAMITY PLAYERS: Be aware Calamity bosses increases the price of the Nurse by 5 times what it normally costs to heal if you are within 400 tiles of them.";
            }
            // Tooltip.SetDefault(tooltipText);
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesWalkieTalkieEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 7, 50, 0); // Set the item's value
            Item.rare = ItemRarityID.LightRed; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }

        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldCoin, 1);
            recipe.AddIngredient(ModContent.ItemType<NurseVIPBadge>(), 1);
            recipe.AddIngredient(ModContent.ItemType<BrokenWalkieTalkie>(), 1);
            recipe.Register();
        }
    }

    public class BioticRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Biotic Rifle");
            // Tooltip.SetDefault("A faint echo of Arabic rings in your ear. Despite it's gun-like shape, you can't wield it. Maybe the Nurse could use it for something...");
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesPaintedShirtEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 7, 50, 0); // Set the item's value
            Item.rare = ItemRarityID.Lime; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }
    }

    public class SurfaceTransponder : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Surface Transponder");
            string tooltipText = "With your \"investments\" into the Nurse's new technology, she has found a way to increase her quick heal's range even further, taking her heals to the sky." +
                "\nIncreases Nurse's quick heal range to anywhere on the surface, plus a decent amount underground." +
                "\nThe Nurse will only be able to track you as long as you keep this item in your inventory.";

            // Check if Calamity Mod is enabled.
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);
            if (Calamity != null)
            {
                tooltipText += "\nCALAMITY PLAYERS: Be aware Calamity bosses increases the price of the Nurse by 5 times what it normally costs to heal if you are within 400 tiles of them.";
            }
            // Tooltip.SetDefault(tooltipText);
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesPaintedShirtEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 20, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Yellow; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }
        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldCoin, 10);
            recipe.AddIngredient(ModContent.ItemType<NurseWalkieTalkie>(), 1);
            recipe.AddIngredient(ModContent.ItemType<BioticRifle>(), 1);
            recipe.Register();
        }
    }

    public class Thruster : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Thruster Components");
            // Tooltip.SetDefault("Why do thrusters make you think of healing? Either the Nurse could use this or you might need some serious medical treatment...");
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NurseNourishmentDiamondEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 10, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Cyan; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }
    }

    public class PlatinumInsurance : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Platinum Insurance");
            string tooltipText = "She has the technology." +
                "\nThe Nurse has created an industry, and that industry is booming. Her customers demand range and that's what she plans to deliver with her new satellite network." +
                "\nAs one of her original clients, she has given you acess to her highest tier of insurance." +
                "\nIncreases's Nurse's quick range to anywhere in the world." +
                "\nThe Nurse will only be able to track you as long as you keep this item in your inventory.";

            // Check if Calamity Mod is enabled.
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);
            if (Calamity != null)
            {
                tooltipText += "\nCALAMITY PLAYERS: Be aware Calamity bosses increases the price of the Nurse by 5 times what it normally costs to heal if you are within 400 tiles of them.";
            }
            // Tooltip.SetDefault(tooltipText);
        }

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NurseNourishmentDiamondEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(1, 50, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Red; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }

        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumCoin, 1);
            recipe.AddIngredient(ModContent.ItemType<SurfaceTransponder>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Thruster>(), 1);
            recipe.Register();
        }
    }
}
