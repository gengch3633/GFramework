using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    public class ScriptDefine
    {
        [MenuItem("Tools/UpdateScriptDefineSymbles")]
        static void UpdateScriptDefineSymbles()
        {
            Debug.LogError($"==> [UpdateScriptDefineSymbles] [Start]");
            var scriptDefineInfos = GameUtils.GetConfigInfos<ScriptDefineInfo>();
            var targetPlatforms = new List<BuildTargetGroup>() { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };
            targetPlatforms.ForEach(targetPlatform => {
                
                string oldSymbles = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform);
                var origDefines = oldSymbles.Split(';').ToList();
                for (int i = 0; i < scriptDefineInfos.Count; i++)
                {
                    var scriptDefineInfo = scriptDefineInfos[i];
                    var defineSymble = scriptDefineInfo.scriptDefineSymble;
                    Debug.LogError($"==> [UpdateScriptDefineSymbles] [{targetPlatform}] [Check]: {scriptDefineInfo.scriptDefineSymble}, isToAdded: {scriptDefineInfo.isToAdded}");
                    if (scriptDefineInfo.isToAdded && !origDefines.Contains(defineSymble))
                    {
                        origDefines.Add(scriptDefineInfo.scriptDefineSymble);
                        Debug.LogError($"==> [UpdateScriptDefineSymbles] [{targetPlatform}] [Add]: {scriptDefineInfo.scriptDefineSymble}");
                    }

                    if (!scriptDefineInfo.isToAdded && origDefines.Contains(defineSymble))
                    {
                        origDefines.Remove(scriptDefineInfo.scriptDefineSymble);
                        Debug.LogError($"==> [UpdateScriptDefineSymbles] [{targetPlatform}] [Remove]: {scriptDefineInfo.scriptDefineSymble}");
                    }
                }

                var newSymbles = string.Join(";", origDefines);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, newSymbles);
            });
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Debug.LogError($"==> [UpdateScriptDefineSymbles] [End]");
        }
    }
}

