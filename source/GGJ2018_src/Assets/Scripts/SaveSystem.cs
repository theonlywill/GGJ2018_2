using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveSystem
{

    public static PlayerSaveData currentData;

    static string saveFileName = "savedata.sav";

    public static void SetNumStars(int levelNum, int stars)
    {
        if(currentData == null)
        {
            //currentData

            //todo stuff
            return;
        }
        for(int i = 0; i < currentData.levelSaveData.Count; i++)
        {
            if(currentData.levelSaveData[i].level == levelNum)
            {
                currentData.levelSaveData[i].stars = stars;
                return;
            }
        }

        // it doesn't exist, create it
        PlayerSaveData.LevelSaveData newdata = new PlayerSaveData.LevelSaveData();
        newdata.level = levelNum;
        newdata.stars = stars;
        currentData.levelSaveData.Add(newdata);
    }

    public static void Save()
    {
        // Serialize to JSON and encrypt
        string jsonString = JsonUtility.ToJson(currentData);
        string encrpyted = Encryption.Encrypt(jsonString);

        // Write the save file
        File.WriteAllText(Application.persistentDataPath + saveFileName, encrpyted);
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + saveFileName))
        {
            // Read, decrypt, and load the save file
            string encrpyted = File.ReadAllText(Application.persistentDataPath + saveFileName);
            string decrpyted = Encryption.Decrypt(encrpyted);
            currentData = JsonUtility.FromJson<PlayerSaveData>(decrpyted);

        }
        else
        {
            // Create the save if there is none
            Save();
        }
    }
}

    
