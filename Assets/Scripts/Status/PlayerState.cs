using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    CharacterState currentState;
    public static PlayerState Instance { get; set; }


    [Header("HealthBar Variables")]
    public float currentHealth;
    public float maxHealth;

    [Header("FoodBar Variables")]
    public float currentFullnes;//tokluk
    public float maxFullnes;

    public float fullnesDistanceTreshold;//tetikleyici mesafe
    public float fullnesDecreaseAmount;


    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject player;

    [Header("EnergyBar Variables")]
    public float currentEnergy;//tokluk
    public float maxEnergy;

    float minEnergy = 0f;
    public float energyDecreaseAmount;
    public float jumpEnergyDecreaseAmount;

    private float energyLoss;
    private float jumpEnergyLoss;

    [Header("Oxygene Variables")]
    public float currentOxygenPercent;
    public float maxOxygenPercent = 100;
    public float oxygenDecreasedPerSecond = 5f;
    private float oxygenTimer = 0f;
    private float decreaseInterval = 1f;

    public float outOfAirDamagePerSecond = 5f;


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
        currentHealth = maxHealth;
        currentFullnes = maxFullnes;
        currentEnergy = maxEnergy;
        currentOxygenPercent = maxOxygenPercent;

        energyLoss = energyDecreaseAmount * Time.deltaTime;
        jumpEnergyLoss = jumpEnergyDecreaseAmount * Time.deltaTime;
    }


    void Update()
    {
        if(player.GetComponent<PlayerMovementController>().isUnderWater)
        {
            oxygenTimer += Time.deltaTime;

            if (oxygenTimer >= decreaseInterval)
            {
                DecreaseOxygen();
                oxygenTimer = 0;
            }
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth -=10;
            currentFullnes -=10;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = CharacterState.Jump;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentState = CharacterState.Run;
            }
            else
            {
                currentState = CharacterState.Walk;
            }
        }
        else
        {
            currentState = CharacterState.Idle;
        }



        distanceTravelled += Vector3.Distance(player.transform.position, lastPosition);
        lastPosition = player.transform.position;

        if (distanceTravelled >= fullnesDistanceTreshold)
        {
            distanceTravelled = 0;
            currentFullnes -= fullnesDecreaseAmount;
        }



        if (currentState == CharacterState.Run)
        {
            currentEnergy -= energyLoss;
        }
        else if (currentState == CharacterState.Jump)
        {
            currentEnergy -= jumpEnergyLoss;
        }
        
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + energyLoss, maxEnergy);
        }

        if(currentEnergy<minEnergy)             //SET MINIMUM ENERGY
        {
            currentEnergy = minEnergy;
        }

    }

    private void DecreaseOxygen()                                          //DECREASE HEALTH
    {
        currentOxygenPercent -= oxygenDecreasedPerSecond;

        if (currentOxygenPercent < 0)
        {
            currentOxygenPercent = 0;
            setHealth(currentHealth - outOfAirDamagePerSecond); 
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentFullnes = newCalories;
    }
}

public enum CharacterState
{
    Idle,
    Walk,
    Run,
    Jump
}
