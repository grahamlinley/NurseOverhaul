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
    public class BossChecklistIntegration : ModSystem
{
    private static readonly Version BossChecklistAPIVersion = new Version(1, 1);
    public class BossChecklistBossInfo
    {
        internal string key = "";
        internal string modSource = "";
        internal string internalName = "";
        internal string displayName = "";

        internal float progression = 0f;
        internal Func<bool> downed = () => false;

        internal bool isBoss = true;
        internal bool isMiniboss = false;
        internal bool isEvent = false;

        internal List<int> npcIDs = new List<int>();
        internal List<int> spawnItem = new List<int>();
        internal List<int> loot = new List<int>();
        internal List<int> collection = new List<int>();
    }

    public static Dictionary<string, BossChecklistBossInfo> bossInfos = new Dictionary<string, BossChecklistBossInfo>();

    public static bool IntegrationSuccessful { get; private set; }

    public override void PostAddRecipes()
    {
        bossInfos.Clear();

        if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) && bossChecklist.Version >= BossChecklistAPIVersion)
        {
            object currentBossInfoResponse = bossChecklist.Call("GetBossInfoDictionary", Mod, BossChecklistAPIVersion.ToString());
            if (currentBossInfoResponse is Dictionary<string, Dictionary<string, object>> bossInfoList)
            {
                foreach (var boss in bossInfoList)
                {
                    BossChecklistBossInfo bossInfo = new BossChecklistBossInfo()
                    {
                        key = boss.Value.ContainsKey("key") ? boss.Value["key"] as string : "CalamityMod Providence",
                        modSource = boss.Value.ContainsKey("modSource") ? boss.Value["modSource"] as string : "CalamityMod",
                        internalName = boss.Value.ContainsKey("internalName") ? boss.Value["internalName"] as string : "CalamityMod Providence",
                        displayName = boss.Value.ContainsKey("displayName") ? boss.Value["displayName"] as string : "Providence, The Profaned Goddess",

                        progression = boss.Value.ContainsKey("progression") ? Convert.ToSingle(boss.Value["progression"]) : 19f,
                        downed = CreateDownedFunc(boss.Value.ContainsKey("downed") ? boss.Value["downed"] : true),

                        isBoss = boss.Value.ContainsKey("isBoss") ? Convert.ToBoolean(boss.Value["isBoss"]) : true,
                        isMiniboss = boss.Value.ContainsKey("isMiniboss") ? Convert.ToBoolean(boss.Value["isMiniboss"]) : false,
                        isEvent = boss.Value.ContainsKey("isEvent") ? Convert.ToBoolean(boss.Value["isEvent"]) : false,

                        npcIDs = boss.Value.ContainsKey("npcIDs") ? boss.Value["npcIDs"] as List<int> : new List<int>(),
                        spawnItem = boss.Value.ContainsKey("spawnItem") ? boss.Value["spawnItem"] as List<int> : new List<int>(),
                        loot = boss.Value.ContainsKey("loot") ? boss.Value["loot"] as List<int> : new List<int>(),
                        collection = boss.Value.ContainsKey("collection") ? boss.Value["collection"] as List<int> : new List<int>(),
                    };

                    bossInfos.Add(boss.Key, bossInfo);
                }

                IntegrationSuccessful = true;
            }
        }
    }

    private Func<bool> CreateDownedFunc(object downedValue)
    {
        if (downedValue is Func<bool> downedFunc)
            return downedFunc;
        else if (downedValue is bool downedBool)
            return () => downedBool;

        return () => false;
    }

    public override void Unload()
    {
        bossInfos.Clear();
    }

    public static float DownedBossProgress()
    {
        if (bossInfos.Count == 0)
            return 0;

        return (float)bossInfos.Count(x => x.Value.downed()) / bossInfos.Count();
    }

    public static bool BossDowned(string bossKey) => bossInfos.TryGetValue(bossKey, out var bossInfo) ? bossInfo.downed() : false;
}
}


