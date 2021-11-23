using UnityEngine;

public class SkillObject : MonoBehaviour
{
    [SerializeField] private SkillsNames Name;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<SkillsManager>().TopUpSkill(Name);
            Destroy(gameObject);
        }
    }
}
