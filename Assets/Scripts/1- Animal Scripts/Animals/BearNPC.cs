using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearNPC : AggressiveAnimalNPC
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
    #endregion

    [SerializeField] private GameObject player;
    float distance;

    bool isFollowingPlayer;

    public Animal animal;

    public GameObject playerState;

    private void Start()
    {
        agent.speed = speed;
        animal = GetComponent<Animal>();
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

        animator.SetFloat("health", animal.currentHealth);

        // Smooth rotation towards player if within 8 units
        if (distance <= 8 && animal.currentHealth>20)
        {
            SmoothRotateTowardsPlayer();
        }
    }

    public override void Attack()
    {
        if (distance <= 8)
            playerState.GetComponent<PlayerState>().currentHealth -= damage;
    }

    public override void Chase()
    {
        StartCoroutine(FollowPlayer());
    }

    IEnumerator FollowPlayer()
    {
        animator.SetBool("isFollowingPlayer", true);
        isFollowingPlayer = true;
        float stopDistance = 6.0f; // NPC'nin oyuncuya yaklaşacağı minimum mesafe

        while (isFollowingPlayer)
        {
            Vector3 playerPosition = FindPlayerPosition();

            if (playerPosition != Vector3.zero)
            {
                // NPC ve oyuncu arasındaki mesafeyi hesapla
                float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

                // Eğer mesafe belirlenen minimum mesafeden büyükse hedef pozisyonu belirle
                if (distanceToPlayer > stopDistance)
                {
                    agent.SetDestination(playerPosition);
                }
                else
                {
                    // NPC durduruluyor, çünkü yeterince yakın
                    agent.SetDestination(transform.position);
                }
            }

            if (!playerInDangerArea)
            {
                runCounter++;

                if (runCounter >= 10)
                    isFollowingPlayer = false;

                yield return new WaitForSeconds(0.2f);
            }

            if (playerInDangerArea)
            {
                runCounter = 0;
            }

            yield return new WaitForSeconds(0.2f);
        }
        ChangeFalseChaseState();
    }

    public override void Die()
    {
        animator.SetBool("isDead", true);
        HandleDeadState();
    }

    public override void Idle()
    {
        StartCoroutine(WaitToMove());
    }

    public override void Move()
    {
        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        agent.SetDestination(randomDestination);

        StartCoroutine(WaitToReachDestination());
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

    protected Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;
            NavMeshHit navMeshHit;

            if (NavMesh.SamplePosition(randomDirection, out navMeshHit, distance, NavMesh.AllAreas))
            {
                return navMeshHit.position;
            }
        }

        return origin;
    }

    //----------------------------------------------------
    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance && agent.isActiveAndEnabled)
        {
            if (Time.time - startTime >= maxWalkTime)
            {
                agent.ResetPath();
                yield break;
            }

            yield return null;
        }
        animator.SetBool("isMoving", false);
    }

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
        playerInDangerArea = true;
        isAnimalRunning = true;
    }

    public void ChangeFalseForRunningState()
    {
        playerInDangerArea = false;
        isAnimalRunning = false;
    }

    public void ChangeFalseChaseState()
    {
        playerInDangerArea = false;
        isFollowingPlayer = false;
        animator.SetBool("isFollowingPlayer", false);
        animator.SetBool("isVisible", false);
    }

    public void SmoothRotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
