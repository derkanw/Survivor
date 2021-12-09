using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillService : MonoBehaviour, ISkillService
{
    public event Action<int, float> ChangedSkillReload;
    public event Action<int, int> ChangedSkillCount;
    public event Action<int> GetSkillCount;

    [SerializeField] private GameObject[] SkillObjects;

    private int _skillCount;
    private int _currentSkill;
    private Player _player;
    private List<Skill> _skills;

    public void SaveParams()
    {
        int[] data = new int[_skillCount];
        for (int index = 0; index < _skillCount; ++index)
            data[index] = _skills[index].Count;
        SaveSystem.Save<int[]>(Tokens.SkillCount, data);
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
        for (int i = 0; i < _skillCount; ++i)
        {
            if (_skills[i] == null)
                continue;
            if (i == index)
                _currentSkill = index;
        }
    }

    private int GetIndex(SkillsNames name)
    {
        for (int index = 0; index < _skillCount; ++index)
            if (_skills[index].Name == name)
                return index;
        return -1;
    }

    private void Start()
    {
        _skillCount = SkillObjects.Length;
        GetSkillCount?.Invoke(_skillCount);
        _skills = new List<Skill>(_skillCount);
        _player = gameObject.GetComponent<Player>();
        
        var data = SaveSystem.IsExists(Tokens.SkillCount) ? SaveSystem.Load<int[]>(Tokens.SkillCount) : new int[_skillCount];


        for (int index = 0; index < _skillCount; ++index)
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
