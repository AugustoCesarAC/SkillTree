using MelonLoader;
using MelonLoader.Utils;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using SkillTree.SkillPatchSocial;
using UnityEngine;

namespace SkillTree.Json
{
    public static class SkillTreeSaveManager
    {
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
    }
}
