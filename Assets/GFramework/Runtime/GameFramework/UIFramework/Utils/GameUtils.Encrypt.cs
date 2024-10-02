using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GameFramework
{
    public partial class GameUtils
    {
        private static HexConverter hexConverter = new HexConverter();
        private static string key = "KeysKeysKeysKeys";//密钥使用128、192 和 256bit三种
        private static string IV = "KeysKeysTestTest";//密钥偏移量
        //加密
        public static string AESEncrypt(string info)
        {
            var infoByte = Encoding.UTF8.GetBytes(info);
            RijndaelManaged aesM = new RijndaelManaged();
            aesM.Key = Encoding.UTF8.GetBytes(key);
            aesM.IV = Encoding.UTF8.GetBytes(IV);
            aesM.Mode = CipherMode.ECB;
            aesM.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTrans = aesM.CreateEncryptor();
            var resultByte = cTrans.TransformFinalBlock(infoByte, 0, infoByte.Length);
            var AesStr = hexConverter.BytesToHexStr(resultByte);
            return AesStr;
        }

        public static string AESDecrypt(string keyData)
        {
            var infoByte = hexConverter.HexStrToBytes(keyData);
            RijndaelManaged aesM = new RijndaelManaged();
            aesM.Key = Encoding.UTF8.GetBytes(key);
            aesM.IV = Encoding.UTF8.GetBytes(IV);
            aesM.Mode = CipherMode.ECB;
            aesM.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTrans = aesM.CreateDecryptor();
            var resultByte = cTrans.TransformFinalBlock(infoByte, 0, infoByte.Length);
            var AesStr = Encoding.UTF8.GetString(resultByte);
            return AesStr;
        }
    }
}