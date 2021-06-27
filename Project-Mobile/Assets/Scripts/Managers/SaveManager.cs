using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string fileName = "PlayerData.json";
    private static PlayerData playerData;

    public static PlayerData PlayerData { get => playerData; set => playerData = value; }

    // Convert data to JSON, then save it.
    public static void Save()
    {
        string json = JsonUtility.ToJson(playerData);
        WriteToFile(fileName, json);
    }

    public static void Save(PlayerData newData)
    {
        playerData = newData;
        string json = JsonUtility.ToJson(playerData);
        WriteToFile(fileName, json);
    }

    /// Load Saved Data, create new Data if none is found.
    public static void Load()
    {
        PlayerData data;

        // Try to get Data from persistentDataPath...
        try
        {
            string path = Application.persistentDataPath + "/" + fileName;
            string _json = File.ReadAllText(path);
            data = JsonUtility.FromJson<PlayerData>(_json);
        }
        // ...if no File is found, create a new one.
        catch (FileNotFoundException)
        {
            data = new PlayerData();
        }

        playerData = data;

        if(playerData == null)
        {
            playerData = new PlayerData();
            Debug.LogWarning("Save file corrupted, new file created.");
        }
    }

    public static void ResetData()
    {
        playerData = new PlayerData();
    }

    private static void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private static string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else Debug.Log("File not found!");
        return "";
    }

    private static string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
