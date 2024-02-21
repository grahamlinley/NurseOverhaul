// Nurse VIP Badge, Local Transponder, Surface Transponder, Global Transponder 
// other ideas: walkie talkie, Nurse's Platinum Insurance, Eye of Horus /the nurse, Holy Word: Salvation/nurse, Goro Goro no Mi, Chiyu Chiyu no Mi
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NurseOverhaul.Items
{
    public class NurseVIPBadge : ModItem
    {
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

        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesWalkieTalkieEnabled) 
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 5, 0, 0); 
            Item.rare = ItemRarityID.Orange; 
        }
    }


    public class NurseWalkieTalkie : ModItem
    {
        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesWalkieTalkieEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 7, 50, 0); 
            Item.rare = ItemRarityID.LightRed; 
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
        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesPaintedShirtEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 7, 50, 0); 
            Item.rare = ItemRarityID.Lime;
        }
    }

    public class SurfaceTransponder : ModItem
    {
        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NursesPaintedShirtEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 20, 0, 0); 
            Item.rare = ItemRarityID.Yellow; 
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
        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NurseNourishmentDiamondEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan; 

        }
    }

    public class PlatinumInsurance : ModItem
    {
        public override void SetDefaults()
        {
            if (!ModContent.GetInstance<NurseOverhaulConfig>().NurseNourishmentDiamondEnabled)
            {
                return;
            }
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(1, 50, 0, 0); 
            Item.rare = ItemRarityID.Red; 
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
