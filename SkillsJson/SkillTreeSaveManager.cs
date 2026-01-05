using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using SkillTree.SkillPatchSocial;
using SkillTree.SkillsJson;
using UnityEngine;

namespace SkillTree.Json
{
    public static class SkillTreeSaveManager
    {
        private static string ConfigPath => Path.Combine(MelonEnvironment.UserDataDirectory, "SkillTree_Config.json");

        public static string GetDynamicPath()
        {
            string saveID = GetCurrentSaveID();
            return Path.Combine(MelonEnvironment.UserDataDirectory, $"SkillTree_{saveID}.json");
        }

        public static SkillTreeData LoadOrCreate()
        {
            string path = GetDynamicPath();

            if (!File.Exists(path))
            {
                MelonLogger.Msg($"[SkillTree] Novo save detectado ou arquivo ausente: {path}");

                CustomerCache.IsLoaded = false;
                CustomerCache.OriginalMinSpend.Clear();
                CustomerCache.OriginalMaxSpend.Clear();

                var data = CreateDefault();
                Save(data); 
                return data;
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SkillTreeData>(json);
        }

        public static void Save(SkillTreeData data)
        {
            string path = GetDynamicPath(); 
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        private static SkillTreeData CreateDefault()
        {
            return new SkillTreeData();
        }

        public static string GetCurrentSaveID()
        {
            string fullPath = Singleton<LoadManager>.Instance.LoadedGameFolderPath;

            if (string.IsNullOrEmpty(fullPath))
                return "DefaultPlayer";

            return Path.GetFileName(fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        public static SkillConfig LoadConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                var newConfig = new SkillConfig();
                SaveConfig(newConfig);
                return newConfig;
            }
            try
            {
                string json = File.ReadAllText(ConfigPath);
                return JsonConvert.DeserializeObject<SkillConfig>(json);
            }
            catch
            {
                return new SkillConfig();
            }
        }

        public static void SaveConfig(SkillConfig config)
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }
    }
}
