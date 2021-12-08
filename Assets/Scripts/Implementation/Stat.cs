using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] [Range(0f, 100f)] private float BaseValue;
    [SerializeField] [Range(-1f, 1f)] private float Rate;
    private float _value;

    public float Value => _value;

    public void Init() => _value = BaseValue;

    public void Modify(int level) => _value = BaseValue * (1 + Rate * level);
}
