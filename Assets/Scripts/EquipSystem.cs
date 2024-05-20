using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;


    //ToolHolder
    public GameObject toolHolder;

    public GameObject selectedItemModel;

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


    private void Start()
    {
        PopulateSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }

    }


    void SelectQuickSlot(int number)
    {
        if (CheckIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;


                if (selectedItem != null)//gecmis secili objesi deselect yapmak
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }


                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                //Change Color

                foreach (Transform child in numbersHolder.transform)
                {
                    child.GetComponentInChildren<Text>().color = Color.gray;
                }


                Text toBeChanged = numbersHolder.transform.Find("number" + number).GetComponentInChildren<Text>();
                toBeChanged.color = Color.white;
            }
            else//ayni slotu secmeye calisirsak
            {
                selectedNumber = -1;//null 

                if (selectedItem != null)//gecmis secili objesi deselect yapmak
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                //Change Color

                foreach (Transform child in numbersHolder.transform)
                {
                    child.GetComponentInChildren<Text>().color = Color.gray;
                }
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(1.15f, 0.6f, 1.98f), Quaternion.Euler(-6f, -178.078f, 15.98f));

        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    bool CheckIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        InventorySystem.Instance.ReSizeList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem!=null)
        {
            if (selectedItem.GetComponent<Weapon>()!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem!=null) //eger esya tutuyorsak
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else
        {
            return 0;
        }
    }

    internal bool IsThereASwingLock()
    {
        if (selectedItemModel && selectedItem.GetComponent<EquipableItem>())
        {
            return selectedItemModel.GetComponent<EquipableItem>().swingWait;
        }
        else
        {
            return false;
        }
    }
}