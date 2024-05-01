using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float YRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //Yukari Asagi
            xRotation -= mouseY;

            //Clamp
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Saga Sola
            YRotation += mouseX;

            //applying both rotations
            transform.localRotation = Quaternion.Euler(xRotation, YRotation, 0f);
        }

    }
}
