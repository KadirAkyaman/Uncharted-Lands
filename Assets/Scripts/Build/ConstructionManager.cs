using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; set; }
 
    public GameObject itemToBeConstructed;
    public bool inConstructionMode = false;
    public GameObject constructionHoldingSpot;
 
    public bool isValidPlacement;
 
    public bool selectingAGhost;
    public GameObject selectedGhost;
 

    public Material ghostSelectedMat;
    public Material ghostSemiTransparentMat;
    public Material ghostFullTransparentMat;

    public List<GameObject> allGhostsInExistence = new List<GameObject>();

    public GameObject itemToBeDestroyed;

    public GameObject constructionModeUI;

    public GameObject player;
 
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
 
    public void ActivateConstructionPlacement(string itemToConstruct)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToConstruct));
 
      
        item.name = itemToConstruct;
 
        item.transform.SetParent(constructionHoldingSpot.transform, false);
        itemToBeConstructed = item;
        itemToBeConstructed.gameObject.tag = "activeConstructable";

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;

        inConstructionMode = true;
    }
 
    private void GetAllGhosts(GameObject itemToBeConstructed)
    {
        List<GameObject> ghostlist = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;
 
        foreach (GameObject ghost in ghostlist)
        {
            Debug.Log(ghost);
            allGhostsInExistence.Add(ghost);
        }
    }
 
    private void PerformGhostDeletionScan()
    {
 
        foreach (GameObject ghost in allGhostsInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition == false) 
                {
                    foreach (GameObject ghostX in allGhostsInExistence)
                    {
            
                        if (ghost.gameObject != ghostX.gameObject)
                        {

                            if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX))
                            {
                                if (ghost != null && ghostX != null)
                                {
                                    ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                    break;
                                }
 
                            }
 
                        }
 
                    }
 
                }
            }
        }
 
        foreach (GameObject ghost in allGhostsInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition)
                {
                    DestroyImmediate(ghost);
                }
            }
 
        }
    }
 
    private float XPositionToAccurateFloat(GameObject ghost)
    {
        if (ghost != null)
        {
            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.x;
            float xFloat = Mathf.Round(pos * 100f) / 100f;
            return xFloat;
        }
        return 0;
    }
 
    private float ZPositionToAccurateFloat(GameObject ghost)
    {
 
        if (ghost != null)
        {
  
            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.z;
            float zFloat = Mathf.Round(pos * 100f) / 100f;
            return zFloat;
 
        }
        return 0;
    }
 
    private void Update()
    {
        if (inConstructionMode)
        {
            constructionModeUI.SetActive(true);
        }
        else
        {
            constructionModeUI.SetActive(false);
        }

 
        if (itemToBeConstructed != null && inConstructionMode)
        {
            if (itemToBeConstructed.name == "FoundationModel")
            {
                if (CheckValidConstructionPosition())
                {
                    isValidPlacement = true;
                    itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                }
                else
                {
                    isValidPlacement = false;
                    itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                }
            }

            
 
 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selectionTransform = hit.transform;
                if (selectionTransform.gameObject.CompareTag("ghost") && itemToBeConstructed.name == "FoundationModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;
                }
                else if (selectionTransform.gameObject.CompareTag("wallGhost") && itemToBeConstructed.name == "WallModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;
                }
                else
                {
                    itemToBeConstructed.SetActive(true);
                    selectedGhost = null;
                    selectingAGhost = false;
                }
 
            }
        }
 

        if (Input.GetMouseButtonDown(0) && inConstructionMode)
        {
            if (isValidPlacement && selectedGhost == false && itemToBeConstructed.name == "FoundationModel") 
            {
                PlaceItemFreeStyle();
                DestroyItem(itemToBeDestroyed);
            }
 
            if (selectingAGhost)
            {
                PlaceItemInGhostPosition(selectedGhost);
                DestroyItem(itemToBeDestroyed);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && constructionModeUI.activeSelf == true && inConstructionMode)
        {     
            
            itemToBeDestroyed.SetActive(true);
            itemToBeDestroyed = null;
            DestroyItem(itemToBeConstructed);

            itemToBeConstructed = null;
            inConstructionMode = false;
        }
    }
 
    private void PlaceItemInGhostPosition(GameObject copyOfGhost)
    {
 
        Vector3 ghostPosition = copyOfGhost.transform.position;
        Quaternion ghostRotation = copyOfGhost.transform.rotation;
 
        selectedGhost.gameObject.SetActive(false);

        itemToBeConstructed.gameObject.SetActive(true);

        itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);
 
        var randomOffset = UnityEngine.Random.Range(0.01f, 0.03f);


        itemToBeConstructed.transform.position = new Vector3(ghostPosition.x, ghostPosition.y, ghostPosition.z+randomOffset);
        itemToBeConstructed.transform.rotation = ghostRotation;
 
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();


        if (itemToBeConstructed.name == "FoundationModel")
        {
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            itemToBeConstructed.tag = "placedFoundation";

            GetAllGhosts(itemToBeConstructed);
            PerformGhostDeletionScan();
        }
        else
        {
            itemToBeConstructed.tag = "placedWall";
            DestroyItem(selectedGhost);
        }
        itemToBeConstructed = null;
 
        inConstructionMode = false;
    }
 
 
    private void PlaceItemFreeStyle()
    {
        itemToBeConstructed.transform.SetParent(transform.parent.transform.parent, true);
 
        itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();

        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
        itemToBeConstructed.tag = "placedFoundation";
        itemToBeConstructed.GetComponent<Constructable>().enabled = false;

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;

        GetAllGhosts(itemToBeConstructed);
        PerformGhostDeletionScan();
 
        itemToBeConstructed = null;
 
        inConstructionMode = false;
    }
 
    private bool CheckValidConstructionPosition()
    {
        if (itemToBeConstructed != null)
        {
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }
 
        return false;
    }

    void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReSizeList();
        CraftingSystem.Instance.RefreshInventory();
    }
}