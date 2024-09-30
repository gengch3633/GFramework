using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    [InitializeOnLoad]
    public class Define_ScriptingDefineSymbles
    {
        private static bool logEnabled = true;
        static Define_ScriptingDefineSymbles()
        {
            logEnabled = false;
            UpdateScriptDefineSymbles();
            logEnabled = true;
        }
        [MenuItem("Tools/SDK参数配置/2.设置宏定义参数")]
        private static void UpdateScriptDefineSymbles()
        {
            if(logEnabled) Debug.LogError($"==> [Define_ScriptingDefineSymbles] 1, [Start]");
            var scriptDefineInfos = GameUtils.GetConfigInfos<ScriptDefineInfo>();
            var targetPlatforms = new List<BuildTargetGroup>() { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };
            targetPlatforms.ForEach(targetPlatform => {
                
                string oldSymbles = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform);
                var origDefines = oldSymbles.Split(';').ToList();
                for (int i = 0; i < scriptDefineInfos.Count; i++)
                {
                    var scriptDefineInfo = scriptDefineInfos[i];
                    var defineSymble = scriptDefineInfo.scriptDefineSymble;
                    if (logEnabled) Debug.LogError($"==> [Define_ScriptingDefineSymbles] 21, [{targetPlatform}] [Check]: {scriptDefineInfo.scriptDefineSymble}, isToAdded: {scriptDefineInfo.isToAdded}");
                    if (scriptDefineInfo.isToAdded && !origDefines.Contains(defineSymble))
                    {
                        origDefines.Add(scriptDefineInfo.scriptDefineSymble);
                        if (logEnabled) Debug.LogError($"==> [Define_ScriptingDefineSymbles] 22, [{targetPlatform}] [Add]: {scriptDefineInfo.scriptDefineSymble}");
                    }

                    if (!scriptDefineInfo.isToAdded && origDefines.Contains(defineSymble))
                    {
                        origDefines.Remove(scriptDefineInfo.scriptDefineSymble);
                        if (logEnabled) Debug.LogError($"==> [Define_ScriptingDefineSymbles] 23, [{targetPlatform}] [Remove]: {scriptDefineInfo.scriptDefineSymble}");
                    }
                }

                var newSymbles = string.Join(";", origDefines);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, newSymbles);
            });
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            if (logEnabled) Debug.LogError($"==> [Define_ScriptingDefineSymbles] 3, [End]");
        }
    }

    public class ScriptDefineInfo
    {
        public int defineId;
        public string scriptDefineSymble;
        public bool isToAdded;
    }
}

