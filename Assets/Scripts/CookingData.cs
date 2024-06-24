using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingData", menuName = "ScriptableObjects/CookingData", order = 1)]
public class CookingData : ScriptableObject
{
    public List<string> validFuels = new List<string>();

    public List<CookableFood> validFoods = new List<CookableFood>();
}

[System.Serializable]
public class CookableFood
{
    public string name;
    public float timeToCook;
    public string cookedFoodName;
    
}