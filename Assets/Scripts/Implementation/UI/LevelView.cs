using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelView : MonoBehaviour, ILevelView
{
    public event Action SceneFinished;

    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image ReloadBar;
    [SerializeField] private Text BulletCount;
    [SerializeField] private Text Points;
    [SerializeField] private Text PlayerLevel;
    [SerializeField] private Text GameLevel;
    [SerializeField] private List<Image> GunPoints;
    [SerializeField] private List<Image> GunIcons;
    [SerializeField] private List<Image> SkillPoints;
    [SerializeField] private List<Image> SkillIcons;
    [SerializeField] private List<Text> SkillCount;
    [SerializeField] private Animator FadedPanel;

    private float _clipSize;

    public void FadeOut() => FadedPanel.SetTrigger("FadeOut");

    public void ChangeBulletBar(float count) => ProgressBar.fillAmount = count;

    public void ChangeHealthBar(float count) => HealthBar.fillAmount = count;

    public void ChangeReloadBar(float count) => ReloadBar.fillAmount = count;

    public void ChangeSkillReloadingBar(int index, float count) => SkillIcons[index].fillAmount = count;

    public void OnChangedBulletCount(float count) => BulletCount.text = count + "\\" + _clipSize;

    public void OnChangedPoints(int points) => Points.text = points.ToString();

    public void OnPlayerLevelUp(int level) => PlayerLevel.text = level.ToString();

    public void OnGameLevelUp(int level) => GameLevel.text = "Level " + ++level + " progress";

    public void OnChangedClipSize(float size) => _clipSize = size;

    public void OnChangedGun(int index) => ViewCornes(GunPoints, index);

    public void OnChangedSkill(int index) => ViewCornes(SkillPoints, index);

    public void ViewGuns(int count)
    {
        for (int index = 0; index < GunIcons.Count; ++index)
            GunIcons[index].enabled = index < count ? true : false;
    }

    public void ViewSkill(int index, int count)
    {
        if (count <= 1)
            SkillCount[index].enabled = false;
        else
            SkillCount[index].enabled = true;
        if (count == 0)
            SkillIcons[index].enabled = false;
        else
        {
            SkillIcons[index].enabled = true;
            SkillCount[index].text = count.ToString();
        }
    }

    private void EndScene() => SceneFinished?.Invoke();

    private void ViewCornes(List<Image> corners, int index = -1)
    {
        for (int i = 0; i < corners.Count; ++i)
            corners[i].enabled = i == index;
    }

    private void Start()
    {
        OnChangedGun(0);
        ViewCornes(SkillPoints);
        ViewCornes(GunPoints, 0);
        ViewGuns(0);
    }
}
