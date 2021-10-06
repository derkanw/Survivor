using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGun : MonoBehaviour
{
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] [Range(0f, 50f)] private float ReloadTime;
    [SerializeField] [Range(0f, 20f)] private float ShootingSpeed;
    [SerializeField][Range(0f, 100f)] private float ClipSize;

    [SerializeField] private Transform BulletsInfo; // change to event system

    private bool _isReloading;
    private bool _isShooting;
    private float _bulletsCount;
    private Image _bulletIcon;
    private Text _bulletText;

    protected abstract void InitBullet();

    private IEnumerator Shooting()
    {
        _isShooting = true;
        while (_bulletsCount != 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            InitBullet();
            --_bulletsCount;
            yield return new WaitForSeconds(ShootingSpeed);
        }
        _isReloading = false;
    }

    private IEnumerator Reloading()
    {
        _isReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        _bulletsCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
    }

    private void Start()
    {
        _bulletText = BulletsInfo.GetChild(0).GetComponent<Text>();
        _bulletIcon = BulletsInfo.GetChild(1).GetComponent<Image>();

        _bulletsCount = ClipSize;
        _isReloading = false;
        _isShooting = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isShooting = true;
            StartCoroutine(Reloading());
            _bulletIcon.fillAmount = 0.0f;
        }
        if (_bulletIcon.fillAmount < 1.0f)
            _bulletIcon.fillAmount += Time.deltaTime / ReloadTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_isShooting)
        {
            StartCoroutine(Shooting());
            _isShooting = false;
        }
        _bulletText.text = _bulletsCount + "\\" + ClipSize;
    }
}
