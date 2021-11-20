using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static AudioList _list;
    private static readonly HashSet<SoundNames> PersistantSounds = new HashSet<SoundNames>();

    public static void PlaySound(SoundNames name, string objectName = "Sound", bool doNotDestroy = false)
    {
        if (PersistantSounds.Contains(name)) return;

        var soundObject = new GameObject(objectName);
        var audioSource = soundObject.AddComponent<AudioSource>();

        var sound = GetSound(name);
        if (sound != null && sound.Clip != null)
        {
            audioSource.clip = sound.Clip;
            audioSource.volume = sound.Volume;
            audioSource.loop = sound.Loop;
            audioSource.Play();
            if (doNotDestroy)
            {
                DontDestroyOnLoad(soundObject);
                PersistantSounds.Add(name);
            }
            else
                Destroy(soundObject, sound.Clip.length);
        }
        else
            Destroy(soundObject);
    }

    private static Sound GetSound(SoundNames name)
    {
        foreach (var sound in _list.Sounds)
            if (sound.Name == name)
                return sound;
        return null;
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
        DontDestroyOnLoad(gameObject);
        _list = gameObject.GetComponent<AudioList>().List;
    }
}
