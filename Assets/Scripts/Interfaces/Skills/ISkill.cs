using System;

public interface ISkill
{
    public event Action<float, SkillsNames> ReloadSkill;
    public int Count { get; set; }

    public void UseSkill(IPlayer player);
}