using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : MonoBehaviour, ISense
{
    public GameObject player;
    public abstract void InitializeSense();

    public abstract void UpdateSense();

    private void Start() {
        InitializeSense();
        player = GameObject.Find("Player");
    }

    private void Update() {
        UpdateSense();
    }

}
