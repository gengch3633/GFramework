using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    public class Define_Python
    {
        [MenuItem("Tools/SDK参数配置/5.Python生成配置文件")]

        private static void CreateConfig()
        {
            CreateSimpleConfigData();
            CreateEncryptConfigData();
        }
        public static void CreateSimpleConfigData()
        {
            var tmpResourceDirInfo = new DirectoryInfo("Assets");
            var fontAssets = tmpResourceDirInfo.GetFiles("*.py", SearchOption.AllDirectories).ToList();
            var utilsPyFile = fontAssets.Find(item => item.Name == "Utils.py");
                UnityEngine.Debug.LogError(utilsPyFile);
            Process p = new Process();
            string path = utilsPyFile.FullName;
            string sArguments = path;

            p.StartInfo.FileName = "python";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = sArguments;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.BeginOutputReadLine();
            p.OutputDataReceived += new DataReceivedEventHandler(Out_RecvData);
            p.WaitForExit();
        }

        private static void CreateEncryptConfigData()
        {
            var simpleConfigDataFolder = "Assets/Game/Data";
            var encryptConfigDataFolder = "Assets/Game/Resources/Data";
            var simpleConfigDataDir = new DirectoryInfo(simpleConfigDataFolder);
            var encryptConfigDataDir = new DirectoryInfo(encryptConfigDataFolder);
            if (!simpleConfigDataDir.Exists)
                simpleConfigDataDir.Create();
            if (!encryptConfigDataDir.Exists)
                encryptConfigDataDir.Create();

            var jsonFiles = simpleConfigDataDir.GetFiles("*.json", SearchOption.AllDirectories).ToList();
            jsonFiles.ForEach(item =>
            {
                var simpleDataText = File.ReadAllText(item.FullName);
                var encyptDataText = GameUtils.AESEncrypt(simpleDataText);
                var decryptDataText = GameUtils.AESDecrypt(encyptDataText);
                var isDecryptSuccess = simpleDataText == decryptDataText;
                var decryptFile = $"{encryptConfigDataFolder}/{item.Name}";
                File.WriteAllText(decryptFile, encyptDataText);
                UnityEngine.Debug.LogError($"==> [Python] [CreateEncryptConfigData]: {item.Name}, isDecryptSuccess: {isDecryptSuccess}");
            });
        }

        static void Out_RecvData(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                UnityEngine.Debug.LogError($"==> [Python] [Out_RecvData]: {e.Data}");
        }
    }
}

