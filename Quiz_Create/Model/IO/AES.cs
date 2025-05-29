using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Quiz_Create.Model.IO
{
    public static class AES
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("najtajniejsze_haslo_na_swiecie!!");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("IV_dzien_dorby<3");

        public static byte[] EncryptString(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cryptoStream);

            sw.Write(plainText);
            sw.Flush();
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }

        public static string DecryptBytes(byte[] cipherData)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherData);
            using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cryptoStream);

            return sr.ReadToEnd();
        }
    }
}
