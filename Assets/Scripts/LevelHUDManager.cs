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
    [SerializeField] private Text PlayerLevel;
    [SerializeField] private Text GameLevel;

    private float _clipSize;

    public void ChangeBulletBar(float count) => ProgressBar.fillAmount = count;

    public void ChangeHealthBar(float count) => HealthBar.fillAmount = count;

    public void ChangeReloadBar(float count)
    {
        if (count == 0)
            ReloadBar.fillAmount = count;
        else
            ReloadBar.fillAmount += count;
    }

    public void OnChangedBulletsCount(float count) => BulletsCount.text = count + "\\" + _clipSize;

    public void OnChangedPoints(int points) => Points.text = points.ToString();

    public void OnPlayerLevelUp(int level) => PlayerLevel.text = level.ToString();

    public void OnGameLevelUp(int level) => GameLevel.text = "Level " + ++level + " progress";

    public void OnChangedClipSize(float size) => _clipSize = size;
}
