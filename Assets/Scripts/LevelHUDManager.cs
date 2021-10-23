using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHUDManager : MonoBehaviour
{
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image ManaBar;
    [SerializeField] private Image ReloadBar;
    [SerializeField] private Text BulletsCount;
    [SerializeField] private Text Points;
    [SerializeField] private Text Level;

    public void ChangeBulletBar(float count) => ProgressBar.fillAmount = count;

    public void ChangeHealthBar(float count) => HealthBar.fillAmount = count;

    public void ChangeReloadBar(float count)
    {
        if (count == 0)
            ReloadBar.fillAmount = count;
        else
            ReloadBar.fillAmount += count;
    }

    public void ChangeBulletsCount(string str) => BulletsCount.text = str;

    public void OnChangedPoints(int points) => Points.text = points.ToString();

    public void OnLevelUp(int level) => Level.text = level.ToString();
}
