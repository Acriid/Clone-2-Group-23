using System.Collections;
using UnityEngine;

public class GunWeapon : Item
{
    private Coroutine _bulletPathRoutine = null;
    private Vector2 _mouseDirection = Vector2.zero;

    private float _itemCooldown = 0f;


    void Awake()
    {
        
        //starts off cooldown
        _itemCooldown = _itemSO.ItemCooldown + 1f;
    }
    public override void UseItem()
    {
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


    }

    private IEnumerator BulletPath()
    {
        while(true)
        {
            
        }
    }

    private void ShootBullet(Vector2 ShootDirection)
    {
        
    }
}
