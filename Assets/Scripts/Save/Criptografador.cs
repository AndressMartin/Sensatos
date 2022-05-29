using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class Criptografador : MonoBehaviour
{
    // FileStream used for reading and writing files.
    static FileStream dataStream;

    // Key for reading and writing encrypted data.
    // (Chave super secreta, significa: Tomas Turbando)
    static byte[] savedKey = { 0x12, 0x14, 0x13, 0x11, 0x17, 0x15, 0x15, 0x15, 0x18, 0x16, 0x11, 0x12, 0x19, 0x18, 0x14, 0x17 };

    public static string ReadFile(string caminhoDoArquivo)
    {
        if (File.Exists(caminhoDoArquivo))
        {
            // Create FileStream for opening files.
            dataStream = new FileStream(caminhoDoArquivo, FileMode.Open);

            // Create new AES instance.
            Aes oAes = Aes.Create();

            // Create an array of correct size based on AES IV.
            byte[] outputIV = new byte[oAes.IV.Length];

            // Read the IV from the file.
            dataStream.Read(outputIV, 0, outputIV.Length);

            // Create CryptoStream, wrapping FileStream
            CryptoStream oStream = new CryptoStream(
                    dataStream,
                    oAes.CreateDecryptor(savedKey, outputIV),
                    CryptoStreamMode.Read);

            // Create a StreamReader, wrapping CryptoStream
            StreamReader reader = new StreamReader(oStream);

            // Read the entire file into a String value.
            string text = reader.ReadToEnd();

            // Close StreamReader.
            reader.Close();

            // Close CryptoStream.
            oStream.Close();

            // Close FileStream.
            dataStream.Close();

            return text;
        }

        return null;
    }

    public static void WriteFile(string caminhoDoArquivo, string save)
    {
        // Create new AES instance.
        Aes iAes = Aes.Create();

        // Create a FileStream for creating files.
        dataStream = new FileStream(caminhoDoArquivo, FileMode.Create);

        // Save the new generated IV.
        byte[] inputIV = iAes.IV;

        // Write the IV to the FileStream unencrypted.
        dataStream.Write(inputIV, 0, inputIV.Length);

        // Create CryptoStream, wrapping FileStream.
        CryptoStream iStream = new CryptoStream(
                dataStream,
                iAes.CreateEncryptor(savedKey, iAes.IV),
                CryptoStreamMode.Write);

        // Create StreamWriter, wrapping CryptoStream.
        StreamWriter sWriter = new StreamWriter(iStream);

        // Write to the innermost stream (which will encrypt).
        sWriter.Write(save);

        // Close StreamWriter.
        sWriter.Close();

        // Close CryptoStream.
        iStream.Close();

        // Close FileStream.
        dataStream.Close();
    }
}
