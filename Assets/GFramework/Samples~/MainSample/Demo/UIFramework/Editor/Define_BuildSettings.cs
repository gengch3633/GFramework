using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    public class Define_BuildSettings
    {
        [MenuItem("Tools/SDK参数配置/6.1 Set GradlePath")]

        private static void SetAndroidGradlePath()
        {
            var buildSettings = GameUtils.GetConfigInfos<BuildSettings>()[0];
            EditorPrefs.SetBool("GradleUseEmbedded", false);
            string path = buildSettings.gradlePath;
            EditorPrefs.SetString("GradlePath", path);
        }

        [MenuItem("Tools/SDK参数配置/6.2 Set GlobalEnv")]

        private static void ToolsSettings()
        {
            var tmpResourceDirInfo = new DirectoryInfo("Assets");
            var fontAssets = tmpResourceDirInfo.GetFiles("*.py", SearchOption.AllDirectories).ToList();
            var utilsPyFile = fontAssets.Find(item => item.Name == "BatUtils.py");
            Process p = new Process();
            var path = utilsPyFile.FullName;
            var sArguments = path;


            p.StartInfo.FileName = "python";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = $"{sArguments}";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.BeginOutputReadLine();
            p.OutputDataReceived += new DataReceivedEventHandler(Out_RecvData);
            p.WaitForExit();
        }

        private static void Out_RecvData(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;
            var info = e.Data;
            if (!info.Contains("[BatUtils]"))
                return;

            var isUrlPath = info.Contains("http://");
            if (isUrlPath)
            {
                UnityEngine.Debug.Log($"==> [Define_BuildSettings] {e.Data}");
                return;
            }

            if(info.Contains("isValueExists: False"))
                UnityEngine.Debug.LogError($"==> [Define_BuildSettings] {e.Data}");
            if (info.Contains("isValueExists: True"))
                UnityEngine.Debug.Log($"==> [Define_BuildSettings] {e.Data}");
        }
    }

    public class BuildSettings
    {
        public string gradlePath;
    }
}

