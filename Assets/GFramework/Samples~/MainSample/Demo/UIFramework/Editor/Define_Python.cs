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
            var path = utilsPyFile.FullName;
            var sArguments = path;


            var languageKeys = GetTMPLanguageKeys();
            p.StartInfo.FileName = "python";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = $"{sArguments} {languageKeys}";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.BeginOutputReadLine();
            p.OutputDataReceived += new DataReceivedEventHandler(Out_RecvData);
            p.WaitForExit();
        }

        private static string GetTMPLanguageKeys()
        {
            var subFolders = Directory.GetDirectories("Assets");
            var guids = AssetDatabase.FindAssets("t:Prefab", subFolders).ToList();
            var assetPaths = guids.ConvertAll(item=> AssetDatabase.GUIDToAssetPath(item));
            var prefabs = assetPaths.ConvertAll(item => AssetDatabase.LoadAssetAtPath<GameObject>(item));
            var allHasKeys = new List<string>();
            var emptyKeys = new List<string>();
            var gameObjectMapWithoutLocalizedText = new Dictionary<string, List<string>>();
            prefabs.ForEach(item =>
            {
                var allTMPs = item.GetComponentsInChildren<TextMeshProUGUI>().ToList();
                var tmpLocalizedTexts = allTMPs.FindAll(item => item.GetComponent<LocalizedText>() != null).ConvertAll(item=> item.GetComponent<LocalizedText>());
                var tmpsWithoutLocalizedText = allTMPs.FindAll(item => item.GetComponent<LocalizedText>() == null);
                var hasKeyTmps = tmpLocalizedTexts.FindAll(item => !string.IsNullOrEmpty(item.key));
                //UnityEngine.Debug.LogError($"==> [GetTMPLanguageKeys], prafab: {item.name}, tmpLocalizedTexts 1: {JsonConvert.SerializeObject(tmpLocalizedTexts.ConvertAll(item=>item.name), Formatting.Indented)}");
                //UnityEngine.Debug.LogError($"==> [GetTMPLanguageKeys], prafab: {item.name}, tmpLocalizedTexts 2: {JsonConvert.SerializeObject(hasKeyTmps.ConvertAll(item => item.name), Formatting.Indented)}");
                //UnityEngine.Debug.LogError($"==> [GetTMPLanguageKeys], prafab: {item.name}, tmpLocalizedTexts 3: {JsonConvert.SerializeObject(hasKeyTmps.ConvertAll(item => item.key), Formatting.Indented)}");
                allHasKeys.AddRange(hasKeyTmps.ConvertAll(item=>item.key));
                var noKeyTmps = tmpLocalizedTexts.FindAll(item => string.IsNullOrEmpty(item.key));
                emptyKeys.AddRange(noKeyTmps.ConvertAll(item => item.key));
                if (tmpsWithoutLocalizedText.Count > 0)
                    gameObjectMapWithoutLocalizedText.Add(item.name, tmpsWithoutLocalizedText.ConvertAll(item=>item.name));
            });

            UnityEngine.Debug.LogError($"==> [GetTMPLanguageKeys], gameObjectMapWithoutLocalizedText: {JsonConvert.SerializeObject(gameObjectMapWithoutLocalizedText, Formatting.Indented)}");
            UnityEngine.Debug.LogError($"==> [GetTMPLanguageKeys], emptyKeys: {JsonConvert.SerializeObject(emptyKeys, Formatting.Indented)}");

            return string.Join("-", allHasKeys);
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
                var folderName = new DirectoryInfo(item.DirectoryName).Name;
                var folder = folderName != "Data" ? $"{encryptConfigDataFolder}/{folderName}": $"{encryptConfigDataFolder}";
                var subFolder = new DirectoryInfo(folder);
                if (!subFolder.Exists)
                    subFolder.Create();
                var decryptFile =  $"{folder}/{item.Name}";
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

