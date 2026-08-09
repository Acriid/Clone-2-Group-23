using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class GunWeapon : Item
{
    [SerializeField] private int _bulletPoolSize = 3;
    [SerializeField] private GameObject _bullet = null;
    [SerializeField] private Transform _shotStartPosition = null;
    [SerializeField] private TimeManager _timeManager = null;

    private float _bulletTimeScale = 1f;

    private Coroutine _bulletPathRoutine = null;
    private Vector2 _mouseDirection = Vector2.zero;

    private float _itemCooldown = 0f;

    //Bullet Pool
    private GenericPool<Bullet> _bulletPool = null;
    private List<Bullet> _activeBullets;


    //Main camera Cache
    private Camera _mainCamera = null;
    private Pointer _mainPointer = null;

    private int _usageLeft;

    void Awake()
    {
        
        //starts off cooldown
        _itemCooldown = _itemSO.ItemCooldown + 1f;

        _usageLeft = _itemSO.ItemUsage;

        //Pool initialization
        _bulletPool = PoolManager.Instance.GetPool<Bullet>(_bullet,true, _bulletPoolSize);
        if(_bulletPool == null)
        {
            Debug.LogError("Failed to load bullet pool.");
        }
        _activeBullets = new(_bulletPoolSize);

        //Cache variables
        _mainCamera = Camera.main;

        _mainPointer = Pointer.current;


    }

    public override void UseItem()
    {
        //Quick return checks
        if(_usageLeft <= 0) return;
        if(_itemCooldown < _itemSO.ItemCooldown) return;

        //First use case.
        if(_bulletPathRoutine == null)
        {
            _bulletPathRoutine = StartCoroutine(BulletPath());
            return;
        }


        //No direction found
        if(_mouseDirection == Vector2.zero)
        {
            return;
        }

        ShootBullet(_mouseDirection);


        if(_bulletPathRoutine != null)
        {
            StopCoroutine(_bulletPathRoutine);
            _bulletPathRoutine = null;
        }
        if(_mouseDirection != Vector2.zero)
        {
            _mouseDirection = Vector2.zero;
        }

        StartCoroutine(CooldownClock());
    }

    private IEnumerator BulletPath()
    {
        while(true)
        {
            Vector2 origin = transform.position;
            Vector2 mousePosition = _mainPointer.position.ReadValue();

            //Change Mouse Position To World Position

            Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

            _mouseDirection = (mouseWorldPosition - origin).normalized;

            yield return null;
        }
    }

    private void ShootBullet(Vector2 shootDirection)
    {
        Bullet instance = _bulletPool.Get();

        _activeBullets.Add(instance);

        instance.OnBulletRemoved += ReturnBullet;
        instance.ShootBullet(_shotStartPosition.position,shootDirection);
        instance.ChangeBulletTime(_bulletTimeScale);
    }

    //Removes bullet after it has hit something or reached the end of its lifetime
    private void ReturnBullet(Bullet bulletToReturn)
    {
        //UnSubscribe event
        bulletToReturn.OnBulletRemoved -= ReturnBullet;
        
        _bulletPool.Return(bulletToReturn);
        _activeBullets.Remove(bulletToReturn);
        
    }

    private IEnumerator CooldownClock()
    {
        //Start item Cooldown
        _itemCooldown = 0f;
        while(_itemCooldown < _itemSO.ItemCooldown)
        {
            //TODO- Add variable to change cooldown time when in slipstream or shadow map.

            _itemCooldown += Time.deltaTime;
            yield return null;
        }
    }



    private void OnEnable()
    {
        _timeManager.OnBulletTimeChange += ChangeBulletTime;
    }

    private void OnDisable()
    {
        _timeManager.OnBulletTimeChange -= ChangeBulletTime;
    }

    public void ChangeBulletTime(float newTime)
    {
        _bulletTimeScale = newTime;
        foreach(Bullet bullet in _activeBullets)
        {
            bullet.ChangeBulletTime(newTime);
        }
    }
}
