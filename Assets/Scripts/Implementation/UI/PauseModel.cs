using UnityEngine;
using System;

public class PauseModel : MonoBehaviour, IPauseModel
{
    public event Action Resume;
    public event Action LevelEnd;

    [SerializeField] private GameObject PauseUI;
    private GameObject _pauseUI;
    private IButtonModel _buttonModel;
    private int _order;

    public void ChangeScene() => _buttonModel.ChangeScene();

    public void ViewModel() => _pauseUI.SetActive(true);
    private void Notify()
    {
        LevelEnd?.Invoke();
        _pauseUI.SetActive(false);
    }

    private void Start()
    {
        _order = 2;
        _pauseUI = Instantiate(PauseUI, Vector3.zero, Quaternion.identity);
        _pauseUI.GetComponent<Canvas>().sortingOrder = _order;
        _pauseUI.SetActive(false);
        _buttonModel = _pauseUI.GetComponent<IButtonModel>();
        _buttonModel.Resume += OnResume;
        _buttonModel.GoToMenu += Notify;
    }

    private void OnResume()
    {
        _pauseUI.SetActive(false);
        Resume?.Invoke();
    }
}
