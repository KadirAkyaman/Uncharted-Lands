using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    public bool swingWait = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !SelectionManager.Instance.handIsVisible && PlayerState.Instance.currentEnergy >= ChoppableTree.Instance.energySpentChoppingWood && !MenuManager.Instance.isMenuOpen && !ConstructionManager.Instance.inConstructionMode && swingWait == false)
        {
            swingWait = true;
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
            StartCoroutine(NewSwingDelay());
            
        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree !=null)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(1f);
        swingWait = false;
    }
}
