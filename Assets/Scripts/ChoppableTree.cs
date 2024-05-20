using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public static ChoppableTree Instance;
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator animator;

    public float energySpentChoppingWood = 10;

    private void Awake() {
    Instance = this;
    }

    private void Start()
    {
        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void GetHit()
    {
        animator.SetTrigger("shake");
        treeHealth -= 1;

        PlayerState.Instance.currentEnergy -= energySpentChoppingWood;
        if (treeHealth<=0)
        {
            TreeWasCutDown();
        }
    }


    private void TreeWasCutDown()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallingSound);
        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),new Vector3(treePosition.x,treePosition.y+0.3f,treePosition.z), Quaternion.Euler(0,0,0));
    }

    private void Update()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.resourcesHealth = treeHealth;
            GlobalState.Instance.resourcesMaxHealth = treeMaxHealth;
        }
    }
}
