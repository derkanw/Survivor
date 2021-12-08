using UnityEngine;
using System;

public class PauseModel : MonoBehaviour, IPauseModel
{
    public event Action Resume;
    public event Action SaveProgress;

    [SerializeField] private GameObject PauseUI;
    private GameObject _pauseUI;
    private IButtonModel _buttonModel;
    private int _order;

    public void ViewModel() => _pauseUI.SetActive(true);
    private void SaveParams() => SaveProgress?.Invoke();

    private void Start()
    {
        _order = 2;
        _pauseUI = Instantiate(PauseUI, Vector3.zero, Quaternion.identity);
        _pauseUI.GetComponent<Canvas>().sortingOrder = _order;
        _pauseUI.SetActive(false);
        _buttonModel = _pauseUI.GetComponent<IButtonModel>();
        _buttonModel.Resume += OnResume;
        _buttonModel.GoToMenu += SaveParams;
    }

    private void OnResume()
    {
        _pauseUI.SetActive(false);
        Resume?.Invoke();
    }
}
