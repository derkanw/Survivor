using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private string Name;
    [SerializeField] [Range(0f, 50f)] private float Count;
    [SerializeField] private GameObject Effect;

    public void UseSkill(Player player)
    {
        Instantiate(Effect, transform.position, Quaternion.identity).transform.SetParent(player.gameObject.transform);
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
        Destroy(gameObject);
    }
}
