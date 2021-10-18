using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseUIManager : MonoBehaviour
{
    public event Action Resume;
    [SerializeField] private GameObject PauseUI;

    private GameObject _pauseUI;

    public void OnPause() => _pauseUI.SetActive(true);

    private void Start()
    {
        _pauseUI = Instantiate(PauseUI, Vector3.zero, Quaternion.identity);
        _pauseUI.SetActive(false);
        _pauseUI.GetComponent<ButtonManager>().Resume += OnResume;
    }

    private void OnResume()
    {
        _pauseUI.SetActive(false);
        Resume?.Invoke();
    }
}
