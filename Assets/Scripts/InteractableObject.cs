using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    public bool PlayerInRange;
    public string GetItemName()
    {
        return ItemName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.SelectedObject == gameObject) // eger e tusuna basilirsa objeyi envantere ekle
        {

            if (!InventorySystem.Instance.isFull)
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Inventory FULL");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}
