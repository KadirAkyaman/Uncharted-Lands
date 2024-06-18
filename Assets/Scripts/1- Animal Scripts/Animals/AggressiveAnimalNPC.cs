using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AggressiveAnimalNPC : AnimalNPC
{
    public abstract void Move();
    public abstract void Idle();
    public abstract void Chase();
    public abstract void Run();
    public abstract void Attack();
    public abstract void Die();
}
