using System;
using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public event Action<float, SkillsNames> ReloadSkill;
    public SkillsNames Name;
    [SerializeField] [Range(0, 5)] private int CountOfUses;
    [SerializeField] [Range(0f, 50f)] private float Value;
    [SerializeField] [Range(0f, 50f)] private float EffectTime;
    [SerializeField] private GameObject Effect;

    private bool _isReloading;
    private float _progress;

    public int Count
    {
        get => CountOfUses;
        set => CountOfUses = value;
    }

    public void UseSkill(Player player)
    {
        if (Count == 0 || _isReloading) return;
        Instantiate(Effect, transform.position, Quaternion.identity).transform.SetParent(player.gameObject.transform);
        switch(Name)
        {
            case SkillsNames.Health:
                    player.Heal(Value);
                    --Count;
                break;
            case SkillsNames.Power:
                    player.PowerUp(Value, EffectTime);
                    --Count;
                break;
            case SkillsNames.Speed:
                    player.SpeedUp((int)Value, EffectTime);
                    --Count;
                break;
            default:
                return;
        }
        if (EffectTime != 0f)
        {
            _isReloading = true;
            _progress = 0f;
        }
        else
            ReloadSkill?.Invoke(0, Name);
    }

    private void Update()
    {
        if (_isReloading)
        {
            _progress += Time.deltaTime / EffectTime;
            ReloadSkill?.Invoke(1 - _progress, Name);
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        while (_progress < 1f)
            yield return new WaitForEndOfFrame();
        _isReloading = false;
    }
}
