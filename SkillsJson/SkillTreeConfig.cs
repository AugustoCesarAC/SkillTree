using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkillTree.SkillsJson
{
    [System.Serializable]
    public class SkillConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public UnityEngine.KeyCode MenuHotkey = UnityEngine.KeyCode.C;
    }
}
