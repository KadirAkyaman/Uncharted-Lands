using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    CharacterState currentState;
    public static PlayerState Instance { get; set; }

    [Header("HealthBar Variables")]
    public float currentHealth;
    public float maxHealth;

    [Header("FoodBar Variables")]
    public float currentFullnes; // tokluk
    public float maxFullnes;

    public float fullnesDistanceTreshold; // tetikleyici mesafe
    public float fullnesDecreaseAmount;

    float distanceTravelled = 0;

    public bool isPlayerHungry;
    Vector3 lastPosition;

    public GameObject player;

    [Header("EnergyBar Variables")]
    public float currentEnergy; // tokluk
    public float maxEnergy;

    float minEnergy = 0f;
    public float energyIncreaseAmount;
    private float maxEnergyIncreaseAmount;
    public float jumpEnergyDecreaseAmount;
    public float jumpEnergyLoss;

    public bool canJump;
    public bool canRun;

    [Header("Oxygene Variables")]
    public float currentOxygenPercent;
    public float maxOxygenPercent = 100;
    public float oxygenDecreasedPerSecond = 5f;
    private float oxygenTimer = 0f;
    private float decreaseInterval = 1f;

    public float outOfAirDamagePerSecond = 5f;

    public Animator hitEffectAnimator;

    private float fullnessElapsedTimeForIncreaseHealth = 0;

    private bool isHearthSoundsPlaying = false;

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
        InitializePlayerState();
        
    }

    void Update()
    {
        UpdateOxygen();
        UpdateState();
        UpdateHunger();
        UpdateDistanceTravelled();
        UpdateEnergy();
        UpdateJumpAndRun();
        LimitEnergyIncreaseSpeedWhenHungry();

        if(currentHealth<=0)
            SceneManager.LoadScene("Dead");

        hitEffectAnimator.SetFloat("playerHealth",currentHealth);

        if(currentFullnes>=90 && currentHealth< 100)
        {
            if ((fullnessElapsedTimeForIncreaseHealth += Time.deltaTime) > 2f)
            {
                currentHealth++;
                fullnessElapsedTimeForIncreaseHealth = 0;
            }
        }

        if (currentHealth<20 && !isHearthSoundsPlaying)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.hearthSound);
            isHearthSoundsPlaying = true;
        }
        else if (currentHealth>20 && isHearthSoundsPlaying)
        {
            SoundManager.Instance.hearthSound.Stop();
            isHearthSoundsPlaying = false;
        }
    }


    private void InitializePlayerState()
    {
        currentHealth = maxHealth;
        currentFullnes = maxFullnes;
        currentEnergy = maxEnergy;
        currentOxygenPercent = maxOxygenPercent;
        canJump = true;
        canRun = true;
        isPlayerHungry = false;
        maxEnergyIncreaseAmount = energyIncreaseAmount;
        jumpEnergyLoss = jumpEnergyDecreaseAmount * Time.deltaTime;
    }

        private void LimitEnergyIncreaseSpeedWhenHungry()
    {
        if(currentFullnes < maxFullnes/2)
        {
            energyIncreaseAmount = maxEnergyIncreaseAmount/2;
        }
        else
        {
            energyIncreaseAmount = maxEnergyIncreaseAmount;
        }
    }

    private void UpdateOxygen()
    {
        if (player.GetComponent<PlayerMovementController>().isUnderWater)
        {
            oxygenTimer += Time.deltaTime;

            if (oxygenTimer >= decreaseInterval)
            {
                DecreaseOxygen();
                oxygenTimer = 0f;
            }
        }
    }

    private void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = CharacterState.Jump;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            currentState = Input.GetKey(KeyCode.LeftShift) ? CharacterState.Run : CharacterState.Walk;
        }
        else
        {
            currentState = CharacterState.Idle;
        }
    }

    private void UpdateHunger()
    {
        isPlayerHungry = currentFullnes < 20;
    }

    private void UpdateDistanceTravelled()
    {
        distanceTravelled += Vector3.Distance(player.transform.position, lastPosition);
        lastPosition = player.transform.position;

        if (distanceTravelled >= fullnesDistanceTreshold)
        {
            distanceTravelled = 0;
            currentFullnes -= fullnesDecreaseAmount;
        }
    }

    private void UpdateEnergy()
    {
        if (currentState == CharacterState.Run)
        {
            currentEnergy -= energyIncreaseAmount * 1.4f;
        }

        if (currentEnergy < maxEnergy && !isPlayerHungry)
        {
            currentEnergy = Mathf.Min(currentEnergy + energyIncreaseAmount, maxEnergy);
        }

        if (currentEnergy < minEnergy)
        {
            currentEnergy = minEnergy;
        }
    }

    private void UpdateJumpAndRun()
    {
        canJump = currentEnergy >= jumpEnergyLoss;
        canRun = currentEnergy > maxEnergy / 8;
    }

    private void DecreaseOxygen()
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
