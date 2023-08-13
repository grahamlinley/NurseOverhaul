using Terraria;
using Terraria.ModLoader;

namespace NurseOverhaul.Buffs
{
    public class NurseInRange : ModBuff
    {
        public override void SetStaticDefaults()
        {
             DisplayName.SetDefault("In Range of Nurse");
             Description.SetDefault("The Nurse is watching and ready to deploy a quick heal at your command");

            Main.debuff[Type] = false; // If this buff is a debuff that can apply to enemies
            Main.buffNoTimeDisplay[Type] = false; // If this buff's remaining duration should not be displayed
        }
    }

    public class NurseSweetSpot : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nurse's Sweet Spot");
            Description.SetDefault("You're close enough for reduced priced Nurse quick heals");

            Main.debuff[Type] = false; // If this buff is a debuff that can apply to enemies
            Main.buffNoTimeDisplay[Type] = false; // If this buff's remaining duration should not be displayed
        }
    }
}
