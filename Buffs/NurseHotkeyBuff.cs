using Terraria;
using Terraria.ModLoader;

namespace NurseHotkey.Buffs
{
    public class NurseInRange : ModBuff
    {
        public void SetDefaults()
        {
            DisplayName.SetDefault("My Custom Buff");
            Description.SetDefault("This is a custom buff.");

            // These settings determine whether the buff can be applied to NPCs or Players, respectively.
            Main.debuff[Type] = true; // If this buff is a debuff that can apply to enemies
            Main.buffNoTimeDisplay[Type] = false; // If this buff's remaining duration should not be displayed
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // You can put any logic here that should occur each tick while the player has the buff.
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // You can put any logic here that should occur each tick while the NPC has the buff.
        }
    }
}
