// Nurse VIP Badge, Local Transponder, Surface Transponder, Global Transponder 
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
                "When you signal (default keybind G), she will drop everything and heal you (for an additional 2 silver, of course).");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0); // Set the item's value
            Item.rare = ItemRarityID.Green; // Set the item's rarity
            Item.accessory = true; // Make the item an accessory
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
}
