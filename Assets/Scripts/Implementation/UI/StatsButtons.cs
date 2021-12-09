using UnityEngine;
using UnityEngine.UI;

public class StatsButtons : MonoBehaviour
{
    [SerializeField] private Text Points;
    [SerializeField] private Text Count;
    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;

    private void Awake()
    {
        ButtonModel.SetUpButton(LeftButton, DownValue);
        ButtonModel.SetUpButton(RightButton, UpValue);
    }

    private void DownValue()
    {
        int count = int.Parse(Count.text);
        if (count >= 1)
        {
            Count.text = (--count).ToString();
            int points = int.Parse(Points.text);
            Points.text = (++points).ToString();
        }
    }

    private void UpValue()
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
