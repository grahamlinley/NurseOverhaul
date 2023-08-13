using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseOverhaul.Items
{
    public class NurseVIPBadge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nurse's VIP Badge");
            Tooltip.SetDefault("When you're close to the Nurse and you " +
                "\ngive her the signal (default key G when bound)," +
                "\nshe will drop everything and heal you." +
                "\nShe tells you to keep it on your person at all " +
                "\ntimes or you'll need to talk to her like everyone else.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = false;
        }
    }

    public class BrokenWalkieTalkie : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Walkie Talkie");
            Tooltip.SetDefault("On first glance, it looks like junk, but on the bottom there looks like there's a slot for a card and a coin");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = false;
        }
    }

    public class NurseWalkieTalkie : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nurse's Walkie Talkie");
            Tooltip.SetDefault("With this in your inventory, the Nurse can now remain " +
                "\nin constant contact with you and track your whereabouts" +
                "\nas long as you remain really close by... and for the right price." +
                "\nShe says when you give the signal (default key G) she'll hit you with her newly developed ranged heal." +
                "\nTo fund this, healing outside a certain range" +
                "\nincreases prices by 3 times her normal rates." +
                "\nIf you're close to her, rates are normal." +
                "\nBuffs will show when you're in her extended range" +
                "\nand when prices are reduced.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = false;
        }

        public override void AddRecipes()
        {
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
            DisplayName.SetDefault("Biotic Rifle");
            Tooltip.SetDefault("As you hold the weapon, you hear the faint echo of " +
                "\na woman yelling in a language you don't recognize. " +
                "\nDespite its gun-like shape, you somehow feel like " +
                "\nit might be more useful to someone else.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 7, 50, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = false;
        }
    }

    public class SurfaceTransponder : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nurse's Painted Shirt");
            Tooltip.SetDefault("Armed with her new rifle, the Nurse tells you" +
                "\nshe'll be able to track and deploy her quick heal" +
                "\neven further than before. Just give her the signal" +
                "\n(default keybind G), and she'll find you" +
                "\nIncreases Nurse's quick heal range to anywhere" +
                "\non the surface, plus a decent amount underground." +
                "\nThe Nurse will only be able to track you as long" +
                "\nas you keep this item in your inventory." +
                "\nDoubles the range of the Nurse's reduced pricing from" +
                "\nNurse's Walkie Talkie." +
                "\nPrice per quick heal outside the sweet spot increased" +
                "\nby 3 times the Nurse's normal rate.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = false;
        }

        public override void AddRecipes()
        {
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
            DisplayName.SetDefault("Thruster Components");
            Tooltip.SetDefault("Why do thrusters make you think of healing? " +
                "\nEither the Nurse could use this or you " +
                "\nmight need some serious medical treatment...");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.accessory = false;
        }
    }

    public class PlatinumInsurance : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nurse Nourishment Diamond");
            Tooltip.SetDefault("Her customers demand range and that's what she aims to deliver." +
                "\nCan quick heal from anywhere in the world." +
                "\nProof of insurance must be carried at all times for treatment." +
                "\nTriples the range of the Nurse's reduced pricing from " +
                "\nNurse's Walkie Talkie." +
                "\nPrice per quick heal outside the sweet spot increased " +
                "\nby 3 times the Nurse's normal rate.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(1, 50, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumCoin, 1);
            recipe.AddIngredient(ModContent.ItemType<SurfaceTransponder>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Thruster>(), 1);
            recipe.Register();
        }
    }
}
