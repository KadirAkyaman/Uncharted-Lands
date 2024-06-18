using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonAggressiveAnimalNPC : AnimalNPC
{
    public abstract void Move();
    public abstract void Idle();
    public abstract void Run();
    public abstract void Die();
}
