using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepNPC : NonAggressiveAnimalNPC
{
    [Header("Idle")]
    public float idleTime;

    [Header("Wander")]
    public float wanderDistance;
    public float maxWalkTime;

    #region RunStateComponents
    public bool playerInDangerArea;
    public int runCounter;
    public bool isAnimalRunning;
    public bool isAnimalDead;
    #endregion

    [SerializeField] private GameObject player;
    float distance;

    private void Start()
    {
        agent.speed = speed;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 15)
        {
            playerInDangerArea = true;
        }
        else
        {
            playerInDangerArea = false;
        }
    }

    public override void Die()
    {
        HandleDeadState();
    }

    public override void Idle()
    {
        StartCoroutine(WaitToMove());
    }

    public override void Move()
    {
        animator.SetBool("isMoving", true);
        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        agent.SetDestination(randomDestination);

        StartCoroutine(WaitToReachDestination());
    }

    protected Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
    {
        
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;
            NavMeshHit navMeshHit;

            // Check if the random direction hits a NavMesh area and is not hitting an obstacle
            if (NavMesh.SamplePosition(randomDirection, out navMeshHit, distance, NavMesh.AllAreas) && !IsPositionBlockedByObstacle(navMeshHit.position))
            {
                return navMeshHit.position;
            }
        }

        return origin;
    }

    private bool IsPositionBlockedByObstacle(Vector3 position)
    {
        // Cast a sphere to check for obstacles around the given position
        Collider[] hitColliders = Physics.OverlapSphere(position, 1.0f);

        foreach (Collider col in hitColliders)
        {
            // Check if the collider belongs to a NavMesh obstacle
            NavMeshObstacle obstacle = col.GetComponent<NavMeshObstacle>();
            if (obstacle != null)
            {
                return true; // Position is blocked by an obstacle
            }
        }

        return false; // Position is not blocked by any obstacle
    }

    private IEnumerator WaitToReachDestination()
    {
        float walkElapsedTime = 0;
        Vector3 firstPos = transform.position;
        Vector3 secondPos;
        float startTime = Time.time;

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance && agent.isActiveAndEnabled)
        {
            if (Time.time - startTime >= maxWalkTime)
            {
                agent.ResetPath();
                animator.SetBool("isMoving", false); // Ensure isMoving is reset
                yield break;
            }

            if ((walkElapsedTime += Time.deltaTime) > 1f)
            {
                walkElapsedTime = 0;
                secondPos = transform.position;

                if (secondPos == firstPos)
                {
                    //WAITING
                    agent.ResetPath();
                    animator.SetBool("isMoving", false); // Ensure isMoving is reset
                    yield break;
                }
            }

            yield return null;
        }
        animator.SetBool("isMoving", false);
    }


    public override void Run()
    {
        StartCoroutine(AnimalRunning());
    }


    //-------------------------------------------------------------------------------------------------------
    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isMoving", true);
    }



    //----------------------------------------------------


    //--------------------------------------------------------
    IEnumerator AnimalRunning()
    {
        ChangeTrueForRunningState();
        while (isAnimalRunning)
        {
            Vector3 playerPosition = FindPlayerPosition();
            if (playerPosition != Vector3.zero)
            {
                Vector3 fleeDirection = transform.position - playerPosition;
                Vector3 targetPosition = transform.position + fleeDirection;

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            if (!playerInDangerArea)
            {
                runCounter++;

                if (runCounter >= 30)
                    isAnimalRunning = false;

                yield return new WaitForSeconds(0.2f);
            }
            if (playerInDangerArea)
            {
                runCounter = 0;
            }
            yield return new WaitForSeconds(0.2f);
        }

        animator.SetBool("isRunning", false);
        ChangeFalseForRunningState();
    }

    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }

    //---------------------------------------------------------------------------
    public void HandleDeadState()
    {
        agent.speed = 0;
        agent.isStopped = true;
    }

    //--------------------------------------------------------------------------
    public void ChangeTrueForRunningState()
    {
        if (!isAnimalDead)
        {
            playerInDangerArea = true;
            isAnimalRunning = true;
        }
    }

    public void ChangeFalseForRunningState()
    {
        playerInDangerArea = false;
        isAnimalRunning = false;
    }
}