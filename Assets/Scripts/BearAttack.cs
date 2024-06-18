using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttack : MonoBehaviour
{
    public void Attack()
    {
        GetComponentInParent<BearNPC>().Attack();
    }
}
