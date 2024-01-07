using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    

    public List<GameObject> SlotList = new List<GameObject>();

    public List<string> ItemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject nextSlotToEquip;

    public bool isOpen;

    public bool isFull;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        isFull = false;
        FillSlotList();
    }



    private void FillSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("InventorySlot"))
            {
                SlotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {

        if (IsInventoryFull())
        {
            isFull = true;
        }
        else
        {
            nextSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName),nextSlotToEquip.transform.position,nextSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(nextSlotToEquip.transform);

            ItemList.Add(itemName);
        }

    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in SlotList)
        {
            if (slot.transform.childCount==0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    private bool IsInventoryFull()
    {
        int counter = 0;

        foreach (GameObject slot in SlotList)
        {
            if (slot.transform.childCount>0)
            {
                counter++;
            }
        }
        if (counter == 21)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}