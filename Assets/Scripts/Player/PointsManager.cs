using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatsNames
{
    Health, Rapidity, Agility, Power
}

public class PointsManager : MonoBehaviour
{
    [SerializeField] private Text PointsField;
    [SerializeField] private List<Text> Values;

    public int Points
    {
        get => int.Parse(PointsField.text);
        set => PointsField.text = value.ToString();
    }

    public Dictionary<StatsNames, int> GetStats()
    {
        int size = Values.Count;
        var _stats = new Dictionary<StatsNames, int>(size);
        for (int index = 0; index < size; ++index)
            _stats.Add((StatsNames)index, int.Parse(Values[index].text));
        return _stats;
    }

    public void SetStats(Dictionary<StatsNames, int> stats)
    {
        int size = Values.Count;
        if (stats == null || stats.Count != size)
            for (int index = 0; index < size; ++index)
                Values[index].text = "0";
        else
            for (int index = 0; index < size; ++index)
                Values[index].text = stats[(StatsNames)index].ToString();
    }
}
