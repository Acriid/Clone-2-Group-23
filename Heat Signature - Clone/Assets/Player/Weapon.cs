using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Public Variables 
    public int damage = 1;

    //On trigger function that attacks enemies when weapon collides 
    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyMovement enemy = col.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.Knockback(transform.position, 5f);
            Debug.Log("enemy knocked ");
        }
        
        
    }
}