/* using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;

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

    public class BossChecklistIntegration : ModSystem
    {
        private static Dictionary<string, BossChecklistBossInfo> bossInfos;

        public static bool IntegrationSuccessful { get; private set; }

        public static void Initialize()
        {
            bossInfos = new Dictionary<string, BossChecklistBossInfo>();
            IntegrationSuccessful = false;
        }

        public static bool BossDowned(string bossKey) => bossInfos.TryGetValue(bossKey, out var bossInfo) ? bossInfo.Downed() : false;

        public static float DownedBossProgress()
        {
            if (bossInfos.Count == 0)
                return 0f;

            return (float)bossInfos.Count(x => x.Value.Downed()) / bossInfos.Count();
        }

        public override void PostAddRecipes()
        {
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) && bossChecklist.Version >= new Version(1, 1))
            {
                object currentBossInfoResponse = bossChecklist.Call("GetBossInfoDictionary", Mod, "1.1");
                if (currentBossInfoResponse is Dictionary<string, Dictionary<string, object>> bossInfoList)
                {
                    bossInfos = bossInfoList.ToDictionary(boss => boss.Key, boss => new BossChecklistBossInfo()
                    {
                        Downed = () => false
                    });

                    if (bossInfos.TryGetValue("CalamityMod:Providence", out var providenceBossInfo))
                    {
                        providenceBossInfo.Downed = () => BossDowned("CalamityMod:Providence");
                    }

                    if (bossInfos.TryGetValue("CalamityMod:DevourerOfGods", out var devourerBossInfo))
                    {
                        devourerBossInfo.Downed = () => BossDowned("CalamityMod:DevourerOfGods");
                    }

                    if (bossInfos.TryGetValue("CalamityMod:Yharon", out var yharonBossInfo))
                    {
                        yharonBossInfo.Downed = () => BossDowned("CalamityMod:Yharon");
                    }

                    IntegrationSuccessful = true;
                }
            }
        }

        public override void Unload()
        {
            bossInfos.Clear();
            bossInfos = null;
            IntegrationSuccessful = false;
        }

        public class BossChecklistBossInfo
        {
            public Func<bool> Downed { get; set; }
        }
    }
}
*/



