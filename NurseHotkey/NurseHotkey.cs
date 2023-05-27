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
