using System;
using System.Collections;
using System.Collections.Generic;
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
    public Image handImage;

    public bool handIsVisible;

    public GameObject selectedTree; //TREE PART
    public GameObject chopHolder;

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



            if (selectedObjectsInteractable && selectedObjectsInteractable.PlayerInRange)
            {
                onTarget = true;
                SelectedObject = selectedObjectsInteractable.gameObject;
                interaction_text.text = selectedObjectsInteractable.GetItemName();
                interaction_Info_UI.SetActive(true);


                if (selectedObjectsInteractable.CompareTag("Pickable"))
                {
                    CenterDotImage.gameObject.SetActive(false);
                    handImage.gameObject.SetActive(true);

                    handIsVisible = true;
                }
                else
                {
                    CenterDotImage.gameObject.SetActive(true);
                    handImage.gameObject.SetActive(false);

                    handIsVisible = false;
                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                CenterDotImage.gameObject.SetActive(true);
                handImage.gameObject.SetActive(false);

                handIsVisible = false;
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            CenterDotImage.gameObject.SetActive(true);
            handImage.gameObject.SetActive(false);

            handIsVisible = false;
        }
    }

    public void DisableSelection()
    {
        handImage.enabled = false;
        CenterDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);
        SelectedObject = null;
    }

    public void EnableSelection()
    {
        handImage.enabled = true;
        CenterDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}