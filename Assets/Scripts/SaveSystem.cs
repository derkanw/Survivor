using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/stats.info";
    private static BinaryFormatter formatter = new BinaryFormatter();

    public static void Save(Dictionary<StatsNames, int> stats)
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
    }
}
