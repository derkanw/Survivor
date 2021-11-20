using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button ExitButton;

    public void SetActive(bool value) => ContinueButton.gameObject.SetActive(value);
    private void NewGame()
    {
        SaveSystem.DeleteAll();
        SceneManager.LoadScene(Tokens.LevelName);
    }

    private void ContinueGame() => SceneManager.LoadScene(Tokens.LevelName);

    private void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    private void Awake()
    {
        ButtonManager.SetUpButton(NewGameButton, NewGame);
        ButtonManager.SetUpButton(ContinueButton, ContinueGame);
        ButtonManager.SetUpButton(ExitButton, ExitGame);
    }

    private void Start() => AudioManager.PlaySound(SoundNames.MainTheme, "PersistentSound", true);
}
