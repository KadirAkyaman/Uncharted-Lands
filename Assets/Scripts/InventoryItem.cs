using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    public bool isTrashable;


    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;

    //Equipping

    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;//quick slotlardaki item

    public bool isSelected;//secili item

    public bool isUsable;


    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<Text>();
    }

    private void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, caloriesEffect);
            }

            if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }

            if (isUsable)
            {
                gameObject.SetActive(false);
                UseItem();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReSizeList();
                CraftingSystem.Instance.RefreshInventory();
            }
        }
    }

    private void UseItem()
    {
        itemInfoUI.SetActive(false);

        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsScreenUI.SetActive(false);
        CraftingSystem.Instance.survivalScreenUI.SetActive(false);
        CraftingSystem.Instance.processScreenUI.SetActive(false);
        CraftingSystem.Instance.buildingScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;

            case "Foundation":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;

            case "Wall(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;

            case "Wall":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;

            case "Campfire(Clone)":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("CampfireModel");
                break;

            case "Campfire":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("CampfireModel");
                break;

            case "Bed(Clone)":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("BedModel");
                break;

            case "Bed":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("BedModel");
                break;

            default:
                break;
        }

    }

    private void consumingFunction(float healthEffect, float caloriesEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

    }


    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //

        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }


    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        // --- Calories --- //

        float caloriesBeforeConsumption = PlayerState.Instance.currentFullnes;
        float maxCalories = PlayerState.Instance.maxFullnes;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.eatSound);
                PlayerState.Instance.setCalories(maxCalories);
            }
            else if (caloriesBeforeConsumption == maxCalories)
            {
                //Do Nothing
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.eatSound);
                PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }
}