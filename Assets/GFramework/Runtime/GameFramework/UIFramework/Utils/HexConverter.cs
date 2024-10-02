using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 由于数据加密之后使用utf8等编码格式转换会造成乱码，
/// 需要转换成另一种方式防止乱码导致数据丢失，
/// 此处用于把加密后的流数据转成十六进制数据字符串可以保存，
/// 也可以使用toBase64编码转换成字符串保存
/// </summary>
public class HexConverter
{
    //流数据转换为16进制数据
    public string BytesToHexStr(byte[] info)
    {
        StringBuilder sbuild = new StringBuilder();
        foreach (var item in info)
        {
            sbuild.AppendFormat("{0:X2}", item);
        }
        return sbuild.ToString();
    }
    //十六进制字符串转换为流数据
    public byte[] HexStrToBytes(string info)
    {
        if (string.IsNullOrEmpty(info))
            return null;
        List<byte> byteHex = new List<byte>();
        for (int i = 0; i < info.Length; i += 2)
        {
            string temp = info.Substring(i, 2);
            byteHex.Add(System.Convert.ToByte(temp, 16));
        }
        return byteHex.ToArray();
    }
}
