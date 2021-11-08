using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    /*public static string path = Application.persistentDataPath + "/stats.info";
    private static readonly BinaryFormatter formatter = new BinaryFormatter();*/

    public static void Save<T>(string key, T data)
    {
        if (data == null) return;
        string json;
        if (typeof(T).IsPrimitive)
            json = data.ToString();
        else
            json = JsonUtility.ToJson(data);
        if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(key)) return;
        PlayerPrefs.SetString(key, json);
    }

    public static T Load<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return default;
        var json = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(json)) return default;
        if (typeof(T).IsPrimitive)
            return default;
        return JsonUtility.FromJson<T>(json);
    }

    public static void DeleteAll() => PlayerPrefs.DeleteAll();

    /*public static void Save(Dictionary<StatsNames, int> stats)
    {
        FileStream stream = new FileStream(path, FileMode.Create);

        int size = stats.Count;
        int[] data = new int[size];
        for (int index = 0; index < size; ++index)
            data[index] = stats[(StatsNames)index];

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static int[] Load()
    {
        int[] data = null;
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as int[];
            stream.Close();
        }
        return data;
    }*/
}
