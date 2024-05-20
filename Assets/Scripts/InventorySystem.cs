using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;


    public List<GameObject> SlotList = new List<GameObject>();

    public List<string> ItemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject nextSlotToEquip;

    public bool isOpen;

    public bool isFull;


    //PickUp PopUp
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

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

        //Cursor.visible = false;
    }



    private void FillSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("InventorySlot"))
            {
                SlotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            OpenUI();
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            CloseUI();
        }
    }

    public void OpenUI()
    {
        inventoryScreenUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        isOpen = true;
    }

    public void CloseUI()
    {
        inventoryScreenUI.SetActive(false);
        
        if (!CraftingSystem.Instance.isOpen && !CampfireUIManager.Instance.isUIOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }

        isOpen = false;
    }

    public void AddToInventory(string itemName)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.pickUpSound);

        nextSlotToEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), nextSlotToEquip.transform.position, nextSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(nextSlotToEquip.transform);

        ItemList.Add(itemName);

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);


        ReSizeList();
        CraftingSystem.Instance.RefreshInventory();

        

    }

    private void TriggerPickupPopUp(string itemName, Sprite itemSprite)                                             //POPUP ALERT
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
        StartCoroutine(PopUpAnim(pickupAlert));
    }

    IEnumerator PopUpAnim(GameObject alert)
    {
        yield return new WaitForSeconds(1f);
        alert.SetActive(false);
    }


    public void RemoveItem(string nameOfTheObjectToBeDeleted, int amountOfObjectToDelete)
    {
        int counter = amountOfObjectToDelete;

        for (var i = SlotList.Count - 1; i >= 0; i--)
        {
            if (SlotList[i].transform.childCount > 0)
            {
                if (SlotList[i].transform.GetChild(0).name == nameOfTheObjectToBeDeleted + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(SlotList[i].transform.GetChild(0).gameObject);

                    counter--;
                }
            }
        }

        ReSizeList();
        CraftingSystem.Instance.RefreshInventory();
    }

    public void ReSizeList()
    {
        ItemList.Clear();

        foreach (GameObject slot in SlotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string stringName = name;
                string stringTag = "(Clone)";
                string resultName = name.Replace(stringTag, "");
                ItemList.Add(resultName);
            }
        }
    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in SlotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in SlotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emptySlot++;
            }
        }
        Debug.Log(emptySlot);

        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}