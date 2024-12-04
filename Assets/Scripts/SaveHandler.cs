using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveData
{
    public string curMap;

    public SaveData(string curMap)
    {
        this.curMap = curMap;
    }
}

public class SaveHandler : MonoBehaviour
{
    public enum SaveType { PlayerPrefs, Binary, JSON }

    public static SaveType type = SaveType.PlayerPrefs;

    static string binarySavePath = Application.persistentDataPath + "/Save.dat";
    static string jsonSavePath = Application.persistentDataPath + "/Save.json";

    static bool savePresent = false;

    public static bool SavePresent => savePresent;

    public static void WipeSaveData()
    {
        PlayerPrefs.DeleteAll();
        if (File.Exists(binarySavePath)) File.Delete(binarySavePath);
        if (File.Exists(jsonSavePath)) File.Delete(jsonSavePath);
    }

    public static void Save(SaveData data)
    {
        switch (type)
        {
            case SaveType.PlayerPrefs:
                {
                    PlayerPrefs.SetString("currentMap", data.curMap);
                    break;
                }
            case SaveType.Binary:
                {

                    BinaryFormatter bf = new BinaryFormatter();

                    FileStream fs = File.Create(binarySavePath);

                    bf.Serialize(fs, data);

                    fs.Close();
                    break;
                }
            case SaveType.JSON:
                {

                    string json = JsonUtility.ToJson(data);

                    File.WriteAllText(json, jsonSavePath);
                    break;
                }
        }
    }

    public static SaveData Load()
    {
        SaveData data = new(null);

        switch (type)
        {
            case SaveType.PlayerPrefs:
                {
                    if (PlayerPrefs.HasKey("currentMap"))
                    {
                        data.curMap = PlayerPrefs.GetString("currentLevel");
                        savePresent = true;
                    }
                    break;
                }
            case SaveType.Binary:
                {
                    if (File.Exists(binarySavePath))
                    {
                        savePresent = true;

                        BinaryFormatter bf = new BinaryFormatter();

                        FileStream fs = File.Open(binarySavePath, FileMode.Open);

                        data = (SaveData)bf.Deserialize(fs);

                        fs.Close();
                    }
                    break;
                }
            case SaveType.JSON:
                {
                    if (File.Exists(jsonSavePath))
                    {
                        savePresent = true;

                        string json = File.ReadAllText(jsonSavePath);

                        data = JsonUtility.FromJson<SaveData>(json);
                    }
                    break;
                }
        }
        return data;
    }
}
