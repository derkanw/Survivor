using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject ContinueButton;

    public void OnFailed() => ContinueButton.SetActive(false);

    public void SetActive() => ContinueButton.SetActive(true);

    private void Start() => AudioManager.PlaySound(SoundNames.MainTheme);

}
