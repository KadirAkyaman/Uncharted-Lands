using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeItems : MonoBehaviour
{
    bool gmItemsVisible = false;

    public GameObject gmItems;

    public GameObject player;

    Vector3 playerBasePos;
    CharacterController controller;
    private void Start()
    {
        gmItems.SetActive(false);
        playerBasePos = new Vector3(434.45f, 4.21f, 869.13f);
        controller = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !gmItemsVisible)
        {
            //gmItemsVisible = true;
            gmItems.SetActive(true);
            gmItemsVisible = true;
        }

        else if (Input.GetKeyDown(KeyCode.G) && gmItemsVisible)
        {
            //gmItemsVisible = false;
            gmItems.SetActive(false);
            gmItemsVisible = false;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            controller.enabled = false; // Önce CharacterController'ı devre dışı bırakın

            controller.transform.position = playerBasePos; // Karakterin pozisyonunu ayarlayın

            controller.enabled = true;
        }
    }
}

