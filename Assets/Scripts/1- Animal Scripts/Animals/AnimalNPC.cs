using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalNPC : MonoBehaviour
{
    [Header("Speed")]
    public float speed;

    [Header("Aggression and Damage")]
    public bool isAggressiveAnimal;
    public int damage;

    [Header("Components")]
    public Animator animator;
    public Rigidbody animalRb;

    public NavMeshAgent agent { get { return GetComponent<NavMeshAgent>(); } }
}
