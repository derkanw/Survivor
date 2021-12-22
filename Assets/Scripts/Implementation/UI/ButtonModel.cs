using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonModel : MonoBehaviour, IButtonModel
{
    public event Action Restart;
    public event Action GoToMenu;
    public event Action Resume;
    public event Action Pause;
    public event Action LooksStats;

    [SerializeField] private Button PauseButton;
    [SerializeField] private Button StatsButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button ResumeButton;

    private bool _canUse;
    private string _sceneName;

    public void ChangeScene()
    {
        if (String.IsNullOrWhiteSpace(_sceneName)) return;
        SceneManager.LoadScene(_sceneName);
    }

    public void DisableButtons() => _canUse = false;

    public static void SetUpButton(Button button, UnityAction function)
    {
        if (button != null && function != null)
        {
            button.onClick.AddListener(function);
            SetUpHover(button.gameObject.GetComponent<EventTrigger>());
            SetUpClick(button.gameObject.GetComponent<EventTrigger>());
        }
    }

    private static void SetUpHover(EventTrigger trigger)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener(data => { AudioManager.PlaySound(SoundNames.ButtonHover); });
        trigger.triggers.Add(entry);
    }

    private static void SetUpClick(EventTrigger trigger)
    {
        var click = new EventTrigger.Entry();
        click.eventID = EventTriggerType.PointerDown;
        click.callback.AddListener(data => { AudioManager.PlaySound(SoundNames.ButtonClick); });
        trigger.triggers.Add(click);
    }

    private void Awake()
    {
        _canUse = true;
        SetUpButton(PauseButton, PauseLevel);
        SetUpButton(StatsButton, ViewStats);
        SetUpButton(RestartButton, RestartLevel);
        SetUpButton(ResumeButton, ResumeLevel);
        SetUpButton(MenuButton, ToMainMenu);
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        Restart?.Invoke();
        _sceneName = SceneManager.GetActiveScene().name;
    }

    private void ToMainMenu()
    {
        Time.timeScale = 1f;
        GoToMenu?.Invoke();
        _sceneName = Tokens.MainMenu;
    }

    private void ResumeLevel()
    {
        Time.timeScale = 1f;
        Resume?.Invoke();
    }

    private void PauseLevel()
    {
        if (!_canUse) return;
        Time.timeScale = 0f;
        Pause?.Invoke();
    }

    private void ViewStats()
    {
        if (!_canUse) return;
        Time.timeScale = 0f;
        LooksStats?.Invoke();
    }
}
