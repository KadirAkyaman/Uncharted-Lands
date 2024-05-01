using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject CraftingScreenUI;
    public GameObject ToolsScreenUI;

    public List<string> InventoryItemList = new List<string>();

    //Category
    private Button toolsButton;

    //Craft
    private Button craftAxeButton;

    //Req
    Text axeReq1, axeReq2;

    public bool isOpen;

    //CRAFTING RECIPE
    public CraftingRecipe AxeRecipe = new CraftingRecipe("Axe", 2, "Stone", 3, "Stick", 2);



    public static CraftingSystem Instance { get; set; }

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
        toolsButton = CraftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenToolsPanel(); });//butona tiklandiginda metodu calistir


        //AXE REQ
        axeReq1 = ToolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        axeReq2 = ToolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeButton = ToolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftItem(AxeRecipe); });
    }

    private void CraftItem(CraftingRecipe recipeToCraft)//ESYALARI CRAFT ETME BOLUMU
    {
        InventorySystem.Instance.AddToInventory(recipeToCraft.ItemName);

        if (recipeToCraft.numOfReqs == 1)//EGER CRAFT ICIN 1 ESYA GEREKLIYSE
        {
            InventorySystem.Instance.RemoveItem(recipeToCraft.Req1, recipeToCraft.Req1Amount);
        }
        else if (recipeToCraft.numOfReqs == 2)//EGER CRAFT ICIN 2 ESYA GEREKLIYSE
        {
            InventorySystem.Instance.RemoveItem(recipeToCraft.Req1, recipeToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(recipeToCraft.Req2, recipeToCraft.Req2Amount);
        }


        StartCoroutine(calculate());


        
    }

    void OpenToolsPanel()
    {
        CraftingScreenUI.SetActive(false);
        ToolsScreenUI.SetActive(true);
    }
    void Update()
    {
        


        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            CraftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            CraftingScreenUI.SetActive(false);
            ToolsScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }

    public void RefreshInventory()
    {
        int stoneCount = 0;//inventory icin gerekli malzemeler buraya eklenecek
        int stickCount = 0;

        InventoryItemList = InventorySystem.Instance.ItemList;

        foreach (string itemName in InventoryItemList)//kac tane objemiz oldugunu kontrol ediyor
        {
            switch (itemName)
            {
                case "Stone":
                    stoneCount += 1;
                    break;

                case "Stick":
                    stickCount += 1;
                    break;
            }
        }

        //AXE
        //axeReq1.text =  AxeRecipe.Req1Amount + " " + AxeRecipe.Req1 + " " + "[" + stoneCount + "]";
        //axeReq2.text =  AxeRecipe.Req2Amount + " " + AxeRecipe.Req2 + " " + "[" + stickCount + "]";

        axeReq1.text = "3 Stone [" + stoneCount + "]";
        axeReq2.text = "2 Stick [" + stickCount + "]";

        //if (stoneCount >= AxeRecipe.Req1Amount && stickCount >= AxeRecipe.Req2Amount)
        //{
        //    craftAxeButton.gameObject.SetActive(true);
        //}
        //else
        //{
        //    craftAxeButton.gameObject.SetActive(false);
        //}

        if (stoneCount >= 3 && stickCount >= 2)
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }
    }

    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReSizeList();
        RefreshInventory();
    }
}
