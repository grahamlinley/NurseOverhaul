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
        public bool NurseVIPBadgeEnabled { get; set; } = true;
        public bool NursesWalkieTalkieEnabled { get; set; } = true;
        public bool NursesPaintedShirtEnabled { get; set; } = true;
        public bool NurseNourishmentDiamondEnabled { get; set; } = true;
        public bool LifeCrystalInShop { get; set; } = false;
        public bool LifeFruitInShop { get; set; } = false;

    }
}