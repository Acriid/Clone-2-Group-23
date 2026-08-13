using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Detection")]
    public FieldOfView fov;
    //public float meleeRange = 1.5f;

    [Header("Long Range/Bullet Attack")]
    public Transform Aim;

    [Header("Calling Melee Weapon and Gun Weapon Scripts")]
    public GunWeapon longAttack;
    public MeleeWeapon shortAttack;

    void Update()
    {

        if (fov == null || fov.playerRef == null || !fov.canSeePlayer)
            return; // can't see the player, do nothing

        float distance = Vector2.Distance(transform.position, fov.playerRef.transform.position);

        if (distance <= shortAttack.GetMeleeRange())
        {
            //PerformMeleeAttack();
            //Call player melee swing/ attack animation here 
            shortAttack.UseItem(fov.playerRef);
        }
        else
        {
            Debug.Log("Shooting is called ");
           // PerformShootAttack();
           Vector2 directionToPlayer = (fov.playerRef.transform.position - Aim.position).normalized;
              // longAttack.ShootBulletEnemy(directionToPlayer);
              longAttack.UseItem(directionToPlayer);
            //  Debug.Log("Shooting is called ");
           
        }
    }
}