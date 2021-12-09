using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuModel : MonoBehaviour, IMenuModel
{
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button ExitButton;

    public void SetContinueAbility(bool value) => ContinueButton.gameObject.SetActive(value);
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
        ButtonModel.SetUpButton(NewGameButton, NewGame);
        ButtonModel.SetUpButton(ContinueButton, ContinueGame);
        ButtonModel.SetUpButton(ExitButton, ExitGame);
    }

    private void Start() => AudioManager.PlaySound(SoundNames.MainTheme, "PersistentSound", true);
}
