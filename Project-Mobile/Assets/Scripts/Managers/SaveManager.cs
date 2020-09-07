using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public PlayerData playerData;
    public string file = "PlayerData.json";

    private new void Awake()
    {
        base.Awake();
    }

    public void SaveCurrentData()
    {
        //playerData.playerName = playerName;
        playerData.playerCurrency = CurrencyManager.Instance.currency;
        playerData.playerShips = ShipsManager.Instance.listShipInfos;
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(playerData);
        WriteToFile(file, json);
    }

    /// Load Saved Data, create new Data if none is found.
    public void Load()
    {
        PlayerData data;

        // Try to get Data from persistentDataPath.
        try
        {
            string path = Application.persistentDataPath + "/" + file;
            string _json = File.ReadAllText(path);
            data = JsonUtility.FromJson<PlayerData>(_json);
        }
        // If no File is found, create a new one.
        catch (System.IO.FileNotFoundException)
        {
            data = new PlayerData { };
        }

        playerData = data;
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
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

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }


}
