using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [Range(0f, 50f)] public float Speed, Power;
    [HideInInspector] public Vector3 direction;

    protected abstract void Flying();

    public void FixedUpdate()
    {
        Flying();
    }
}
