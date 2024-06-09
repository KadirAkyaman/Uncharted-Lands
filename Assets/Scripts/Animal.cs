using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;

    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip sheepHit;
    [SerializeField] AudioClip sheepDie;

    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem bloodParticle;

    public bool isDead;
    enum AnimalType
    {
        Sheep,
        Wolf,
        Bear,
        Rabbit
    }

    [SerializeField] AnimalType thisAnimalType;
    public AnimalController animalController;


    private void Start()
    {
        currentHealth = maxHealth;
        animalController = gameObject.GetComponent<AnimalController>();
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            bloodParticle.Play();

            animalController.isAnimalRunning = true;
            animalController.isDamaged = true;
            animalController.HandleRunningState();

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                GetComponent<AnimalController>().enabled = false;

                isDead = true;
                animalController.isAnimalDead = true;
                animalController.HandleDeadState();
            }
            else
            {
                PlayHitSound();
            }
        }
    }


    private void PlayDyingSound()
    {
        switch (thisAnimalType)
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
        switch (thisAnimalType)
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



