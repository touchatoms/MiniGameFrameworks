using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

public partial class UtilsHttp
{
    // CMD + Action
    public static string GetCmdAction(JObject jobj)
    {
        string cmd = (string)jobj["cmd"];

        if (jobj.ContainsKey("result"))
        {
            return cmd;
        }

        JToken jresult = jobj["result"];
        string action = (string)jresult["action"];
        if (action == null)
        {
            return cmd;
        }

        return string.Format("{0}_{1}", cmd, action);
    }

    // MD5
    public static string GetMd5(string source)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            return sBuilder.ToString();
        }
    }

    // NiCai
    public static string DesNicaiAndBase64(string source)
    {
        byte[] tmp = EncryptDesEcb("Yq~" + source + "~POKER202305031548", "yqcom");
        return Convert.ToBase64String(tmp);
    }

    static byte[] EncryptDesEcb(string sourceStr, string key)
    {
        var utf8 = Encoding.UTF8;
        byte[] sourceBytes = utf8.GetBytes(sourceStr);
        int sourceLen = sourceBytes.Length;
        int inputLen = sourceLen / 8 * 8 + 8;
        if (inputLen > sourceLen)
        {
            byte[] tmpBytes = new byte[inputLen];
            Array.Copy(sourceBytes, tmpBytes, sourceBytes.Length);
            for (int i = 0; i < inputLen - sourceLen; i++)
            {
                tmpBytes[sourceLen + i] += ((byte)'0');
            }

            sourceBytes = tmpBytes;
        }

        byte[] keyBytes = new byte[8];
        for (int i = 0; i < 8; i++)
        {
            keyBytes[i] = Convert.ToByte(key[i]);
        }

        byte[] encryptBytes = new byte[inputLen];
        for (int i = 0; i < inputLen / 8; i++)
        {
            byte[] encryptSub = new byte[8];
            int k = 0;
            for (int j = i * 8; j < (i + 1) * 8; j++)
            {
                encryptSub[k] = sourceBytes[j]; // Convert.ToByte(source[j]);
                k++;
            }

            byte[] encrypted = Encrypt8(encryptSub, keyBytes);
            k = 0;
            for (int j = i * 8; j < (i + 1) * 8; j++)
            {
                encryptBytes[j] = encrypted[k];
                k++;
            }
        }

        return encryptBytes;
    }

    static byte[] Encrypt8(byte[] data, byte[] keys)
    {
        DES des = new DESCryptoServiceProvider();
        des.Mode = CipherMode.ECB;
        des.Key = keys;
        ICryptoTransform encryptor = des.CreateEncryptor();
        return encryptor.TransformFinalBlock(data, 0, data.Length);
    }
}