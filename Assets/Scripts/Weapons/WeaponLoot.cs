using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class WeaponLoot : MonoBehaviour
{
    public event Action GetNewWeapon;
    [SerializeField] [Range(0f, 1f)] private float Chance;
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Image Icon;

    private string _sceneName;
    private int _index;
    private int _count;

    public void SetArsenalSize(int size) => _count = size;

    public void InitUI(string sceneName)
    {
        print(_count);
        if (UnityEngine.Random.Range(0f, 1f) >= (1 - Chance) && _index <= _count - 1)
        {
            var ui = Instantiate(gameObject, Vector3.zero, Quaternion.identity).GetComponent<WeaponLoot>();
            ButtonManager.SetUpButton(ui.GetComponent<WeaponLoot>().AcceptButton, LoadNextLevel);
            //Icon.sprite = null;
            _sceneName = sceneName;
        }
        else
            SceneManager.LoadScene(sceneName);
    }

    private void LoadNextLevel()
    {
        ++_index;
        GetNewWeapon?.Invoke();
        SceneManager.LoadScene(_sceneName);
    }
}
