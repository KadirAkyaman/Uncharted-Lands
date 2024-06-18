using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : Sense
{
    [SerializeField] private float fieldOfView;
    [SerializeField] float maxCheckDistance;

    public override void InitializeSense()
    {
        fieldOfView = 90;
        maxCheckDistance = 40;
    }

    public override void UpdateSense()
    {
        Vector3 v1 = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, v1 * maxCheckDistance, Color.red);
        float angle = Vector3.Angle(v1, transform.forward);

        if (angle < fieldOfView / 2)
        {
            Debug.DrawRay(transform.position, v1 * maxCheckDistance, Color.gray);
            Ray ray = new Ray(transform.position, v1);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxCheckDistance))
            {
                Debug.DrawRay(transform.position, v1 * maxCheckDistance, Color.green);

                string name = hitInfo.transform.name;
                if (name.Equals("Player"))
                {   
                    GetComponentInChildren<Animator>().SetBool("isVisible",true);
                }
                else
                {
                    GetComponentInChildren<Animator>().SetBool("isVisible",false);
                }
            }
            else
            {
                GetComponentInChildren<Animator>().SetBool("isVisible",false);
            }
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("isVisible",false);
        }
    }
}
