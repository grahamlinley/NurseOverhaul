//TO DO: 
// Add weak references to Calamity when 1.4.4 preview released? Remove bosschecklistintegration?
// IMMPORTANT CALAMITY CHECK. SEE IF DIFFICULTY FACTOR * 100 WORKS FOR DEBUFF CALC IN PLAYER. VERY IMPORTANT
// WISH: Dialogue for how much you spent at the nurse (button in box? maybe add total spent next to money just spent. could be too cluttery though)
// WISH Specific equipment item slot that will only take NurseHotkey range modification items 
// WISH: Maybe resprite or just an animation when you press the hotkey? Would be funny for like an Ana type character that shoots a healing dart at you to heal you past a certain range. Could add when glimmer introduced to tmod
// Potentially add life crystal/jungle heart if people want

using Terraria.ModLoader;

namespace NurseOverhaul
{
    public class NurseOverhaul : Mod
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


