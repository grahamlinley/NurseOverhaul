// All of this taken from AnglerShop with permission (ty mr notle0n), will commment 1.4.4 changes and Nurse specific changes
using Microsoft.Xna.Framework;
using NurseOverhual;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace NurseOverhaul
{
    internal class UISystem : ModSystem
    {
        internal UserInterface UserInterface;
        private static bool dialogueTweakLoaded = false;
        private UIState UI;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                UI = new NurseOverhaulUIState(); //What's going to be drawn
                UI.Activate();
                UserInterface = new UserInterface();
                UserInterface.SetState(UI);
            }
        }

        public override void Unload()
        {
            UI = null;
            UserInterface = null;
        }

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak))
            {
                dialogueTweakLoaded = true;
                dialogueTweak.Call("AddButton",
                    NPCID.Nurse, // Changed, but only useful for DialogueTweak
                    () => Language.GetTextValue("LegacyInterface.28"),
                    "DialogueTweak/Interfaces/Assets/Icon_Default", // The texture's path
                    () =>
                    {
                        NurseOverhaulUIState.OpenShop(1); //Shop button in DialogueTweak will call our OpenShop method 
                    });
            }
        }

        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;

            // if the player is talking to the Nurse and the new shop isn't opened
            // changes index from original angler shop
            if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Nurse && Main.npcShop != 1)
            {
                UserInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "NurseHotkey: UI",
                    delegate
                    {
                        // if the player is talking to the Nurse, the new shop isn't opened and dialogue tweak mod isn't active.
                        // changes index from original angler shop
                        if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Nurse && Main.npcShop != 1 && !dialogueTweakLoaded)
                        {
                            UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    }, InterfaceScaleType.UI));
            }
        }
    }
}
