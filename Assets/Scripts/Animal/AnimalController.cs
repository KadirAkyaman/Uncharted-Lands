using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalController : MonoBehaviour
{
    [Header("Wander")]
    public float wanderDistance;
    public float walkSpeed;
    public float maxWalkTime;

    [Header("Idle")]
    public float idleTime;

    protected NavMeshAgent navMeshAgent;
    public AnimalState currentState = AnimalState.Idle;

    // ANİMASYON
    public Animator animator;

    public bool isAnimalDead;
    public bool isAnimalRunning;

    public bool isDamaged;

    public bool playerInDangerArea;

    private int runCounter;

    private void Start()
    {
        InitialiseAnimal();
        isAnimalDead = false;
        isAnimalRunning = false;
        playerInDangerArea = false;
        isDamaged = false;
    }

    protected virtual void InitialiseAnimal()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;

        currentState = AnimalState.Idle;
        UpdateState();
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                animator.SetBool("isRunning", false);
                if (!isAnimalDead && !isAnimalRunning)
                    StartCoroutine(WaitToMove());
                break;

            case AnimalState.Wander:
                animator.SetBool("isRunning", true);
                if (!isAnimalDead && !isAnimalRunning)
                    StartCoroutine(WaitToReachDestination());
                break;

            case AnimalState.Running:
                animator.SetBool("isRunning", true);
                if (isAnimalRunning)
                    StartCoroutine(AnimalRunning());
                break;
        }
    }

    public void HandleDeadState()
    {
        navMeshAgent.isStopped = true;
        currentState = AnimalState.Dead;
    }

    public void HandleRunningState()
    {
        currentState = AnimalState.Running;
        StartCoroutine(AnimalRunning());
    }

    IEnumerator AnimalRunning()
    {
        while (playerInDangerArea && isAnimalRunning)
        {
            Vector3 playerPosition = FindPlayerPosition();
            if (playerPosition != Vector3.zero)
            {
                Vector3 fleeDirection = transform.position - playerPosition;
                Vector3 targetPosition = transform.position + fleeDirection;

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hit.position);
                }
            }
            runCounter++;

            if (runCounter >= 30)
                isAnimalRunning = false;

            yield return new WaitForSeconds(0.2f);
        }
        isDamaged = false;
        playerInDangerArea = false;

        currentState = AnimalState.Wander;
        UpdateState();
    }

    private void Update()
    {
        if (isAnimalRunning || currentState == AnimalState.Running)
        {
            animator.SetBool("isRunning", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDangerArea = true;
        }

        if (other.CompareTag("Player") && !isAnimalDead && isDamaged)
        {
            isAnimalRunning = true;
            playerInDangerArea = true;
            runCounter = 0;
        }
    }

    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
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

    #region HandleState

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);
        navMeshAgent.SetDestination(randomDestination);
        currentState = AnimalState.Wander;
        UpdateState();
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;

        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && navMeshAgent.isActiveAndEnabled)
        {
            if (Time.time - startTime >= maxWalkTime)
            {
                navMeshAgent.ResetPath();
                currentState = AnimalState.Idle;
                UpdateState();
                yield break;
            }

            yield return null;
        }

        currentState = AnimalState.Idle;
        UpdateState();
    }

    #endregion
}

public enum AnimalState
{
    Idle, // Hayvanın hareketsiz olduğu durum
    Wander, // Hayvanın hareket ettiği durum
    Eating, // Hayvanın yemek yediği durum
    Running, // Tehlike durumunda kaçtığı durum
    Attacking, // Saldırı durumunda olduğu durum
    Resting, // Hayvanın dinlendiği durum
    Dead // Hayvanın öldüğü durum
}