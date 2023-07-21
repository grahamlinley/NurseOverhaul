using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace NurseOverhaul
{
    public class BossChecklistIntegration : ModSystem
    {
        //Bosschecklist integration for checking Calamity boss kills 

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
                            key = boss.Value.ContainsKey("key") ? boss.Value["key"] as string : "",
                            modSource = boss.Value.ContainsKey("modSource") ? boss.Value["modSource"] as string : "",
                            internalName = boss.Value.ContainsKey("internalName") ? boss.Value["internalName"] as string : "",
                            displayName = boss.Value.ContainsKey("displayName") ? boss.Value["displayName"] as string : "",

                            progression = boss.Value.ContainsKey("progression") ? Convert.ToSingle(boss.Value["progression"]) : 0f,
                            downed = CreateDownedFunc(boss.Value.ContainsKey("downed") ? boss.Value["downed"] : true),

                            isBoss = boss.Value.ContainsKey("isBoss") ? Convert.ToBoolean(boss.Value["isBoss"]) : false,
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

        //end of integration code

        // Creating infrastructure here for checking if Calamity bosses are killed
        public static bool isCalamitasCloneDefeated()
        {
            // Check if integration with Boss Checklist was successful
            if (!IntegrationSuccessful)
            {
                // Integration was not successful, handle the error or return an appropriate value
                return false;
            }

            // Boss key for CalamityMod Providence
            string calamatisCloneKey = "CalamityMod The Calamitas Clone";

            // Check if the bossInfos dictionary contains the boss key
            if (bossInfos.TryGetValue(calamatisCloneKey, out BossChecklistBossInfo bossInfo))
            {
                // Check if the boss has been defeated by invoking the downed function
                bool isDefeated = bossInfo.downed();
                return isDefeated;
            }
            // Boss info for CalamityMod Calamitas Clone is not found, handle the error or return an appropriate value
            return false;
        }

        public static bool isPlaguebringerDefeated()
        {
            if (!IntegrationSuccessful)
            {
                return false;
            }
            string plaguebringerKey = "CalamityMod Plaguebringer Goliath";

            if (bossInfos.TryGetValue(plaguebringerKey, out BossChecklistBossInfo bossInfo))
            {
                bool isDefeated = bossInfo.downed();
                return isDefeated;
            }
            return false;
        }

        public static bool isRavagerDefeated()
        {
            if (!IntegrationSuccessful)
            {
                return false;
            }

            string ravagerKey = "CalamityMod Ravager";

            if (bossInfos.TryGetValue(ravagerKey, out BossChecklistBossInfo bossInfo))
            {
                bool isDefeated = bossInfo.downed();
                return isDefeated;
            }
            return false;
        }

        public static bool isProvidenceDefeated()
        {
            if (!IntegrationSuccessful)
            {
                return false;
            }
            string providenceKey = "CalamityMod Providence";

            if (bossInfos.TryGetValue(providenceKey, out BossChecklistBossInfo bossInfo))
            {
                bool isDefeated = bossInfo.downed();
                return isDefeated;
            }
            return false;
        }

        public static bool isDevourerDefeated()
        {
            if (!IntegrationSuccessful)
            {
                return false;
            }

            string devourerKey = "CalamityMod Devourer of Gods";

            if (bossInfos.TryGetValue(devourerKey, out BossChecklistBossInfo bossInfo))
            {
                bool isDefeated = bossInfo.downed();
                return isDefeated;
            }
            return false;
        }

        public static bool isYharonDefeated()
        {
            if (!IntegrationSuccessful)
            {
                return false;
            }
            string yharonKey = "CalamityMod Yharon";

            if (bossInfos.TryGetValue(yharonKey, out BossChecklistBossInfo bossInfo))
            {
                bool isDefeated = bossInfo.downed();
                return isDefeated;
            }
            return false;
        }
    }
}
