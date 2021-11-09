using Newtonsoft.Json;
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
        var json = JsonConvert.SerializeObject(data);
        if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(key)) return;
        PlayerPrefs.SetString(key, json);
    }

    public static T Load<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || !IsExists(key)) return default;
        var json = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(json)) return default;
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static void DeleteAll() => PlayerPrefs.DeleteAll();

    public static void Delete(string key) => PlayerPrefs.DeleteKey(key);

    public static bool IsExists(string key) => PlayerPrefs.HasKey(key);
}
