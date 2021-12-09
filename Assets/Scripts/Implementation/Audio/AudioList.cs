using UnityEngine;

public class AudioList : MonoBehaviour, IAudioList
{
    [SerializeField] private Sound[] SoundsList;
    private static IAudioList _instance;

    public Sound[] Sounds { get => SoundsList; }

    public IAudioList List
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
