//TO DO: Downed check for calamity bosses
// COMBAT CHECK. Current guess is between 6 and 6.1309
// Piggy bank money count correct and accessed if inventoroy isn't (check on other potential money sources for purchase funding ie Vault)
// Customizable distance from nurse in config
// Play Nurse Heal sound effect on button press
// Hate toggle in config? Manual testing and new methods and logic reconfig. 
// Find some way to access Nurse happiness and use it to modify price automatically. Would be default in config with love/hate as options
// Icon for tmod
// readme
// DEBUG. Found error when stacking to chest + some other places (spawned something in and got an error, might be unrelated). Check log trace
// Dialogue for how much you spent at the nurse (button in box? maybe add total spent next to money just spent. could be too cluttery though)
// Stretch: Maybe resprite or just an animation when you press the hotkey? Would be funny for like an Ana type character that shoots a healing dart at you to heal you past a certain range

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace NurseHotkey
{
    public class NurseHotkey : Mod
    {
        internal static ModKeybind NurseHealHotkey;

        public override void Load()
        {
            NurseHealHotkey = KeybindLoader.RegisterKeybind(this, "Heal in range of Nurse", "G");
        }

        public override void Unload()
        {
            NurseHealHotkey = null;
        }
    }
}
