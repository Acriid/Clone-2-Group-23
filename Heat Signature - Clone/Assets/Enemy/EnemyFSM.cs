using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }

    [Header("References")]
    public Enemy patrolScript;   // your existing patrol script, left as-is
    public FieldOfView fov;              // your existing FOV script, left as-is
    public MonoBehaviour attackBehaviour; // drag your future attack script here (must implement IAttacker)

    [Header("Chase/Attack Settings")]
    public float attackRange = 1.5f;
    public float chaseSpeed = 4f;

    private NavMeshAgent agent;
    //private IAttacker attacker;
    private EnemyState currentState = EnemyState.Patrol;
    private float originalSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        //attacker = attackBehaviour as IAttacker;

        // if (attackBehaviour != null && attacker == null)
        // {
        //     Debug.LogWarning("attackBehaviour does not implement IAttacker!");
        // }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                if (fov.canSeePlayer) TransitionTo(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                agent.SetDestination(fov.playerRef.transform.position);

                if (!fov.canSeePlayer)
                {
                    TransitionTo(EnemyState.Patrol);
                }
                else if (Vector2.Distance(transform.position, fov.playerRef.transform.position) <= attackRange)
                {
                    TransitionTo(EnemyState.Attack);
                }
                break;

            case EnemyState.Attack:
                if (!fov.canSeePlayer)
                {
                    TransitionTo(EnemyState.Patrol);
                }
                else if (Vector2.Distance(transform.position, fov.playerRef.transform.position) > attackRange)
                {
                    TransitionTo(EnemyState.Chase);
                }
                break;
        }
    }

    private void TransitionTo(EnemyState newState)
    {
        // Exit old state
        // if (currentState == EnemyState.Attack)
        // {
        //     attacker?.StopAttacking();
        // }

        // Enter new state
        switch (newState)
        {
            case EnemyState.Patrol:
                agent.speed = originalSpeed;
                if (patrolScript != null) patrolScript.enabled = true;
                break;

            case EnemyState.Chase:
                if (patrolScript != null) patrolScript.enabled = false; // stop it fighting for control of the agent
                agent.speed = chaseSpeed;
                break;

            case EnemyState.Attack:
                agent.SetDestination(transform.position); // stop moving
                //attacker?.StartAttacking(fov.playerRef.transform);
                break;
        }

        currentState = newState;
    }
}
