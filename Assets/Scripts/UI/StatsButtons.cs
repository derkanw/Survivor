using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsButtons : MonoBehaviour
{
    [SerializeField] private Text Points;
    [SerializeField] private Text Count;

    public void OnLeftButton()
    {
        int count = int.Parse(Count.text);
        if (count >= 1)
        {
            Count.text = (--count).ToString();
            int points = int.Parse(Points.text);
            Points.text = (++points).ToString();
        }
    }

    public void OnRightButton()
    {
        int points = int.Parse(Points.text);
        if (points >= 1)
        {
            Points.text = (--points).ToString();
            int count = int.Parse(Count.text);
            Count.text = (++count).ToString();
        }
    }
}
