//TO DO: 
// IMMPORTANT CALAMITY CHECK. SEE IF DIFFICULTY FACTOR * 100 WORKS FOR DEBUFF CALC IN PLAYER. VERY IMPORTANT
// Icon for tmod and item sprites 
// readme + tmodloader info file
// Dialogue for how much you spent at the nurse (button in box? maybe add total spent next to money just spent. could be too cluttery though)
// WISH Specific equipment item slot that will only take NurseHotkey range modification items 
// WISH: Maybe resprite or just an animation when you press the hotkey? Would be funny for like an Ana type character that shoots a healing dart at you to heal you past a certain range. Could add when glimmer introduced to tmod
// Potentially add lifeforce/jungle heart if people want

using Terraria.ModLoader;

namespace NurseHotkey
{
    public class NurseHotkey : Mod
    {
        // vessel for keybind
        internal static ModKeybind NurseHealHotkey;

        public override void Load()
        {
            // set keybind
            NurseHealHotkey = KeybindLoader.RegisterKeybind(this, "Quick Heal button", "G");
        }

        public override void Unload()
        {
            // unload mod
            NurseHealHotkey = null;
        }
    }
}


