using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Private Variables 
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    
    //Public Variables 
    public float movementSpeed = 4f;
    public GameObject player;

    // Health System
    public int lives = 10;
    public GameObject[] heartSprites;

    // Melee Attack System --> Will change to Range/Gun Attack 
    public Transform aim;
    public GameObject melee;
    private bool isAttacking = false;
    public float attackDuration = 0.3f;
    public float attackTimer = 0f;
    
    //KnockBack System 
    private bool isKnockedBack = false;
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.2f;
    
    //Player Temporary Invincibility
    private bool invincible = false;
    public float invincibilityTime = 0.5f;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        melee.SetActive(false);
    
    }

    
    void Update()
    {
        // Rotate the aim object to one of 8 directions
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            // Snap to nearest 45 degrees
            angle = Mathf.Round(angle / 45f) * 45f;

            // If your sword points UP by default, change this to angle + 90
            aim.rotation = Quaternion.Euler(0f, 0f, angle+90f);
        }
        CheckMeleeTimer();
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            return;
        }
        //Moves Player using RigidBody
        rb.linearVelocity = moveInput * movementSpeed;
    }

    //Function that moves Player 
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        bool isWalking = moveInput != Vector2.zero;
        animator.SetBool("isWalking", isWalking);

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        if (isWalking)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }
    
    //Player Attack Function
    public void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack test");
        if (!context.performed)
            return;

        
        OnAttack();
    }
   
    //Call Animator to play melee attack, will change to gun attack 
    void OnAttack()
    {
        if (!isAttacking)
        {
          
            melee.SetActive(true);
            isAttacking = true;
            //Call animator to play melee attack here 
            
            
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
                melee.SetActive(false);
            }
        }
    }

    //Function that determines how player character takes damage 
    public void TakeDamage(int damage)
    {
        
        if (lives <= 0 || invincible)
        {
            return;

        }

        StartCoroutine(Invincibility());

        lives -= damage;
        heartSprites[lives].SetActive(false);

        if (lives == 0)
        {
            Debug.Log("Player died");
            //Switch scene here to death screen 
            player.SetActive(false);
            Application.Quit();
            
        }
    }
    
    //Player becomes temporarily invincible to prevent consecutive attacks 
    IEnumerator Invincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(invincibilityTime);

        invincible = false;
    }
    
    //Player is knocked back when attacked by enemies
    public void Knockback(Vector2 attackerPosition, float force)
    {
        StartCoroutine(KnockbackRoutine(attackerPosition, force));
    }

    IEnumerator KnockbackRoutine(Vector2 attackerPosition, float force)
    {
        isKnockedBack = true;

        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;

        rb.linearVelocity = direction * force;

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
    
    

   //How player can restore lives--> Not used yet 
    public void RestoreLives()
    {
        if (lives < 10)
        {
            lives = 10;

            foreach (GameObject heart in heartSprites)
            {
                heart.SetActive(true);
            }
        }
    }
}