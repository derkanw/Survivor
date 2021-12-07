using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour, ILevelView
{
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image ReloadBar;
    [SerializeField] private Text BulletCount;
    [SerializeField] private Text Points;
    [SerializeField] private Text PlayerLevel;
    [SerializeField] private Text GameLevel;
    [SerializeField] private GameObject GunPoint;
    [SerializeField] private List<Image> GunIcons;
    [SerializeField] private GameObject SkillPoint;
    [SerializeField] private List<Image> SkillIcons;
    [SerializeField] private List<Text> SkillCount;

    private float _clipSize;
    private Vector3 _offset;

    public void ChangeBulletBar(float count) => ProgressBar.fillAmount = count;

    public void ChangeHealthBar(float count) => HealthBar.fillAmount = count;

    public void ChangeReloadBar(float count) => ReloadBar.fillAmount = count;

    public void ChangeSkillReloadingBar(int index, float count) => SkillIcons[index].fillAmount = count;

    public void OnChangedBulletCount(float count) => BulletCount.text = count + "\\" + _clipSize;

    public void OnChangedPoints(int points) => Points.text = points.ToString();

    public void OnPlayerLevelUp(int level) => PlayerLevel.text = level.ToString();

    public void OnGameLevelUp(int level) => GameLevel.text = "Level " + ++level + " progress";

    public void OnChangedClipSize(float size) => _clipSize = size;

    public void OnChangedGun(int index) => GunPoint.transform.position = GunIcons[index].transform.position + _offset;

    public void OnChangedSkill(int index)
    {
        if (SkillPoint.activeSelf == false)
            SkillPoint.SetActive(true);
        SkillPoint.transform.position = SkillIcons[index].transform.position + _offset;
    }

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

    private void Start()
    {
        _offset = new Vector3(-20f, -20f, 0);
        SkillPoint.SetActive(false);
        OnChangedGun(0);
        ViewGuns(0);
    }
}
