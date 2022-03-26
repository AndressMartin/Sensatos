using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private const string SAVE_EXTENSION = "txt";

    public static void Init()
    {
        // Test if Save Folder exists
        if (!Directory.Exists(SAVE_FOLDER))
        {
            // Create Save Folder
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(object saveObject)
    {
        string jsonString = JsonUtility.ToJson(saveObject);
        // Make sure the Save Number is unique so it doesnt overwrite a previous save file
        int saveNumber = 1;
        while (File.Exists(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION))
        {
            saveNumber++;
        }
        // saveNumber is unique
        File.WriteAllText(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION, jsonString);
    }

    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        // Get all save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        // Cycle through all save files and identify the most recent one
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostRecentFile == null)
            {
                mostRecentFile = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;
                }
            }
        }

        // If theres a save file, load it, if not, order the game to save and then try loading again (could cause a stack overflow)
        if (mostRecentFile != null)
        {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        }
        else
        {
            Debug.LogError("Couldn't find a save file to load");
            return null;
        }
    }

    public static void DeleteSaves()
    {
        string[] filePaths = Directory.GetFiles(SAVE_FOLDER);
        foreach (string filePath in filePaths) File.Delete(filePath);
    }
}
