using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !SelectionManager.Instance.handIsVisible && PlayerState.Instance.currentEnergy >= ChoppableTree.Instance.energySpentChoppingWood && !MenuManager.Instance.isMenuOpen)
        {
            animator.SetTrigger("hit");
        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree !=null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
    }
}
