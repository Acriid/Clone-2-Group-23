using System.Collections;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletSO _bulletSO = null;
    [SerializeField] private Rigidbody2D _bulletRigidBody = null;
    private WaitForSeconds _bulletDespawnWaitTime = new(5f);

    private Coroutine _despawnRoutine;

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
        if(_bulletRigidBody == null) return;

        gameObject.transform.position = startPosition;
        gameObject.transform.right = shootDirection;

        _bulletRigidBody.AddForce(shootDirection * _bulletSO.BulletSpeed,ForceMode2D.Impulse);


        _despawnRoutine ??= StartCoroutine(DespawnBullet());
    }


    private IEnumerator DespawnBullet()
    {
        yield return _bulletDespawnWaitTime;
        _despawnRoutine = null;
        OnBulletRemoved?.Invoke(this);
    }
}
