using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;

    public bool playerInRange;

    public int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip sheepHit;
    [SerializeField] AudioClip sheepDie;

    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem bloodParticle;

    public AnimalType animalType;
    public bool isDead;
    public enum AnimalType
    {
        Sheep,
        Wolf,
        Bear,
        Rabbit
    }
    public SheepNPC sheepNPC;
    public BearNPC bearNPC;


    private void Start()
    {
        currentHealth = maxHealth;
        switch (animalType)
        {
            case AnimalType.Sheep:
                sheepNPC = gameObject.GetComponent<SheepNPC>();
                break;
            case AnimalType.Bear:
                bearNPC = gameObject.GetComponent<BearNPC>();
                break;
            default:
                break;
        }
        
    }

    public void TakeDamageSheep(int damage)
    {
        if (!isDead)
        {
            GetComponentInChildren<Animator>().SetBool("isRunning",true);
            currentHealth -= damage;
            bloodParticle.Play();

            sheepNPC.isAnimalRunning = true;

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                GetComponentInChildren<SheepNPC>().enabled = false;

                isDead = true;
                sheepNPC.isAnimalDead = true;
                GetComponentInChildren<Animator>().SetTrigger("DIE");
            }
            else
            {
                PlayHitSound();
            }
        }
    }

    public void TakeDamageBear(int damage)
    {
        if (!isDead)
        {
            GetComponent<BearNPC>().SmoothRotateTowardsPlayer();
            GetComponentInChildren<Animator>().SetBool("isRunning",true);
            currentHealth -= damage;
            bloodParticle.Play();

            bearNPC.isAnimalRunning = true;

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                GetComponentInChildren<BearNPC>().enabled = false;

                isDead = true;
                GetComponentInChildren<Animator>().SetTrigger("DIE");
            }
            else
            {
                PlayHitSound();
            }
        }
    }


    private void PlayDyingSound()
    {
        switch (animalType)
        {
            case AnimalType.Sheep:
                soundChannel.PlayOneShot(sheepDie);
                break;
            default:
                break;
        }

    }

    private void PlayHitSound()
    {
        switch (animalType)
        {
            case AnimalType.Sheep:
                soundChannel.PlayOneShot(sheepHit);
                break;
            default:
                break;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}



