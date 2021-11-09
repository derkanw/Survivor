using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{
    [SerializeField] private string Name;
    [SerializeField] [Range(0f, 50f)] private float Count;
    [SerializeField] private GameObject Effect;

    public void UseSkill(Player player)
    {
        Instantiate(Effect, player.gameObject.transform.position, Quaternion.identity); //player position
        switch(Name)
        {
            case "Health":
                player.Heal(Count);
                break;
            case "Mana":
                player.TopUpMana(Count);
                break;
            default:
                return;
        }
        //Destroy(gameObject);
    }
}
