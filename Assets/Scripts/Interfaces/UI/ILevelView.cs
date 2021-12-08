using System;

public interface ILevelView
{
    public event Action SceneFinished;
    public void FadeOut();
    public void ChangeBulletBar(float count);
    public void ChangeHealthBar(float count);
    public void ChangeReloadBar(float count);
    public void ChangeSkillReloadingBar(int index, float count);
    public void OnChangedBulletCount(float count);
    public void OnChangedPoints(int points);
    public void OnPlayerLevelUp(int level);
    public void OnGameLevelUp(int level);
    public void OnChangedClipSize(float size);
    public void OnChangedGun(int index);
    public void OnChangedSkill(int index);
    public void ViewGuns(int count);
    public void ViewSkill(int index, int count);
}