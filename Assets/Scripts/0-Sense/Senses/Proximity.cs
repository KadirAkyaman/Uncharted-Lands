using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : Sense
{
    [SerializeField] private float proximityDistance;
    float delay;
    bool isInProximity;

    public bool isInAttackRange;

    float elapsedTime = 0;
    public Animator animator;
    public override void InitializeSense()
    {
        proximityDistance = 20;
        delay = 0;
        isInProximity = false;
    }

    public override void UpdateSense()
    {
        animator.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));                  //set animator distance
        Vector3 v1 = (player.transform.position - transform.position).normalized;
        if ((delay += Time.deltaTime) > 0.5f)
        {
            
            delay = 0;
            if ((Vector3.Distance(transform.position, player.transform.position) < proximityDistance) && isInProximity == false)
            {
                GetComponentInChildren<Animator>().SetBool("isInProximity", true);
                isInProximity = true;
                
            }
            else if (Vector3.Distance(transform.position, player.transform.position) >= proximityDistance)
            {
                GetComponentInChildren<Animator>().SetBool("isInProximity", false);
                isInProximity = false;
            }

            if (Vector3.Distance(transform.position, player.transform.position) < 8)
            {
                isInAttackRange = true;
                GetComponentInChildren<Animator>().SetBool("isInAttackRange", true);
                

            }
            else
            {
                isInAttackRange = false;
                GetComponentInChildren<Animator>().SetBool("isInAttackRange", false);
            }

        }
    }

}
