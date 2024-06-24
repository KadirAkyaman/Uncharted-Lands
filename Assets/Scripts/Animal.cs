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
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip die;

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

    public AudioSource animalSound;


    private void Start()
    {
        animalSound.pitch = Random.Range(1f, 2f);
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
        if (!isDead && PlayerState.Instance.currentEnergy >=10 )
        {
            GetComponentInChildren<Animator>().SetBool("isRunning",true);
            currentHealth -= damage;
            PlayerState.Instance.currentEnergy-=10;
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
        if (!isDead && PlayerState.Instance.currentEnergy >=10)
        {
            GetComponent<BearNPC>().SmoothRotateTowardsPlayer();
            GetComponentInChildren<Animator>().SetBool("isRunning",true);
            currentHealth -= damage;
            bloodParticle.Play();
            PlayerState.Instance.currentEnergy-=10;
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
        soundChannel.PlayOneShot(die);

    }

    private void PlayHitSound()
    {
        soundChannel.PlayOneShot(hit);

        if (animalType == AnimalType.Sheep)
        {
        
        animalSound.Play();
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



