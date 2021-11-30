using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public event Action GoToMenu;
    public event Action Resume;
    public event Action Pause;
    public event Action LooksStats;
    public event Action Restart;

    [SerializeField] private Button PauseButton;
    [SerializeField] private Button StatsButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button ResumeButton;

    private bool _canUse;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Restart?.Invoke();
    }

    private void ToMainMenu()
    {
        Time.timeScale = 1f;
        GoToMenu?.Invoke();
        SceneManager.LoadScene(Tokens.MainMenu);
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
