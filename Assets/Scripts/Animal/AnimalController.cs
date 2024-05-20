using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
    protected AnimalState currentState = AnimalState.Idle;

    //ANIMATION
    public Animator animator;


    private void Start()
    {
        InitialiseAnimal();


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
                HandleIdleState();

                break;

            case AnimalState.Wander:
                animator.SetBool("isRunning", true);
                HandleMovingState();
                break;
        }
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

    protected virtual void HandleIdleState()
    {
        StartCoroutine(WaitToMove());
    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);

        yield return new WaitForSeconds(waitTime);

        Vector3 randomDestination = GetRandomNavMeshPosition(transform.position, wanderDistance);

        navMeshAgent.SetDestination(randomDestination);
        SetState(AnimalState.Wander);
    }

    protected virtual void HandleMovingState()
    {
        StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;

        while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && navMeshAgent.isActiveAndEnabled)
        {
            if (Time.time - startTime >= maxWalkTime)
            {
                navMeshAgent.ResetPath();
                SetState(AnimalState.Idle);
                yield break;
            }

            //CheckChaseConditions();

            yield return null;
        }

        // Destination has been reached
        SetState(AnimalState.Idle);
    }

    #endregion

    protected void SetState(AnimalState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(AnimalState newState)
    {
        UpdateState();
    }
}

public enum AnimalState
{
    Idle, // Hayvan�n hareketsiz oldu�u durum
    Wander, // Hayvan�n hareket etti�i durum
    Eating, // Hayvan�n yemek yedi�i durum
    Running, // Tehlike durumunda ka�t��� durum
    Attacking, // Sald�r� durumunda oldu�u durum
    Resting // Hayvan�n dinlendi�i durum
}
