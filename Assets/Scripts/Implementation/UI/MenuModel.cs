using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class MenuModel : MonoBehaviour, IMenuModel
{
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Animator FadedPanel;

    private string _sceneName;

    public void SetContinueAbility(bool value) => ContinueButton.gameObject.SetActive(value);

    private void NewGame()
    {
        SaveSystem.DeleteAll();
        _sceneName = Tokens.LevelName;
        FadedPanel.SetTrigger("FadeOut");
    }

    private void ContinueGame()
    {
        _sceneName = Tokens.LevelName;
        FadedPanel.SetTrigger("FadeOut");
    }

        private void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    private void EndScene()
    {
        if (String.IsNullOrWhiteSpace(_sceneName)) return;
        SceneManager.LoadScene(_sceneName);
    }


    private void Awake()
    {
        ButtonModel.SetUpButton(NewGameButton, NewGame);
        ButtonModel.SetUpButton(ContinueButton, ContinueGame);
        ButtonModel.SetUpButton(ExitButton, ExitGame);
    }

    private void Start() => AudioManager.PlaySound(SoundNames.MainTheme, "PersistentSound", true);
}
