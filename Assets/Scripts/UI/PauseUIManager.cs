using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseUIManager : MonoBehaviour
{
    public event Action Resume;
    public event Action SaveProgress;

    [SerializeField] private GameObject PauseUI;
    private GameObject _pauseUI;
    private ButtonManager _manager;

    public void OnPause() => _pauseUI.SetActive(true);
    public void OnGoToMenu() => SaveProgress?.Invoke();

    private void Start()
    {
        _pauseUI = Instantiate(PauseUI, Vector3.zero, Quaternion.identity);
        _pauseUI.SetActive(false);
        _manager = _pauseUI.GetComponent<ButtonManager>();
        _manager.Resume += OnResume;
        _manager.GoToMenu += OnGoToMenu;
    }

    private void OnResume()
    {
        _pauseUI.SetActive(false);
        Resume?.Invoke();
    }

    private void OnDestroy()
    {
        _manager.Resume -= OnResume;
        _manager.GoToMenu -= OnGoToMenu;
    }
}
