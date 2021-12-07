using UnityEngine;
using System;

public class PauseModel : MonoBehaviour, IPauseModel
{
    public event Action Resume;
    public event Action SaveProgress;

    [SerializeField] private GameObject PauseUI;
    private GameObject _pauseUI;
    private IButtonModel _buttonModel;

    public void OnPause() => _pauseUI.SetActive(true);
    public void OnGoToMenu() => SaveProgress?.Invoke();

    private void Start()
    {
        _pauseUI = Instantiate(PauseUI, Vector3.zero, Quaternion.identity);
        _pauseUI.SetActive(false);
        _buttonModel = _pauseUI.GetComponent<IButtonModel>();
        _buttonModel.Resume += OnResume;
        _buttonModel.GoToMenu += OnGoToMenu;
    }

    private void OnResume()
    {
        _pauseUI.SetActive(false);
        Resume?.Invoke();
    }
}
