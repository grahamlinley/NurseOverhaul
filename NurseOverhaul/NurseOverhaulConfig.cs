﻿using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace NurseOverhaul
{
    public class NurseOverhaulConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        //[Label("Nurse's VIP Badge Enabled")]
        //[Tooltip("Enabled by default")]
        [DefaultValue(true)]
        public bool NurseVIPBadgeEnabled { get; set; } = true;

        //[Label("Nurse's Walkie Talkie Enabled")]
        //[Tooltip("Enabled by default")]
        [DefaultValue(true)]
        public bool NursesWalkieTalkieEnabled { get; set; } = true;

        //[Label("Nurse's Painted Shirt Enabled")]
        //[Tooltip("Enabled by default")]
        [DefaultValue(true)]
        public bool NursesPaintedShirtEnabled { get; set; } = true;

        //[Label("Nurse Nourishment Diamond Enabled")]
        //[Tooltip("Enabled by default")]
        [DefaultValue(true)]
        public bool NurseNourishmentDiamondEnabled { get; set; } = true;

        //[Label("Life Crystal in Shop after Eye of Cthulhu")]
        //[Tooltip("Disabled by default")]
        [DefaultValue(false)]
        public bool LifeCrystalInShop { get; set; } = false;

        //[Label("Life Fruit in Shop after Mech Bosses")]
        //[Tooltip("Disabled by default")]
        [DefaultValue(false)]
        public bool LifeFruitInShop { get; set; } = false;

    }
}