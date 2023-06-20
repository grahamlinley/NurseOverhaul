// Nurse VIP Badge, Local Transponder, Surface Transponder, Global Transponder 
// other ideas: walkie talkie, Nurse's Platinum Insurance, Eye of Horus /the nurse, Holy Word: Salvation/nurse, Goro Goro no Mi, Chiyu Chiyu no Mi
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseHotkey.Items
{
    public class NurseVIPBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nurse VIP Badge");
            Tooltip.SetDefault("The Nurse will be watching for your signal. " +
                "\nWhen you signal (default keybind G), she will drop everything and heal you." +
                "\nIf using quick heal, the Nurse will charge you as if she has max happiness." +
                "\nAll she asks for is a measly additional 2 silver per quick heal.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 1, 0); // Set the item's value
            Item.rare = ItemRarityID.Blue; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Add any functionality you want the accessory to have when equipped here
            player.moveSpeed += 0.1f; // Example: Increase player's movement speed
        }

        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.Register();
        }
    }


    public class LocalTransponder : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Local Transponder");
            Tooltip.SetDefault("The Nurse can use this to target you with a heal at range, about one " +
                "\n\"screen's worth\" she tells you. Whatever that means." +
                "\nIf using quick heal, the Nurse will you charge as if she has max happiness." +
                "\nQuick heal now costs an additional 5 silver.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Green; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Add any functionality you want the accessory to have when equipped here
            //player.moveSpeed += 0.1f; // Example: Increase player's movement speed
        }

        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.Register();
        }
    }

    public class SurfaceTransponder : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surface Transponder");
            Tooltip.SetDefault("With your \"investments\" into the Nurse's new technology, she has found a way to increase the transponder's range even further." +
                "\nIf using quick heal, the Nurse will you charge as if she has max happiness." +
                "\nIncreases Nurse's quick heal range to anywhere on the surface, plus a decent amount underground (despite the name)." +
                "\nBase price increased by 15 silver.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 20, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Orange; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Add any functionality you want the accessory to have when equipped here
            //player.moveSpeed += 0.1f; // Example: Increase player's movement speed
        }

        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.Register();
        }
    }

    public class GlobalTransponder : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Global Transponder");
            Tooltip.SetDefault("The Nurse has created an industry, and that industry is booming. Her customers demand range and that's what she plans to give." +
                "\nIncreases's Nurse's quick range to anywhere in the world." +
                "\nIf using quick heal, the Nurse will you charge as if she has max happiness." +
                "\nBase price increased by 25 silver.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 20, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Orange; // Set the item's rarity
            Item.accessory = false; // Make the item an accessory
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Add any functionality you want the accessory to have when equipped here
            //player.moveSpeed += 0.1f; // Example: Increase player's movement speed
        }

        public override void AddRecipes()
        {
            // Add any crafting recipe for the item here if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.Register();
        }
    }
}
