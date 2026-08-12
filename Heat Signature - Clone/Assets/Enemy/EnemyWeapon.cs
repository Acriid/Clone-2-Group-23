using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    //Public Variables
    public float damage = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Enemy enemy = col.GetComponent<Enemy>();
        // if (enemy != null)
        // {
        //     enemy.TakeDamage(damage);
        // }
        
        //Need to call function for player to take damage here 
    }
}
