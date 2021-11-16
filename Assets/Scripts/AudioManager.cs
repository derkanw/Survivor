using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject _soundObject;
    private static AudioSource _audioSource;
    private static AudioManager _instance;
    private static AudioList _list;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        _list = gameObject.GetComponent<AudioList>().List;
    }
    public static void PlaySound(SoundNames name)
    {
        if (_soundObject == null)
        {
            _soundObject = new GameObject("Sound");
            _audioSource = _soundObject.AddComponent<AudioSource>();
        }
        AudioClip clip = GetClip(name);
        if (clip == null) return;
        // TODO: music restarts
        _audioSource.volume = 0.2f;
        if (name == SoundNames.MainTheme)
        {
            _audioSource.volume = 1f;
            _audioSource.loop = true;
        }
        _audioSource.PlayOneShot(clip);
    }
    
    private static AudioClip GetClip(SoundNames name)
    {
        foreach (Sound sound in _list.Sounds)
            if (sound.Name == name)
                return sound.Clip;
        return null;
    }
}
