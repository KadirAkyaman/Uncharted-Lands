using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject interaction_Info_UI;
    public GameObject SelectedObject;

    Text interaction_text;


    public Image CenterDotImage;
    public Image handIcon;

    public bool handIsVisible;

    public GameObject selectedTree; //TREE PART
    public GameObject chopHolder;

    public GameObject selectedCampfire;

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
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject selectedObjectsInteractable = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();                          //TREE

            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree!=null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.PlayerInRange)
            {
                onTarget = true;
                SelectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                CenterDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;
            }

            BedController bed = selectionTransform.GetComponent<BedController>();

            if (bed && bed.playerInRange)
            {
                onTarget = true;
                SelectedObject = bed.gameObject;
                interaction_text.text = bed.GetItemName();
                interaction_Info_UI.SetActive(true);

                CenterDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;
            }


            Campfire campfire = selectionTransform.GetComponent<Campfire>();

            if (campfire && campfire.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Cook";
                interaction_Info_UI.SetActive(true);

                selectedCampfire = campfire.gameObject;

                if (Input.GetMouseButtonDown(0) && campfire.isCooking == false)
                {
                    campfire.OpenUI();
                }
            }
            else
            {
                if (selectedCampfire != null)
                {
                    selectedCampfire = null;
                }
            }


            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerInRange)
            {
                if (animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);

                    CenterDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    CenterDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon()&& EquipSystem.Instance.IsThereASwingLock() == false)
                    {
                        StartCoroutine(DealDamageTo(animal, 0.4f, EquipSystem.Instance.GetWeaponDamage()));
                    }
                }

            }

            if (!interactable && !animal && !bed)
            {
                onTarget=false;
                handIsVisible = false;

                CenterDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if (!interactable && !animal && !choppableTree && !campfire && !bed)
            {
                interaction_text.text = "";
                interaction_Info_UI.gameObject.SetActive(false);
            }
        }
    }

    private void Loot(Lootable lootable)
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();
            foreach (LootPossibility loot in lootable.possibleLoot)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin,loot.amountMax+1);
                if (lootAmount!=0)
                {
                    LootRecieved lt = new LootRecieved();
                    lt.item = loot.item;
                    lt.amount = lootAmount;

                    recievedLoot.Add(lt);
                }
            }

            lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }
        Vector3 lootSpawnPos = lootable.gameObject.transform.position;
        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name), new Vector3(lootSpawnPos.x, lootSpawnPos.y+0.2f, lootSpawnPos.z),Quaternion.Euler(0,0,0));
            }
        }
        Destroy(lootable.gameObject);

    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        switch (animal.animalType)
        {
            case Animal.AnimalType.Sheep : 
                animal.TakeDamageSheep(damage);
                break;

            case Animal.AnimalType.Bear : 
                animal.TakeDamageBear(damage);
                break;


            default:
                break;
        }
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        CenterDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);
        SelectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        CenterDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}