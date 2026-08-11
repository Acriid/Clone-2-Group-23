using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //Public Variables 
    public float radius;

    [Range(0, 360)]
    //Makes Range a slider to adjust
    public float angle;

    
    [Header("Reference to Player/ Object to Search for")] 
    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        //Need to tag actual/ final player 
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            //Permanently search for player 
            yield return wait;
            //Search
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
       
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
       
        //Put player only on this targetMask
        //Make sure player has box or valid 2D Collider on, otherwise wont work 
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            float eulerZ = transform.eulerAngles.z;
            Vector2 forward = new Vector2(Mathf.Cos(eulerZ * Mathf.Deg2Rad), Mathf.Sin(eulerZ * Mathf.Deg2Rad));

            if (Vector2.Angle(forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);

                if (hit.collider == null)
                {
                    //Nothing blocking the view between enemy and player
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }
}