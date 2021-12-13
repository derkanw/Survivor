using System;
using System.Collections;
using UnityEngine;

public class GameOverModel : MonoBehaviour, IGameOverModel
{
    public event Action LevelEnd;

    private IButtonModel _buttonModel;
    private int _order;

    public void ChangeScene() => _buttonModel.ChangeScene();

    public void Activate() => gameObject.SetActive(true);

    private void Awake()
    {
        _order = 2;
        gameObject.GetComponent<Canvas>().sortingOrder = _order;
        _buttonModel = gameObject.GetComponent<IButtonModel>();
        gameObject.SetActive(false);
        _buttonModel.GoToMenu += Notify;
        _buttonModel.Restart += Notify;
    }

    private void Notify()
    {
        LevelEnd?.Invoke();
        gameObject.SetActive(false);
    }
}
