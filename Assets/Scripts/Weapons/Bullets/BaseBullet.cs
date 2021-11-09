using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] [Range(0f, 50f)] protected float Power;
    public void SetPower(float incPower) => Power *= incPower;
    protected abstract void Move();
}
