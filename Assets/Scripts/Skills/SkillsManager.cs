using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillsManager : MonoBehaviour
{
    public event Action<int, float> ChangedSkillReload;
    public event Action<int, int> ChangedSkillCount;
    public event Action<int> GetSkillsCount;
    [SerializeField] private GameObject[] SkillObjects;

    private int _skillsCount;
    private int _currentSkill;
    private Player _player;
    private List<Skill> _skills;

    public void SaveParams()
    {
        int[] data = new int[_skillsCount];
        for (int index = 0; index < _skillsCount; ++index)
            data[index] = _skills[index].Count;
        SaveSystem.Save<int[]>(Tokens.SkillsCount, data);
    }

    public void TopUpSkill(SkillsNames name)
    {
        var index = GetIndex(name);
        if (index != -1)
            ChangedSkillCount?.Invoke(index, ++_skills[index].Count);
    }

    public void UseSkill(bool value)
    {
        if (!value) return;
        _skills[_currentSkill].UseSkill(_player);
    }

    public void SetSkill(int index)
    {
        if (_skills[index] == null)
            return;
        AudioManager.PlaySound(SoundNames.Equip);
        for (int i = 0; i < _skillsCount; ++i)
        {
            if (_skills[i] == null)
                continue;
            if (i == index)
                _currentSkill = index;
        }
    }

    private int GetIndex(SkillsNames name)
    {
        for (int index = 0; index < _skillsCount; ++index)
            if (_skills[index].Name == name)
                return index;
        return -1;
    }

    private void Start()
    {
        _skillsCount = SkillObjects.Length;
        GetSkillsCount?.Invoke(_skillsCount);
        _skills = new List<Skill>(_skillsCount);
        _player = gameObject.GetComponent<Player>();
        
        var data = SaveSystem.IsExists(Tokens.SkillsCount) ? SaveSystem.Load<int[]>(Tokens.SkillsCount) : new int[_skillsCount];


        for (int index = 0; index < _skillsCount; ++index)
        {
            GameObject obj = Instantiate(SkillObjects[index]);
            obj.transform.parent = _player.transform;
            obj.transform.localPosition = Vector3.zero;
            _skills.Add(obj.GetComponent<Skill>());
            _skills[index].Count = data[index];
            ChangedSkillCount?.Invoke(index, _skills[index].Count);
            _skills[index].ReloadSkill += SkillReloading;
        }
    }

    private void SkillReloading(float count, SkillsNames name)
    {
        var index = GetIndex(name);
        if (index == -1) return;
        ChangedSkillReload?.Invoke(index, count);
        if (count <= 0f)
        {
            ChangedSkillReload?.Invoke(index, 1);
            ChangedSkillCount?.Invoke(index, _skills[index].Count);
        }
    }
}
