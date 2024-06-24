using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, processScreenUI, buildingScreenUI;

    //public List<string> InventoryItemList = new List<string>();
    public List<string> InventoryItemList;
    //Category
    private Button toolsButton, survivalButton, processButton, buildingButton;

    //Craft
    private Button craftAxeButton, craftPlankButton, craftFoundationButton, craftWallButton, craftCampfireButton, craftBedButton, craftTorchButton;

    //Req
    Text axeReq1, axeReq2, plankReq1, foundationReq1, wallReq1, campfireReq1, campfireReq2, bedReq1, bedReq2, torchReq1, torchReq2;

    public bool isOpen;

    //CRAFTING RECIPE
    public CraftingRecipe AxeRecipe = new CraftingRecipe("Axe",1 , 2, "Stone", 3, "Stick", 2);
    public CraftingRecipe PlankRecipe = new CraftingRecipe("Plank",2 , 1, "Log", 1, "", 0);

    public CraftingRecipe FoundationRecipe = new CraftingRecipe("Foundation",1 , 1, "Plank", 4, "", 0);

    public CraftingRecipe WallRecipe = new CraftingRecipe("Wall",1 , 1, "Plank", 2, "", 0);

    public CraftingRecipe CampfireRecipe = new CraftingRecipe("Campfire", 1, 2, "Stone", 4, "Stick", 4);
    public CraftingRecipe BedRecipe = new CraftingRecipe("Bed", 1, 2, "Plank", 2, "Wool", 2);
    public CraftingRecipe TorchRecipe = new CraftingRecipe("Torch", 1, 2, "Flint", 1, "Stick", 1);

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
        
        toolsButton = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsButton.onClick.AddListener(delegate { OpenToolsPanel(); });//butona tiklandiginda metodu calistir

        buildingButton = craftingScreenUI.transform.Find("BuildingButton").GetComponent<Button>();
        buildingButton.onClick.AddListener(delegate { OpenBuildingPanel(); });//butona tiklandiginda metodu calistir

        survivalButton = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalButton.onClick.AddListener(delegate { OpenSurvivalPanel(); });//butona tiklandiginda metodu calistir

        processButton = craftingScreenUI.transform.Find("ProcessButton").GetComponent<Button>();
        processButton.onClick.AddListener(delegate { OpenProcessPanel(); });//butona tiklandiginda metodu calistir


        //AXE REQ
        axeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        axeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeButton = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate { CraftItem(AxeRecipe); });

        //PLANK REQ
        plankReq1 = processScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();

        craftPlankButton = processScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankButton.onClick.AddListener(delegate { CraftItem(PlankRecipe); });

        //FOUNDATION REQ
        foundationReq1 = buildingScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();

        craftFoundationButton = buildingScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationButton.onClick.AddListener(delegate { CraftItem(FoundationRecipe); });

        //WALL REQ
        wallReq1 = buildingScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();

        craftWallButton = buildingScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallButton.onClick.AddListener(delegate { CraftItem(WallRecipe); });

        //CAMPFIRE REQ
        campfireReq1 = survivalScreenUI.transform.Find("Campfire").transform.Find("req1").GetComponent<Text>();
        campfireReq2 = survivalScreenUI.transform.Find("Campfire").transform.Find("req2").GetComponent<Text>();

        craftCampfireButton = survivalScreenUI.transform.Find("Campfire").transform.Find("Button").GetComponent<Button>();
        craftCampfireButton.onClick.AddListener(delegate { CraftItem(CampfireRecipe); });

        //BED REQ
        bedReq1 = survivalScreenUI.transform.Find("Bed").transform.Find("req1").GetComponent<Text>();
        bedReq2 = survivalScreenUI.transform.Find("Bed").transform.Find("req2").GetComponent<Text>();

        craftBedButton = survivalScreenUI.transform.Find("Bed").transform.Find("Button").GetComponent<Button>();
        craftBedButton.onClick.AddListener(delegate { CraftItem(BedRecipe); });

        //TORCH REQ
        torchReq1 = survivalScreenUI.transform.Find("Torch").transform.Find("req1").GetComponent<Text>();
        torchReq2 = survivalScreenUI.transform.Find("Torch").transform.Find("req2").GetComponent<Text>();

        craftTorchButton = survivalScreenUI.transform.Find("Torch").transform.Find("Button").GetComponent<Button>();
        craftTorchButton.onClick.AddListener(delegate { CraftItem(TorchRecipe); });
    }

    private void CraftItem(CraftingRecipe recipeToCraft)//ESYALARI CRAFT ETME BOLUMU
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);
        //ESYALARIN SAYISINA GORE ENVANTERE EKLEMEK
        StartCoroutine(craftedDelayForSound(recipeToCraft));

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
        craftingScreenUI.SetActive(false);
        processScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        buildingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    void OpenBuildingPanel()
    {
        craftingScreenUI.SetActive(false);
        processScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        buildingScreenUI.SetActive(true);
    }
    void OpenSurvivalPanel()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        processScreenUI.SetActive(false);
        buildingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }

    void OpenProcessPanel()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        buildingScreenUI.SetActive(false);
        processScreenUI.SetActive(true);
    }
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            buildingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            processScreenUI.SetActive(false);

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
        int logCount = 0;
        int plankCount = 0;
        int woolCount = 0;
        int flintCount = 0;

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

                case "Log":
                    logCount += 1;
                    break;   

                case "Plank":
                    plankCount +=1;
                    break; 

                case "Wool":
                    woolCount +=1;
                    break; 

                case "Flint":
                    flintCount +=1;
                    break;
            }
        }

        //AXE
        axeReq1.text = "3 Stone [" + stoneCount + "]";
        axeReq2.text = "2 Stick [" + stickCount + "]";

        if (stoneCount >= 3 && stickCount >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeButton.gameObject.SetActive(true);
        }
        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

        //PLANK
        plankReq1.text = "1 Log [" + logCount + "]";

        if (logCount >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankButton.gameObject.SetActive(true);
        }
        else
        {
            craftPlankButton.gameObject.SetActive(false);
        }

        //FOUNDATION
        foundationReq1.text = "4 Plank [" + plankCount + "]";

        if (plankCount >= 4 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftFoundationButton.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationButton.gameObject.SetActive(false);
        }

        //WALL
        wallReq1.text = "2 Plank [" + plankCount + "]";

        if (plankCount >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftWallButton.gameObject.SetActive(true);
        }
        else
        {
            craftWallButton.gameObject.SetActive(false);
        }

        //CAMPFIRE
        campfireReq1.text = "4 Stone [" + stoneCount + "]";
        campfireReq2.text = "4 Stick [" + stickCount + "]";

        if (stoneCount >= 4 && stickCount >= 4 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftCampfireButton.gameObject.SetActive(true);
        }
        else
        {
            craftCampfireButton.gameObject.SetActive(false);
        }

        //BED
        bedReq1.text = "2 Plank [" + plankCount + "]";
        bedReq2.text = "2 Wool [" + woolCount + "]";

        if (plankCount >= 2 && woolCount >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftBedButton.gameObject.SetActive(true);
        }
        else
        {
            craftBedButton.gameObject.SetActive(false);
        }

        //TORCH
        torchReq1.text = "1 Flint [" + flintCount + "]";
        torchReq2.text = "1 Stick [" + stickCount + "]";

        if (flintCount >= 1 && stickCount >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftTorchButton.gameObject.SetActive(true);
        }
        else
        {
            craftTorchButton.gameObject.SetActive(false);
        }
    }

    public IEnumerator calculate()
    {
        yield return 0;
        InventorySystem.Instance.ReSizeList();
        RefreshInventory();
    }

    IEnumerator craftedDelayForSound(CraftingRecipe recipeToCraft) 
    {
        yield return new WaitForSeconds(1f);
        
        for (var i = 0; i < recipeToCraft.numOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(recipeToCraft.ItemName);
        }
    }
}
