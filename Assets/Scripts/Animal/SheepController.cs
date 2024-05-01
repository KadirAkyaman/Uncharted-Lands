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
    Fleeing, // Kaçma
    Walking, // Yürümek
    Eating,  // Yemek yeme
    Idle,    // Bekleme
    Dead     // Ölüm
}