using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    private string typeOfAnimal;
    public string TypeOfAnimal
    {
        get { return typeOfAnimal; }
        set { typeOfAnimal = value; }
    }

    [SerializeField]
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    private bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
}