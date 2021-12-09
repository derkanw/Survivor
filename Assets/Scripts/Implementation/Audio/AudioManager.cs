using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static IAudioList _list;
    private static readonly HashSet<SoundNames> _persistantSounds = new HashSet<SoundNames>();
    private static Dictionary<SoundNames, float> _soundsTimer;

    public static void PlaySound(SoundNames name, string objectName = "Sound", bool doNotDestroy = false)
    {
        if (_persistantSounds.Contains(name)) return;

        var soundObject = new GameObject(objectName);
        var audioSource = soundObject.AddComponent<AudioSource>();

        var sound = GetSound(name);
        if (sound != null && sound.Clip != null)
        {
            audioSource.clip = sound.Clip;
            audioSource.volume = sound.Volume;
            audioSource.loop = sound.Loop;
            if (sound.AllowedTime > 0f && !_soundsTimer.ContainsKey(name))
                _soundsTimer.Add(sound.Name,  0f);
            if (CanPlaySound(sound.Name, sound.AllowedTime))
                audioSource.Play();
            if (doNotDestroy)
            {
                DontDestroyOnLoad(soundObject);
                _persistantSounds.Add(name);
            }
            else
                Destroy(soundObject, sound.Clip.length);
        }
        else
            Destroy(soundObject);
    }

    private static bool CanPlaySound(SoundNames name, float allowedTime)
    {
        if (_soundsTimer.ContainsKey(name))
        {
            float lastTime = _soundsTimer[name];
            if (lastTime + allowedTime >= Time.time)
                return false;
            _soundsTimer[name] = Time.time;
        }
        return true;
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
        _list = gameObject.GetComponent<IAudioList>().List;
        _soundsTimer = new Dictionary<SoundNames, float>();
    }
}
