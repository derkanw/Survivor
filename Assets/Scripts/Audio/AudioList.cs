using UnityEngine;

public class AudioList : MonoBehaviour
{
    public Sound[] Sounds;
    private static AudioList _instance;

    public AudioList List
    {
        get => _instance;
        private set => _instance = value;
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
