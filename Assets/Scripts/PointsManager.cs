using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatsNames
{
    Health, Mana, Rapidity, Agility, Power
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

    public Dictionary<StatsNames, int> UpdateStats()
    {
        int size = Values.Count;
        var _stats = new Dictionary<StatsNames, int>(size);
        for (int index = 0; index < size; ++index)
            _stats.Add((StatsNames)index, int.Parse(Values[index].text));
        return _stats;
    }
}
