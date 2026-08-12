using System.Collections;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletSO _bulletSO = null;
    [SerializeField] private Rigidbody2D _bulletRigidBody = null;


    private Coroutine _despawnRoutine;


    private float _bulletTimeScale = 1f;
    private Vector2 _bulletVelocity;

 
    public event Action<Bullet> OnBulletRemoved;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(_bulletSO.PlayerBullet)
        {
            if(collision.collider.CompareTag("Enemy"))
            {
                //Todo kill enemy
            }   
        }
        else
        {
            if (collision.collider.CompareTag("Player"))
            {
                //Todo kill player
            }
        }

        //Can Add a window check if needed. To break windows.

    }   

    public void ShootBullet(Vector2 startPosition, Vector2 shootDirection)
    {
        if (_bulletRigidBody == null) return;

        gameObject.transform.position = startPosition;
        gameObject.transform.right = shootDirection;

        _bulletVelocity = shootDirection * _bulletSO.BulletSpeed;
        _bulletRigidBody.linearVelocity = _bulletVelocity * _bulletTimeScale;

        _despawnRoutine ??= StartCoroutine(DespawnBullet());
    }

    public void ChangeBulletTime(float newTime)
    {
        _bulletTimeScale = newTime;
        _bulletRigidBody.linearVelocity = _bulletVelocity * _bulletTimeScale;
    }
    
    private IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(_bulletSO.BulletLifeTime / _bulletTimeScale);
        _despawnRoutine = null;
        OnBulletRemoved?.Invoke(this);
    }
}
