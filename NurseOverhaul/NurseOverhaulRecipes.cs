using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace NurseOverhaul
{
    public class NurseOverhaulRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            /* Not balanced with shop prices, could just buy 5 and convert for way cheaper than buying one from the shop
            // Recipe to turn Lesser Healing Potions into a Healing Potion
            Recipe healingPotionRecipe = Recipe.Create(ItemID.HealingPotion);
            healingPotionRecipe.AddIngredient(ItemID.LesserHealingPotion, 5);
            healingPotionRecipe.AddTile(TileID.AlchemyTable);
            healingPotionRecipe.Register();
            */

            // Recipe to turn Healing Potions into a Greater Healing Potion
            Recipe greaterHealingPotionRecipe = Recipe.Create(ItemID.GreaterHealingPotion);
            greaterHealingPotionRecipe.AddIngredient(ItemID.HealingPotion, 2);
            greaterHealingPotionRecipe.AddIngredient(ItemID.PixieDust, 1);
            greaterHealingPotionRecipe.AddTile(TileID.AlchemyTable);
            greaterHealingPotionRecipe.Register();

            // Recipe to turn Greater Healing Potions into a Super Healing Potion
            Recipe superHealingPotionRecipe = Recipe.Create(ItemID.SuperHealingPotion);
            superHealingPotionRecipe.AddIngredient(ItemID.GreaterHealingPotion, 4);
            superHealingPotionRecipe.AddTile(TileID.AlchemyTable);
            superHealingPotionRecipe.Register();

            //Super Healing Potion to Supreme Healing Potion
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);
            if (Calamity != null && Calamity.TryFind<ModItem>("SupremeHealingPotion", out ModItem supremeHealingPotion))
            {
                Recipe supremeHealingPotionRecipe = Recipe.Create(supremeHealingPotion.Type);
                supremeHealingPotionRecipe.AddIngredient(ItemID.SuperHealingPotion, 4);
                supremeHealingPotionRecipe.AddTile(TileID.AlchemyTable);
                supremeHealingPotionRecipe.Register();
            }

            /* don't really want to touch this for now. haven't played in a while and not sure how balanced something like this would be
             * 
            if (Calamity != null && Calamity.TryFind<ModItem>("OmegaHealingPotion", out ModItem omegaHealingPotion))
            {
                Recipe omegaHealingPotionRecipe = Recipe.Create(omegaHealingPotion.Type);
                omegaHealingPotionRecipe.AddIngredient(ItemID.SuperHealingPotion, 5);
                omegaHealingPotionRecipe.AddTile(TileID.AlchemyTable);
                omegaHealingPotionRecipe.Register();
            }
            */
        }
    }
}