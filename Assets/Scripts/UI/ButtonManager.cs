using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class ButtonManager : MonoBehaviour
{
    public event Action Pause;
    public event Action Resume;
    public event Action LooksStats;
    public event Action GoToMenu;
    public event Action Restart;

    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Restart?.Invoke();
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        GoToMenu?.Invoke();
        SceneManager.LoadScene(Tokens.MainMenu);
    }

    public void OnExitButton()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(Tokens.LevelName);
    }

    public void OnResumeButton()
    {
        Time.timeScale = 1f;
        Resume?.Invoke();
    }

    public void OnPauseButton()
    {
        Time.timeScale = 0f;
        Pause?.Invoke();
    }

    public void OnStatsButton()
    {
        Time.timeScale = 0f;
        LooksStats?.Invoke();
    }

    public void OnResetButton()
    {
        SaveSystem.DeleteAll();
        SceneManager.LoadScene(Tokens.LevelName);
    }
}
