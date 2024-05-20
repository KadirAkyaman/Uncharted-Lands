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


    private void Start() 
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -=damage;
            bloodParticle.Play();

            if (currentHealth<=0)
            {
                PlayDyingSound();
                animator.SetTrigger("DIE");
                GetComponent<AnimalController>().enabled = false;

                isDead = true;
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
            default :
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
            default :
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



