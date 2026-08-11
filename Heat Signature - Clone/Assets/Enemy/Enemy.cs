using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // This script deals with patrolling movement and health

    // Public Variables
    public Transform[] patrolPoints;
    public float health = 3f;
    public float maxHealth = 3f;

    // Private Variables
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private Rigidbody2D rb;

    // Weapon functionality
    public Transform aim;
    public bool isWalking = false;

    [Header("Facing")]
    public float rotationSpeed = 10f; // how quickly it turns to face movement, 0 = instant

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Setting up agent
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        health = maxHealth;

        // Null Check
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.Log("Assign patrol points!");
        }
        else
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        // Sets enemy to walk along patrol points
        if (patrolPoints.Length > 0 && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }

        // Determine if actually moving based on agent's velocity
        isWalking = agent.velocity.sqrMagnitude > 0.01f;

        if (isWalking)
        {
            FaceMovementDirection();
        }
    }

    private void FaceMovementDirection()
    {
        Vector2 moveDir = agent.velocity;
        float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        // Rotate the enemy itself, so FieldOfView (which reads this transform) faces the same way
        transform.rotation = rotationSpeed > 0f
            ? Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * 100f * Time.deltaTime)
            : targetRotation;

        // Keep aim in sync with the same facing direction
        if (aim != null)
        {
            aim.rotation = transform.rotation;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}