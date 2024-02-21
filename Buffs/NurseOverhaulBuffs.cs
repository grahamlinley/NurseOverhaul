using Terraria;
using Terraria.ModLoader;

namespace NurseOverhaul.Buffs
{
    public class NurseInRange : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; // If this buff is a debuff that can apply to enemies
            Main.buffNoTimeDisplay[Type] = false; // If this buff's remaining duration should not be displayed
        }
    }

    public class NurseSweetSpot : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; 
            Main.buffNoTimeDisplay[Type] = false; 
        }
    }
}
