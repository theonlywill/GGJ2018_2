using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Encryption  
{
    public static string key = "d2a48a96";

    public static string Decrypt(string encryptedString)
    {
        DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
        desProvider.Mode = CipherMode.ECB;
        desProvider.Padding = PaddingMode.PKCS7;
        desProvider.Key = Encoding.ASCII.GetBytes(key);
        using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedString)))
        {
            using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
            {
                using (StreamReader sr = new StreamReader(cs, Encoding.ASCII))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

    public static string Encrypt(string decryptedString)
    {
        DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
        desProvider.Mode = CipherMode.ECB;
        desProvider.Padding = PaddingMode.PKCS7;
        desProvider.Key = Encoding.ASCII.GetBytes(key);
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] data = Encoding.Default.GetBytes(decryptedString);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }

}
