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
    [SerializeField] private GameObject WeaponsPoint;
    [SerializeField] private List<Image> WeaponsIcons;
    [SerializeField] private GameObject SkillsPoint;
    [SerializeField] private List<Image> SkillsIcons;

    private float _clipSize;
    private Vector3 _offset;

    public void ChangeBulletBar(float count) => ProgressBar.fillAmount = count;

    public void ChangeHealthBar(float count) => HealthBar.fillAmount = count;

    public void ChangeManaBar(float count) => ManaBar.fillAmount = count;

    public void ChangeReloadBar(float count) => ReloadBar.fillAmount = count;

    public void OnChangedBulletsCount(float count) => BulletsCount.text = count + "\\" + _clipSize;

    public void OnChangedPoints(int points) => Points.text = points.ToString();

    public void OnPlayerLevelUp(int level) => PlayerLevel.text = level.ToString();

    public void OnGameLevelUp(int level) => GameLevel.text = "Level " + ++level + " progress";

    public void OnChangedClipSize(float size) => _clipSize = size;

    public void OnChangedWeapon(int index) => WeaponsPoint.transform.position = WeaponsIcons[index].transform.position + _offset;

    public void OnChangedSkill(int index)
    {
        if (SkillsPoint.activeSelf == false)
            SkillsPoint.SetActive(true);
        SkillsPoint.transform.position = SkillsIcons[index].transform.position + _offset;
    }

    public void ViewSkill(int index, bool value) => SkillsIcons[index].enabled = value;

    private void Start()
    {
        _offset = new Vector3(-20f, -20f, 0);
        SkillsPoint.SetActive(false);
        OnChangedWeapon(0);
    }
}