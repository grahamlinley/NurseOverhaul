using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace NurseOverhaul

    // Configuration options player can set
{
    public class NurseOverhaulConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide; // Deleted most 1.4.3 extranenous code but think this is still necessary. Keeping in for now
        [DefaultValue(true)]
        public bool NurseVIPBadgeEnabled { get; set; } = true;
        [DefaultValue(true)]
        public bool NursesWalkieTalkieEnabled { get; set; } = true;
        [DefaultValue(true)]
        public bool NursesPaintedShirtEnabled { get; set; } = true;
        [DefaultValue(true)]
        public bool NurseNourishmentDiamondEnabled { get; set; } = true;

        [DefaultValue(30)]
        [Range(5, int.MaxValue)]
        public int LifeCrystalPrice { get; set; } = 30;
        public bool LifeCrystalInShop { get; set; } = false;

        [DefaultValue(35)]
        [Range(8, int.MaxValue)]
        public int LifeFruitPrice { get; set; } = 35;
        public bool LifeFruitInShop { get; set; } = false;

    }
}