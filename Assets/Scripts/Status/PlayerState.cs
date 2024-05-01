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

        energyLoss = energyDecreaseAmount * Time.deltaTime;
        jumpEnergyLoss = jumpEnergyDecreaseAmount * Time.deltaTime;
    }


    void Update()
    {
        //CharacterStateController
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
