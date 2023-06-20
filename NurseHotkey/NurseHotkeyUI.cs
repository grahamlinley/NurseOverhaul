using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NurseHotkey.Items;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NurseHotkey
{
    internal class NurseHotkeyUI : UIState
    {
        private static object TextDisplayCache => typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.instance);
        private bool focused;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 scale = new Vector2(0.9f);
            string text = Language.GetTextValue("LegacyInterface.28"); // "Shop"
            int numLines = (int)TextDisplayCache.GetType().GetProperty("AmountOfLines", BindingFlags.Instance | BindingFlags.Public).GetValue(TextDisplayCache);
            Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

            Vector2 value2 = Vector2.One;
            if (stringSize.X > 260f)
                value2.X *= 260f / stringSize.X;

            Player player = Main.LocalPlayer;
            float debuffCount = NurseHotkeyPlayer.GetDebuffCount(player);

            if (!Main.LocalPlayer.ghost && Main.LocalPlayer.statLife == Main.LocalPlayer.statLifeMax2 && debuffCount == 0)
            {
                float posButton1 = 180 + (Main.screenWidth - 800) / 2;
                float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.54"), scale).X + 30f;
                float posButton3 = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;
                float posButton4 = posButton3 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("UI.NPCCheckHappiness"), scale).X + 30f;
                Vector2 position = new Vector2(posButton4, 130 + numLines * 30);

                if (Main.MouseScreen.Between(position, position + stringSize * scale * value2.X) && !PlayerInput.IgnoreMouseInterface)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Main.LocalPlayer.releaseUseItem = false;
                    scale *= 1.2f;

                    if (!focused)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }

                    focused = true;
                }
                else
                {
                    if (focused)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }

                    focused = false;
                }

                ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, text, position + stringSize * value2 * 0.5f, (!focused) ? Color.Black : Color.Brown, 0f, stringSize * 0.5f, scale * value2);
                ChatManager.DrawColorCodedString(spriteBatch, FontAssets.MouseText.Value, text, position + stringSize * value2 * 0.5f, !focused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69), 0f, stringSize * 0.5f, scale);
            }
        }

        public static bool baseShop = false;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (focused && Main.mouseLeft)
            {
                OpenShop(99);
            }
        }

        internal static void OpenShop(int shopIndex)
        {
            Main.playerInventory = true;
            Main.stackSplit = 9999;
            Main.npcChatText = "";
            Main.SetNPCShopIndex(shopIndex);
            SetupShop(Main.instance.shop[99]);
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
        
        private static void SetupShop(Chest shop)
        {
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);

            List<(int id, int price)> items = new List<(int id, int price)>
            {
                (ItemID.Mushroom, 250),
                (ItemID.BottledWater, 200),
                (ItemID.BottledHoney, 400),
                (ItemID.LesserHealingPotion, 300),
                (ItemID.RestorationPotion, 15000),
                (ItemID.HealingPotion, 10000),
                (ItemID.GreaterHealingPotion, 50000),
                (ItemID.LifeforcePotion, 10000),
                (ItemID.SuperHealingPotion, 150000),
                (ModContent.ItemType<NurseVIPBadge>(), 5000),
                (ModContent.ItemType<NurseWalkieTalkie>(), 250000),
                (ModContent.ItemType<SurfaceTransponder>(), 750000),
                (ModContent.ItemType<PlatinumInsurance>(), 2000000),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0),
                (ItemID.None, 0)
            };

            if (!NPC.downedSlimeKing)
            {
                items.RemoveAll(item => item.id == ItemID.HealingPotion ||
                                        item.id == ItemID.RestorationPotion ||
                                        item.id == ItemID.GreaterHealingPotion ||
                                        item.id == ItemID.LifeforcePotion ||
                                        item.id == ItemID.SuperHealingPotion ||
                                        item.id == ItemID.None ||
                                        item.id == ModContent.ItemType<NurseWalkieTalkie>() ||
                                        item.id == ModContent.ItemType<SurfaceTransponder>() ||
                                        item.id == ModContent.ItemType<PlatinumInsurance>());
            }

            if (!NPC.downedBoss1)
            {
                items.RemoveAll(item => item.id == ItemID.RestorationPotion ||
                                        item.id == ItemID.GreaterHealingPotion ||
                                        item.id == ItemID.LifeforcePotion ||
                                        item.id == ItemID.SuperHealingPotion ||
                                        item.id == ModContent.ItemType<SurfaceTransponder>() ||
                                        item.id == ModContent.ItemType<PlatinumInsurance>());
            }

            if (!NPC.downedBoss3)
            {
                items.RemoveAll(item => item.id == ItemID.RestorationPotion ||
                                        item.id == ItemID.GreaterHealingPotion ||
                                        item.id == ItemID.LifeforcePotion ||
                                        item.id == ItemID.SuperHealingPotion ||
                                        item.id == ModContent.ItemType<PlatinumInsurance>());
            }

            if (!Main.hardMode)
            {
                items.RemoveAll(item => item.id == ItemID.LifeforcePotion ||
                                        item.id == ItemID.SuperHealingPotion);
            }

            if (!NPC.downedAncientCultist)
            {
                items.RemoveAll(item => item.id == ItemID.SuperHealingPotion);
            }


            int supremeHealingPotionIndex = -1;
            int omegaHealingPotionIndex = -1;

            if (Calamity != null && NPC.downedMoonlord && Calamity.TryFind<ModItem>("SupremeHealingPotion", out ModItem supremeHealingPotion))
            {
                supremeHealingPotionIndex = items.FindIndex(item => item.id == ItemID.SuperHealingPotion);

                    items.Add((supremeHealingPotion.Type, 500000));
            

            }

            if (Calamity != null && BossChecklistIntegration.isDevourerDefeated() && Calamity.TryFind<ModItem>("OmegaHealingPotion", out ModItem omegaHealingPotion))
            {
                omegaHealingPotionIndex = items.FindIndex(item => item.id == ItemID.SuperHealingPotion);

                    items.Add((omegaHealingPotion.Type, 1000000));

            }

            for (int i = 0; i < items.Count; i++)
            {
                Item newItem = new Item();
                newItem.SetDefaults(items[i].id);
                newItem.shopCustomPrice = items[i].price;
                shop.item[i].SetDefaults(newItem.type);
                shop.item[i].shopCustomPrice = newItem.shopCustomPrice;
                shop.item[i].isAShopItem = true;
            }
        }

    }
}