using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using UnityEngine;

namespace SF.DataManagement
{
    public class SaveSystem : MonoBehaviour
    {
        private static List<SaveFileData> SaveFiles = new() { new SaveFileData() };
        public static SaveFileData CurrentSaveFileData { get; private set; }

        // This is the path when called from Unity editor.
        // C:\Users\jonat\AppData\LocalLow\Shatter Fantasy\Immortal Chronicles - The Realm of Imprisoned Sorrows\ICSaveData.txt
        private readonly static string SaveFileNameBase = Application.persistentDataPath + "/ICSaveData.txt";

        // The data stream of the contents being written and read from the save file.
        private static FileStream DataStream;

        // Key for reading and writing encrypted data.
        // (This is a "hardcoded" secret key. )
        private static byte[] SavedKey = { 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15 };

        public static void SaveDataStream()
        {
            // Create an AES instance
            // The i stands for input
            Aes iAes = Aes.Create();

            // Save the generated IV aka the initialization Vector = IV
            // This tells the AES where to start as it encrypts data.
            byte[] inputIV = iAes.IV;

            // Create a FileStream for writing data to.
            DataStream = new FileStream(SaveFileNameBase, FileMode.Create);

            // Just save the inputIV at the start of the fil before encrypting it.
            // It doesn't need to be private.
            DataStream.Write(inputIV,0,inputIV.Length);

            // Create a wrapper for the CryptoStream file to encrypt the FileStream
            CryptoStream cryptoStream = new CryptoStream(
                DataStream,
                iAes.CreateEncryptor(SavedKey, iAes.IV),
                CryptoStreamMode.Write
            );

            // Create StreamWriter, wrapping CryptoStream.
            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            // Serialize the SaveFileData object into JSON and save string.
            string jsonString = JsonUtility.ToJson(SaveFiles[0]);

            //Write to the innermost stream which is the encryption one.
            streamWriter.WriteLine(jsonString);

            //Close the streams in reverse order as they were made.
            streamWriter.Close();
            cryptoStream.Close();
            DataStream.Close();
        }

        public static void LoadDataFile()
        {
            if(!File.Exists(SaveFileNameBase))
                return;

            // Create new AES instance.
            // The o stands for output
            Aes oAes = Aes.Create();

            // Crete an array of correct size
            byte[] outputIV = new byte[oAes.IV.Length];


            // Create a FileStream
            DataStream = new FileStream(SaveFileNameBase, FileMode.Open);

            // Read the AES IV from the file
            DataStream.Read(outputIV, 0, outputIV.Length);

            // Create a wrapper for the CryptoStream file to decrypt the FileStream
            CryptoStream cryptoStream = new CryptoStream(
                DataStream,
                oAes.CreateDecryptor(SavedKey, outputIV),
                CryptoStreamMode.Read
            );

            // Create a StreamReader to wrap the cryptoStream
            StreamReader streamReader = new StreamReader(cryptoStream);

            // Read the entire file
            string text = streamReader.ReadToEnd();
            // Close the stream after done using it
            streamReader.Close();

            //Deserialize the data from here and load it into Unity Object.
            SaveFiles[0] = JsonUtility.FromJson<SaveFileData>(text);
            CurrentSaveFileData = SaveFiles[0];
        }
    }

    [System.Serializable]
    public class SaveFileData
    {
        public int HoursPlayed = 3;
    }
}
