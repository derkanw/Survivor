using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image ReloadBar;
    [SerializeField] private Text BulletsCount;

    public void ChangeBulletBar(float count)
    {
        ProgressBar.fillAmount = count;
    }

    public void ChangeHealthBar(float count)
    {
        HealthBar.fillAmount = count;
    }

    public void ChangeReloadBar(float count)
    {
        if (count == 0)
            ReloadBar.fillAmount = count;
        else
            ReloadBar.fillAmount += count;
    }

    public void ChangeBulletsCount(string str)
    {
        BulletsCount.text = str;
    }
}
