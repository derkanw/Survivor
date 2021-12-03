using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillsNames Name;
    [SerializeField] [Range(0, 5)] private int CountOfUses;
    [SerializeField] [Range(0f, 50f)] private float Value;
    [SerializeField] [Range(0f, 50f)] private float EffectTime;
    [SerializeField] private GameObject Effect;

    public int Count
    {
        get => CountOfUses;
        set => CountOfUses = value;
    }

    public void UseSkill(Player player)
    {
        if (Count == 0) return;
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
    }
}
