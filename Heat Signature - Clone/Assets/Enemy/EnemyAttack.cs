using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Public Variables
    public GameObject melee;
    public bool isAttacking = false;
    public float attackDuration = 0.3f;
    public float attackTimer = 0f;

    [Header("Detection")]
    public FieldOfView fov;
    //public float meleeRange = 1.5f;

    [Header("Long Range/Bullet Attack")]
    public Transform Aim;
    public GameObject bullet;
    public float fireForce = 10f;
    public float shootCooldown = 0.25f;
    private float shootTimer = 0f;

    [Header("Calling Melee Weapon and Gun Weapon Scripts")]
    public GunWeapon longAttack;
    public MeleeWeapon shortAttack;

    void Update()
    {
       CheckMeleeTimer();
        shootTimer += Time.deltaTime;

        if (fov == null || fov.playerRef == null || !fov.canSeePlayer)
            return; // can't see the player, do nothing

        float distance = Vector2.Distance(transform.position, fov.playerRef.transform.position);

        if (distance <= shortAttack.GetMeleeRange())
        {
            PerformMeleeAttack();
            //Call player melee swing/ attack animation here 
        }
        else
        {
           // PerformShootAttack();
           if (shootTimer >= shootCooldown)
           {
               shootTimer = 0f;
               Vector2 directionToPlayer = (fov.playerRef.transform.position - Aim.position).normalized;
               longAttack.ShootBulletEnemy(directionToPlayer);
           }
           
        }
    }

    void PerformMeleeAttack()
    {
        if (!isAttacking)
        {
            if (melee != null) melee.SetActive(true);
            isAttacking = true;
            Debug.Log("Enemy is attacking");
        }
    }

    void PerformShootAttack()
    {
        if (shootTimer >= shootCooldown)
        {
            shootTimer = 0f;

            Vector2 directionToPlayer = (fov.playerRef.transform.position - Aim.position).normalized;

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle - 90f);

            GameObject instantiatedBullet = Instantiate(bullet, Aim.position, bulletRotation);
            instantiatedBullet.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * fireForce, ForceMode2D.Impulse);
            Destroy(instantiatedBullet, 1f);

            Debug.Log("Enemy is shooting");
        }
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                isAttacking = false;
                if (melee != null) melee.SetActive(false);
            }
        }
    }
}