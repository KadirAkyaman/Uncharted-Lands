using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; set; }

    public GameObject placementHoldingSpot; // Drag our construcionHoldingSpot or a new placementHoldingSpot
    public GameObject enviromentPlaceables;


    public bool inPlacementMode;
    [SerializeField] bool isValidPlacement;

    [SerializeField] GameObject itemToBePlaced;
    public GameObject inventoryItemToDestory;

    [SerializeField] GameObject placementModeUI;

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

    public void ActivatePlacementMode(string itemToPlace)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToPlace));

        // Changing the name of the gameobject so it will not be (clone)
        item.name = itemToPlace;

        // Setting the item to be a child of our placement holding spot
        item.transform.SetParent(placementHoldingSpot.transform, false);

        // Saving a reference to the item we want to place
        itemToBePlaced = item;

        // Actiavting Construction mode
        inPlacementMode = true;
    }



    private void Update()
    {

        if (inPlacementMode)
        {
           placementModeUI.SetActive(true);
        }
        else
        {
            placementModeUI.SetActive(false);
        }

        if (itemToBePlaced != null && inPlacementMode)
        {
            if (IsCheckValidPlacement())
            {
                isValidPlacement = true;
                itemToBePlaced.GetComponent<PlacebleItem>().SetValidColor();
            }
            else
            {
                isValidPlacement = false;
                itemToBePlaced.GetComponent<PlacebleItem>().SetInvalidColor();
            }
        }

        // Left Mouse Click to Place item
        if (Input.GetMouseButtonDown(0) && inPlacementMode && isValidPlacement)
        {
            PlaceItemFreeStyle();
            DestroyItem(inventoryItemToDestory);
        }

        // Cancel Placement                     //TODO - don't destroy the ui item until you actually placed it.
        if (Input.GetKeyDown(KeyCode.Escape) && inPlacementMode)
        {
            inventoryItemToDestory.SetActive(true);
            inventoryItemToDestory = null;
            DestroyItem(itemToBePlaced);
            itemToBePlaced = null;
            inPlacementMode = false;
        }
    }

    private bool IsCheckValidPlacement()
    {
        if (itemToBePlaced != null)
        {
            return itemToBePlaced.GetComponent<PlacebleItem>().isValidToBeBuilt;
        }

        return false;
    }

    private void PlaceItemFreeStyle()
    {
        // Setting the parent to be the root of our scene
        itemToBePlaced.transform.SetParent(enviromentPlaceables.transform, true);

        // Setting the default color/material
        itemToBePlaced.GetComponent<PlacebleItem>().SetDefaultColor();
        itemToBePlaced.GetComponent<PlacebleItem>().enabled = false;

        itemToBePlaced = null;

        inPlacementMode = false;
    }

    private void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReSizeList();
        CraftingSystem.Instance.RefreshInventory();
    }
}