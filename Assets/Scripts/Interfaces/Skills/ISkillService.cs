using System;

public interface ISkillService
{
    public event Action<int, float> ChangedSkillReload;
    public event Action<int, int> ChangedSkillCount;
    public event Action<int> GetSkillCount;

    public void SaveParams();
    public void TopUpSkill(SkillsNames name);
    public void UseSkill(bool value);
    public void SetSkill(int index);
}