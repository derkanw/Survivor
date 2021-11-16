using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillsManager : MonoBehaviour
{
    public event Action<int, bool> SkillExists;
    public event Action<int> GetSkillsCount;
    [SerializeField] private GameObject[] SkillObjects;

    private int _skillsCount;
    private int _currentSkill;
    private Player _player;
    private List<Skill> _skills;

    public void UseSkill(bool value)
    {
        if (!value) return;
        _skills[_currentSkill].UseSkill(_player);
        SkillExists?.Invoke(_currentSkill, false);
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
            {
                _skills[i].gameObject.SetActive(true);
                _currentSkill = index;
            }
            else
                _skills[i].gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        _skillsCount = SkillObjects.Length;
        GetSkillsCount?.Invoke(_skillsCount);
        _skills = new List<Skill>(_skillsCount);
        _player = gameObject.GetComponent<Player>();

        for (int index = 0; index < _skillsCount; ++index)
        {
            GameObject obj = Instantiate(SkillObjects[index]);
            obj.transform.parent = _player.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
            _skills.Add(obj.GetComponent<Skill>());
            SkillExists?.Invoke(index, true);
        }
    }
}
