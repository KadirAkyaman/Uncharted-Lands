using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : Animal
{
    SheepState currentSheepState;
    private void Start()
    {
        currentSheepState = SheepState.Idle;
        IsAlive = true;
    }
}

public enum SheepState
{
    Fleeing, // Ka�ma
    Walking, // Y�r�mek
    Eating,  // Yemek yeme
    Idle,    // Bekleme
    Dead     // �l�m
}