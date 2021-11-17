using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public void OnButtonHover() => AudioManager.PlaySound(SoundNames.ButtonHover);
    public void OnButtonClick() => AudioManager.PlaySound(SoundNames.ButtonClick);
}
