using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public float viewDistance = 50f;
    private Camera characterCamera;

    private void Start()
    {
        characterCamera = Camera.main;
    }

    private void Update()
    {
        characterCamera.farClipPlane = viewDistance;
    }
}
