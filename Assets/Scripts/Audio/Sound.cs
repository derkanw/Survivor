using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundNames Name;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
    [Range(0f, 2f)] public float AllowedTime;
    public bool Loop;
}
