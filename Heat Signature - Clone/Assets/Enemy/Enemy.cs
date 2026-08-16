using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // This script deals with patrolling movement and health

    // Public Variables
    public Transform[] patrolPoints;
    public float health = 1f;
    public float maxHealth = 1f;

    // Private Variables
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private Rigidbody2D rb;


    public TimeManager TimeManager;
    private float _timeVariable = 1f;

    [Header("Weapon Functionality")]
    public Transform aim;
    public bool isWalking = false;

    [Header("Facing correct direction")]
    public float rotationSpeed = 10f; // how quickly it turns to face movement, 0 = instant

    [Header("Making Enemy Idle")]
    public bool isEnemyIdle;

    [Header("Detection")]
    public FieldOfView fov;
    public float chaseSpeed = 4f;
    private float defaultSpeed;

    public event Action<Enemy> OnPatrolEnd;
    public event Action<Enemy> OnEnemyKilled;

    private bool _isPatroling = false;
    private bool _isChasing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if(TimeManager != null)
        {
            TimeManager.OnEnemyTimeChange += OnTimeChange;
        }
    }
    void OnDisable()
    {
        if(TimeManager != null)
        {
            TimeManager.OnEnemyTimeChange -= OnTimeChange;
        }        
    }
    void Start()
    {
        // Setting up agent
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        defaultSpeed = agent.speed;

        health = maxHealth;

        // Null Check
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.Log("Assign patrol points!");
        }
        else if (!isEnemyIdle)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        HandleChaseBehavior();
        if (isEnemyIdle && !_isChasing)
        {
            HandleIdleBehaviour();
        }
        else if(!isEnemyIdle && !_isChasing)
        {
            HandlePatrolBehaviour();
        }

        // Determine if actually moving based on agent's velocity
        isWalking = agent.velocity.sqrMagnitude > 0.01f;

        if (isWalking)
        {
            FaceMovementDirection();
        }
    }

    private void HandleIdleBehaviour()
    {
        // Stay idle, don't move
        ChangeAgentSpeed(0f);
        agent.SetDestination(transform.position);

    }

    private void HandleChaseBehavior()
    {
        if (fov != null && fov.canSeePlayer)
        {
            // Player spotted — chase them
            ChangeAgentSpeed(chaseSpeed);
            agent.SetDestination(fov.playerRef.transform.position);
            _isChasing = true;
        }
        else
        {
            _isChasing = false;
        }
    }

    private void HandlePatrolBehaviour()
    {
        ChangeAgentSpeed(defaultSpeed);



        if (patrolPoints.Length > 0 && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            if(currentPointIndex == 0 && !_isPatroling)
            {
                OnPatrolEnd?.Invoke(this);
                SetIsIdle(true);
                return;
            }



            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;

            if(currentPointIndex == patrolPoints.Count() -1)
            {
                _isPatroling = false;
            }

            agent.SetDestination(patrolPoints[currentPointIndex].position);

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
            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void OnTimeChange(float newValue)
    {
        _timeVariable = newValue;

        ChangeAgentSpeed(defaultSpeed);
    }

    private void ChangeAgentSpeed(float newSpeed)
    {
        agent.speed = newSpeed * _timeVariable;
    }

    public void SetPatrolPoints(Transform[] newPoints)
    {
        Array.Copy(newPoints,patrolPoints,newPoints.Length);
    }
    public void SetIsIdle(bool newValue)
    {
        isEnemyIdle = newValue;
    }
    public void Patrol()
    {
        _isPatroling = true;
        SetIsIdle(false);
    }
}