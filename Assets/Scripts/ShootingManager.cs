using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    private List<BaseGun> _guns;

    void Start()
    {
        _guns = new List<BaseGun>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown("F"))
            _guns.Add();*/
    }
}
