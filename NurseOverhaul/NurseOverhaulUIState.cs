using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NurseOverhaul;
using NurseOverhaul.Items;
using System;
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

namespace NurseOverhual
{
    internal class NurseOverhaulUIState : UIState
        {
        // The next 75 lines are responsible for drawing the shop button. We're going to do some special things for the Nurse shop button though.
        private static object TextDisplayCache => typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.instance);
        private bool focused;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 scale = new Vector2(0.9f);
            string text = Language.GetTextValue("LegacyInterface.28"); // "Shop" = LegacyInterface.28. This is the actual "shop button"
            int numLines = (int)TextDisplayCache.GetType().GetProperty("AmountOfLines", BindingFlags.Instance | BindingFlags.Public).GetValue(TextDisplayCache);
            Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

            Vector2 value2 = Vector2.One;
            if (stringSize.X > 260f)
                value2.X *= 260f / stringSize.X;

            Player player = Main.LocalPlayer;
            float debuffCount = NurseOverhaulPlayer.GetDebuffCount(player); // We're bringing these in so when the heal button is active, it doesn't mess with the our drawn shop button. The heal text's length is dynamic, and the shop button is fixed in place, so the text will be jumbled if we don't do this

            if (!Main.LocalPlayer.ghost && Main.LocalPlayer.statLife == Main.LocalPlayer.statLifeMax2 && debuffCount == 0) //the player must not be able to be healed by the nurse for us to draw the shop button
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

                ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, text, position + stringSize * value2 * 0.5f, (!focused) ? Color.Black : Color.Brown, 0f, stringSize * 0.5f, scale * value2); // condensed version of AnglerShop
                ChatManager.DrawColorCodedString(spriteBatch, FontAssets.MouseText.Value, text, position + stringSize * value2 * 0.5f, !focused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69), 0f, stringSize * 0.5f, scale);
            }
        }

        //from here on, the logic is similar to AnglerShop but things are changed drastically for 1.4.4 compatability such as how we're calling the shop, where it's stored etc.
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (focused && Main.mouseLeft)
            {
                OpenShop();
            }
        }

        internal static void OpenShop()
        {
            Main.playerInventory = true;
            Main.stackSplit = 9999;
            Main.npcChatText = "";
            Main.SetNPCShopIndex(1);
            //string shopName = NPCShopDatabase.GetShopName(npc.type, "NurseShop");
            //Main.NewText($"Opening shop: {shopName}"); // Print the shop name
            //Chest chest = Main.instance.shop[1];
            //chest.SetupShop("NurseShop", nurseNPC);
            Main.instance.shop[Main.npcShop].SetupShop("Terraria/Nurse/Shop", Main.LocalPlayer.TalkNPC);
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}