/* using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
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
    
    // This class provides an example of advanced Boss Checklist integration utilizing the "GetBossInfoDictionary" Mod.Call that other Mods can copy into their mod's source code.
    // If you are simply adding support for bosses in your mod to Boss Checklist, this is not what you want. Go read https://github.com/JavidPack/BossChecklist/wiki/Support-using-Mod-Call
    // By copying this class into your mod, you can access Boss Checklist boss data reliably and with type safety without requiring a strong dependency.
    public class BossChecklistIntegration : ModSystem
{
        private static int NPCType(string npcName)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.FullName == npcName)
                {
                    return npc.type;
                }
            }
            return 0;
        }
        
        public static bool BossDowned1(string bossKey)
        {
            return bossInfos.TryGetValue(bossKey, out var bossInfo) && bossInfo.downed();
        }


        // Boss Checklist might add new features, so a version is passed into GetBossInfo. 
        // If a new version of the GetBossInfo Call is implemented, find this class in the Boss Checklist Github once again and replace this version with the new version: https://github.com/JavidPack/BossChecklist/blob/master/BossChecklistIntegrationExample.cs
        private static readonly Version BossChecklistAPIVersion = new Version(1, 1); // Do not change this yourself.

        public class BossChecklistBossInfo
    {
        internal string key = ""; // equal to "modSource internalName"
        internal string modSource = "";
        internal string internalName = "";
        internal string displayName = "";

        internal float progression = 0f; // See https://github.com/JavidPack/BossChecklist/blob/master/BossTracker.cs#L13 for vanilla boss values
        internal Func<bool> downed = () => false;

        internal bool isBoss = false;
        internal bool isMiniboss = false;
        internal bool isEvent = false;

        internal List<int> npcIDs = new List<int>(); // Does not include minions, only npcids that count towards the NPC still being alive.
        internal List<int> spawnItem = new List<int>();
        internal List<int> loot = new List<int>();
        internal List<int> collection = new List<int>();
    }

    public static Dictionary<string, BossChecklistBossInfo> bossInfos = new Dictionary<string, BossChecklistBossInfo>();

    public static bool IntegrationSuccessful { get; private set; }

        public override void PostAddRecipes()
        {
            // For best results, this code is in PostAddRecipes
            bossInfos.Clear();

            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist) && bossChecklist.Version >= BossChecklistAPIVersion)
            {
                object currentBossInfoResponse = bossChecklist.Call("GetBossInfoDictionary", Mod, BossChecklistAPIVersion.ToString());
                if (currentBossInfoResponse is Dictionary<string, Dictionary<string, object>> bossInfoList)
                {
                    bossInfos = bossInfoList.ToDictionary(boss => boss.Key, boss => new BossChecklistBossInfo()
                    {
                        key = boss.Value.ContainsKey("key") ? boss.Value["key"] as string : "",
                        modSource = boss.Value.ContainsKey("modSource") ? boss.Value["modSource"] as string : "",
                        internalName = boss.Value.ContainsKey("internalName") ? boss.Value["internalName"] as string : "",
                        displayName = boss.Value.ContainsKey("displayName") ? boss.Value["displayName"] as string : "",

                        progression = boss.Value.ContainsKey("progression") ? Convert.ToSingle(boss.Value["progression"]) : 0f,
                        downed = boss.Value.ContainsKey("downed") ? boss.Value["downed"] as Func<bool> : () => false,

                        isBoss = boss.Value.ContainsKey("isBoss") ? Convert.ToBoolean(boss.Value["isBoss"]) : false,
                        isMiniboss = boss.Value.ContainsKey("isMiniboss") ? Convert.ToBoolean(boss.Value["isMiniboss"]) : false,
                        isEvent = boss.Value.ContainsKey("isEvent") ? Convert.ToBoolean(boss.Value["isEvent"]) : false,

                        npcIDs = boss.Value.ContainsKey("npcIDs") ? boss.Value["npcIDs"] as List<int> : new List<int>(),
                        spawnItem = boss.Value.ContainsKey("spawnItem") ? boss.Value["spawnItem"] as List<int> : new List<int>(),
                        loot = boss.Value.ContainsKey("loot") ? boss.Value["loot"] as List<int> : new List<int>(),
                        collection = boss.Value.ContainsKey("collection") ? boss.Value["collection"] as List<int> : new List<int>(),
                    });

                    string calamitasInternalName = "Calamitas";
                    string plaguebringerInternalName = "PlaguebringerGoliath";
                    string providenceInternalName = "Providence";
                    string devourerInternalName = "DevourerOfGods";
                    string yharonInternalName = "Yharon";

                    if (ModLoader.Mods.Any(mod => mod.Name == "CalamityMod"))
                    {
                        if (bossInfos.TryGetValue($"{"CalamityMod"}:{calamitasInternalName}", out var calamitasBossInfo))
                        {
                            calamitasBossInfo.downed = () => BossDowned(calamitasInternalName);
                        }

                        if (bossInfos.TryGetValue($"{"CalamityMod"}:{plaguebringerInternalName}", out var plaguebringerBossInfo))
                        {
                            plaguebringerBossInfo.downed = () => BossDowned(plaguebringerInternalName);
                        }

                        if (bossInfos.TryGetValue($"{"CalamityMod"}:{providenceInternalName}", out var providenceBossInfo))
                        {
                            providenceBossInfo.downed = () => BossDowned(providenceInternalName);
                        }

                        if (bossInfos.TryGetValue($"{"CalamityMod"}:{devourerInternalName}", out var devourerBossInfo))
                        {
                            devourerBossInfo.downed = () => BossDowned(devourerInternalName);
                        }

                        if (bossInfos.TryGetValue($"{"CalamityMod"}:{yharonInternalName}", out var yharonBossInfo))
                        {
                            yharonBossInfo.downed = () => BossDowned(yharonInternalName);
                        }

                        IntegrationSuccessful = true;
                    }
                }
            }
        }

        public override void Unload()
        {
            bossInfos.Clear();
        }

    // This method shows an example of using the BossChecklistBossInfo data for something cool in your mod.
    public static float DownedBossProgress()
    {
        if (bossInfos.Count == 0) // bossInfos might be empty, if BossChecklist isn't present or something goes wrong.
            return 0;

        return (float)bossInfos.Count(x => x.Value.downed()) / bossInfos.Count();
    }

    // This utility method shows how you can easily check downed bosses from mods without worrying about the typical cross mod headaches like reflection, strong/weak references, and obtaining dll files to reference.
    public static bool BossDowned(string bossKey) => bossInfos.TryGetValue(bossKey, out var bossInfo) ? bossInfo.downed() : false;

    // Your other methods and classes...
}

/*






/* using System;
using System.Collections.Generic;
using log4net;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using BossChecklist;

namespace NurseHotkey
{
    public class NurseHotkey : Mod
    {
        static internal NurseHotkey instance;
        internal static ModKeybind NurseHealHotkey;
        internal static bool calamityLoaded; // Added calamityLoaded here
        public const float SlimeKing = 1f;
        public const float EyeOfCthulhu = 2f;
        public const float EaterOfWorlds = 3f;
        public const float QueenBee = 4f;
        public const float Skeletron = 5f;
        public const float WallOfFlesh = 6f;
        public const float TheTwins = 7f;
        public const float TheDestroyer = 8f;
        public const float SkeletronPrime = 9f;
        public const float Plantera = 10f;
        public const float Golem = 11f;
        public const float DukeFishron = 12f;
        public const float LunaticCultist = 13f;
        public const float Moonlord = 14f;

        List<BossInfo> allBosses = new List<BossInfo>
        {
            // Calamity
            new BossInfo("Desert Scourge", SlimeKing + .5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedDesertScourge()),
            new BossInfo("The Hive Mind", QueenBee + .5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedHiveMind()),
            new BossInfo("The Perforator", QueenBee + .5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedPerforator()),
            new BossInfo("Slime God", Skeletron + 0.5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedSlimeGod()),
            new BossInfo("Cryogen", WallOfFlesh + 0.5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedCryogen()),
            new BossInfo("Calamitas", Plantera - 0.5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedCalamitas()),
            new BossInfo("The Devourer of Gods", Golem - 0.5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedDoG()),
            new BossInfo("Plaguebringer Goliath", Golem + 0.5f, () => NurseHotkey.calamityLoaded, () => CalamityMod.CalamityWorld.downedPlaguebringer()),
            // CalamityMod.CalamityWorld.downedYharon
        };

        internal void AddBoss(string bossname, float bossValue, Func<bool> bossDowned)
        {
            allBosses.Add(new BossInfo(bossname, bossValue, () => true, bossDowned));
        }

        public override void Load()
        {
            instance = this;
            NurseHealHotkey = KeybindLoader.RegisterKeybind(this, "Heal in range of Nurse", "G");
        }

        public override void Unload()
        {
            NurseHealHotkey = null;
        }

        public override void PostSetupContent()
        {
            try
            {
                calamityLoaded = ModLoader.GetMod("CalamityMod") != null;

                // Check if BossChecklist mod is present
                Mod bossChecklistMod = ModLoader.GetMod("BossChecklist");
                if (bossChecklistMod != null)
                {
                    // Use reflection to access BossChecklistBossInfo class
                    Type bossInfoType = bossChecklistMod.Code.GetType("BossChecklist.BossChecklistBossInfo");
                    if (bossInfoType != null)
                    {
                        // Get the field for the bossInfos dictionary
                        System.Reflection.FieldInfo bossInfosField = bossInfoType.GetField("bossInfos");
                        if (bossInfosField != null)
                        {
                            // Retrieve the current dictionary instance
                            Dictionary<string, BossChecklistBossInfo> bossInfos = (Dictionary<string, BossChecklistBossInfo>)bossInfosField.GetValue(null);

                            // Add custom bosses to bossInfos dictionary
                            foreach (BossInfo bossInfo in allBosses)
                            {
                                // Create a new BossChecklistBossInfo instance
                                BossChecklistBossInfo customBossInfo = new BossChecklistBossInfo();

                                // Set the downed method for the boss
                                customBossInfo.downed = bossInfo.downed;

                                // Add the custom boss to the bossInfos dictionary
                                bossInfos.Add(bossInfo.name, customBossInfo);
                            }
                        }
                    }
                }

                // Add custom bosses
                AddBoss("CustomBoss1", 15f, () => SomeCondition1());
                AddBoss("CustomBoss2", 20f, () => SomeCondition2());
                // Add more custom bosses if needed
            }
            catch (Exception e)
            {
                Logger.Error($"PostSetupContent Error: {e.StackTrace} {e.Message}");
            }
        }

        // Your other methods and classes...

        // Integration code for Boss Checklist

        private bool SomeCondition1()
        {
            // Check if the condition for CustomBoss1 is met
            return true;
        }

        private bool SomeCondition2()
        {
            // Check if the condition for CustomBoss2 is met
            return true;
        }
    }

    public class BossInfo
    {
        internal Func<bool> available;
        internal Func<bool> downed;
        internal string name;
        internal float progression;

        public BossInfo(string name, float progression, Func<bool> available, Func<bool> downed)
        {
            this.name = name;
            this.progression = progression;
            this.available = available;
            this.downed = downed;
        }
    }
}

*/
